using System.Collections.Generic;
using System.Linq;
using UnityEngine;
// Github: Em3rgencyLT - em3rgency.LT@gmail.com
public class Hexasphere
{
    private readonly float _radius;
    private readonly int _divisions;
    private readonly float _hexSize;
    private readonly MeshDetails _meshDetails;
    private readonly List<Tile> _tiles;
    private readonly List<Point> _points;
    private readonly List<Face> _icosahedronFaces;
    public Material sectorMaterial;
    public Hexasphere(float radius, int divisions, float hexSize, Material sectorMaterial, GameObject sectors)
    {
        this.sectorMaterial = sectorMaterial;

        _radius = radius;
        _divisions = divisions;
        _hexSize = hexSize;
        _tiles = new List<Tile>();
        _points = new List<Point>();
        _icosahedronFaces = ConstructIcosahedron();
        SubdivideIcosahedron();
        ConstructTiles(sectors);
        _meshDetails = StoreMeshDetails();
    }
    public List<Tile> Tiles => _tiles;
    public MeshDetails MeshDetails => _meshDetails;
    public string ToJson()
    {
        return $"{{\"radius\":{_radius},\"tiles\":[{string.Join(",", _tiles.Select(tile => tile.ToJson()))}]}}";
    }
    public string ToObj()
    {
        string objString = $"#Hexasphere. Radius {_radius}, divisions {_divisions}, hexagons scaled to {_hexSize}\n";
        objString += string.Join("\n", _meshDetails.Vertices.Select(vertex => $"v {vertex.x} {vertex.y} {vertex.z}"));
        //+1 to all values as .obj indexes start from 1 -.-
        List<int> offsetTriangles = _meshDetails.Triangles.Select(index => index + 1).ToList();
        for (var i = 0; i < offsetTriangles.Count; i += 3) objString += $"f {offsetTriangles[i]} {offsetTriangles[i + 1]} {offsetTriangles[i + 2]}\n";
        return objString;
    }
    private void ConstructTiles(GameObject sectors)
    {
        for (int i = 0; i < _points.Count; i++)
        {
            // Create a GameObject for each tileq
            GameObject tileObject = new GameObject($"Sector {i}");
            tileObject.transform.position = _points[i].Position * 0;
            tileObject.transform.parent = sectors.transform;
            Tile tile = new Tile(_points[i], _radius, _hexSize);
            _tiles.Add(tile);

            // Assign the mesh and collider to the GameObject
            MeshFilter meshFilter = tileObject.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = tileObject.AddComponent<MeshRenderer>();

            meshFilter.mesh = tile.TileMesh;
            meshRenderer.material = new Material(sectorMaterial);

            // Assign MeshCollider for click detection
            tile.AssignCollider(tileObject);

            tile.ResolveNeighbourTiles(_tiles);
        }
    }
    private List<Face> ConstructIcosahedron()
    {
        const float tao = Mathf.PI / 2;
        const float defaultSize = 100f;

        List<Point> icosahedronCorners = new List<Point>
            {
                new Point(new Vector3(defaultSize, tao * defaultSize, 0f)),
                new Point(new Vector3(-defaultSize, tao * defaultSize, 0f)),
                new Point(new Vector3(defaultSize, -tao * defaultSize, 0f)),
                new Point(new Vector3(-defaultSize, -tao * defaultSize, 0f)),
                new Point(new Vector3(0, defaultSize, tao * defaultSize)),
                new Point(new Vector3(0, -defaultSize, tao * defaultSize)),
                new Point(new Vector3(0, defaultSize, -tao * defaultSize)),
                new Point(new Vector3(0, -defaultSize, -tao * defaultSize)),
                new Point(new Vector3(tao * defaultSize, 0f, defaultSize)),
                new Point(new Vector3(-tao * defaultSize, 0f, defaultSize)),
                new Point(new Vector3(tao * defaultSize, 0f, -defaultSize)),
                new Point(new Vector3(-tao * defaultSize, 0f, -defaultSize))
            };
        icosahedronCorners.ForEach(point => CachePoint(point));

        return new List<Face>
            {
                new Face(icosahedronCorners[0], icosahedronCorners[1], icosahedronCorners[4], false),
                new Face(icosahedronCorners[1], icosahedronCorners[9], icosahedronCorners[4], false),
                new Face(icosahedronCorners[4], icosahedronCorners[9], icosahedronCorners[5], false),
                new Face(icosahedronCorners[5], icosahedronCorners[9], icosahedronCorners[3], false),
                new Face(icosahedronCorners[2], icosahedronCorners[3], icosahedronCorners[7], false),
                new Face(icosahedronCorners[3], icosahedronCorners[2], icosahedronCorners[5], false),
                new Face(icosahedronCorners[7], icosahedronCorners[10], icosahedronCorners[2], false),
                new Face(icosahedronCorners[0], icosahedronCorners[8], icosahedronCorners[10], false),
                new Face(icosahedronCorners[0], icosahedronCorners[4], icosahedronCorners[8], false),
                new Face(icosahedronCorners[8], icosahedronCorners[2], icosahedronCorners[10], false),
                new Face(icosahedronCorners[8], icosahedronCorners[4], icosahedronCorners[5], false),
                new Face(icosahedronCorners[8], icosahedronCorners[5], icosahedronCorners[2], false),
                new Face(icosahedronCorners[1], icosahedronCorners[0], icosahedronCorners[6], false),
                new Face(icosahedronCorners[3], icosahedronCorners[9], icosahedronCorners[11], false),
                new Face(icosahedronCorners[6], icosahedronCorners[10], icosahedronCorners[7], false),
                new Face(icosahedronCorners[3], icosahedronCorners[11], icosahedronCorners[7], false),
                new Face(icosahedronCorners[11], icosahedronCorners[6], icosahedronCorners[7], false),
                new Face(icosahedronCorners[6], icosahedronCorners[0], icosahedronCorners[10], false),
                new Face(icosahedronCorners[11], icosahedronCorners[1], icosahedronCorners[6], false),
                new Face(icosahedronCorners[9], icosahedronCorners[1], icosahedronCorners[11], false)
            };
    }
    private Point CachePoint(Point point)
    {
        Point existingPoint = _points.FirstOrDefault(candidatePoint => Point.IsOverlapping(candidatePoint, point));
        if (existingPoint != null) return existingPoint;

        _points.Add(point);
        return point;
    }
    private void SubdivideIcosahedron()
    {
        _icosahedronFaces.ForEach(icoFace =>
        {
            List<Point> facePoints = icoFace.Points;
            List<Point> previousPoints;
            List<Point> bottomSide = new List<Point> { facePoints[0] };
            List<Point> leftSide = facePoints[0].Subdivide(facePoints[1], _divisions, CachePoint);
            List<Point> rightSide = facePoints[0].Subdivide(facePoints[2], _divisions, CachePoint);
            for (int i = 1; i <= _divisions; i++)
            {
                previousPoints = bottomSide;
                bottomSide = leftSide[i].Subdivide(rightSide[i], i, CachePoint);
                for (int j = 0; j < i; j++)
                {
                    //Don't need to store faces, their points will have references to them.
                    new Face(previousPoints[j], bottomSide[j], bottomSide[j + 1]);
                    if (j == 0) continue;
                    new Face(previousPoints[j - 1], previousPoints[j], bottomSide[j]);
                }
            }
        });
    }
    private MeshDetails StoreMeshDetails()
    {
        List<Point> vertices = new List<Point>();
        List<int> triangles = new List<int>();
        _tiles.ForEach(tile =>
        {
            tile.Points.ForEach(point =>
            {
                vertices.Add(point);
            });
            tile.Faces.ForEach(face =>
            {
                face.Points.ForEach(point =>
                {
                    int vertexIndex = vertices.FindIndex(vertex => vertex.ID == point.ID);
                    triangles.Add(vertexIndex);
                });
            });
        });

        return new MeshDetails(vertices.Select(point => point.Position).ToList(), triangles);
    }
}