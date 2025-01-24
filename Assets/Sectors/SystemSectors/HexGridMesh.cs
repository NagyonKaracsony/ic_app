using UnityEngine;
public class HexagonGridGenerator : MonoBehaviour
{
    [SerializeField] private float hexagonRadius = 1f; // Radius of each hexagon
    [SerializeField] private int gridWidth = 20; // Number of hexagons in width
    [SerializeField] private int gridHeight = 20; // Number of hexagons in height
    [SerializeField] private Material hexagonMaterial;
    private void Start()
    {
        GenerateHexagonGrid();
    }
    private void GenerateHexagonGrid()
    {
        float hexWidth = hexagonRadius * 2f;
        float hexHeight = Mathf.Sqrt(3f) * hexagonRadius;

        float horizontalSpacing = hexWidth * 0.75f; // Horizontal distance between hexagon centers
        float verticalSpacing = hexHeight; // Vertical distance between hexagon centers

        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                float offsetX = (y % 2 == 0) ? 0 : verticalSpacing / 2f;
                Vector3 position = new Vector3(x * verticalSpacing + offsetX, 0, y * horizontalSpacing);
                CreateHexagon(position);
            }
        }
    }

    private void CreateHexagon(Vector3 position)
    {
        GameObject hexagon = new GameObject($"Hexagon ({position.x}-{position.z})");
        hexagon.transform.position = position;
        hexagon.transform.parent = transform;

        MeshFilter meshFilter = hexagon.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = hexagon.AddComponent<MeshRenderer>();
        MeshCollider meshCollider = hexagon.AddComponent<MeshCollider>();
        hexagon.transform.Rotate(new Vector3(180, 90, 0));
        meshFilter.mesh = GenerateHexagonMesh();
        meshRenderer.material = hexagonMaterial;
        meshCollider.sharedMesh = meshFilter.mesh;
    }

    private Mesh GenerateHexagonMesh()
    {
        Mesh mesh = new Mesh();

        float angleStep = 60f;
        Vector3[] vertices = new Vector3[7]; // 6 corners + center
        int[] triangles = new int[18]; // 6 triangles, 3 vertices each

        vertices[0] = Vector3.zero; // Center

        // Create vertices
        for (int i = 0; i < 6; i++)
        {
            float angle = Mathf.Deg2Rad * (i * angleStep);
            vertices[i + 1] = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * hexagonRadius;
        }

        // Create triangles
        for (int i = 0; i < 6; i++)
        {
            int startIndex = i * 3;
            triangles[startIndex] = 0;
            triangles[startIndex + 1] = i + 1;
            triangles[startIndex + 2] = i == 5 ? 1 : i + 2;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }
}
