using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
public class SystemSectors : MonoBehaviour
{
    private int hexagonRadius = 10; // Radius of each hexagon
    private int hexagonGridRadius = 6; // Radius of the hexagonal grid (number of rings)
    private Material hexagonMaterial;
    private Dictionary<Vector2Int, GameObject> hexagonDictionary = new();
    public void CreateNew(Material sectorMaterial, int size)
    {
        hexagonMaterial = sectorMaterial;
        hexagonGridRadius = size;
        GenerateSystemSectors();
        CalculateNeighbors();
    }
    public void LoadExisting(Material sectorMaterial, int size)
    {
        hexagonMaterial = sectorMaterial;
        hexagonGridRadius = size;
        GenerateSystemSectors();
        CalculateNeighbors();
    }
    private void GenerateSystemSectors()
    {
        for (int q = -hexagonGridRadius; q <= hexagonGridRadius; q++)
        {
            int r1 = Mathf.Max(-hexagonGridRadius, -q - hexagonGridRadius);
            int r2 = Mathf.Min(hexagonGridRadius, -q + hexagonGridRadius);
            for (int r = r1; r <= r2; r++)
            {
                Vector2Int axialCoords = new Vector2Int(q, r);
                CreateNewSector(AxialToWorld(axialCoords), axialCoords);
            }
        }
    }
    private void CreateNewSector(Vector3 position, Vector2Int axialCoords)
    {
        GameObject sector = new($"Sector ({axialCoords.x}, {axialCoords.y})");
        sector.layer = 10;
        sector.transform.position = position;
        sector.transform.parent = transform;

        Sector SectorComponent = sector.AddComponent<Sector>();
        MeshFilter meshFilter = sector.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = sector.AddComponent<MeshRenderer>();
        MeshCollider meshCollider = sector.AddComponent<MeshCollider>();
        sector.transform.Rotate(new Vector3(180, 0, 0));

        if (Random.Range(1, 15) <= 1)
        {
            string name = NameGenerators.PlanetNameGenerator.GenerateUniqueName();
            GameObject planet = Planet.LoadFrom(Path.Combine(Application.streamingAssetsPath, "Planets\\GenerationTemplate.json"), name);
            planet.transform.parent = sector.transform;
            planet.transform.position = (Random.Range(0, 2) <= 0) ? position + ChoseOffset() : position;
            GameManager.Planets.Add(planet);

            GameObject namePlate = Instantiate(ReferenceHolder.Instance.NamePlate, planet.transform);
            namePlate.transform.SetParent(ReferenceHolder.Instance.WorldSpaceCanvas.transform);
            namePlate.GetComponent<TextMeshProUGUI>().text = name;

            NavMeshObstacle navMeshObstacleComponent = planet.AddComponent<NavMeshObstacle>();
            navMeshObstacleComponent.shape = NavMeshObstacleShape.Capsule;
            navMeshObstacleComponent.carving = true;
            navMeshObstacleComponent.carveOnlyStationary = true;
            navMeshObstacleComponent.radius = 0.4f;
        }

        meshFilter.mesh = GenerateHexagonMesh();
        meshRenderer.material = hexagonMaterial;
        meshCollider.sharedMesh = meshFilter.mesh;

        hexagonDictionary.Add(axialCoords, sector);
    }
    private void CreateExistingSector(Vector3 position, Vector2Int axialCoords)
    {
        GameObject sector = new($"Sector ({axialCoords.x}, {axialCoords.y})");
        sector.layer = 10;
        sector.transform.position = position;
        sector.transform.parent = transform;

        MeshFilter meshFilter = sector.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = sector.AddComponent<MeshRenderer>();
        MeshCollider meshCollider = sector.AddComponent<MeshCollider>();
        sector.transform.Rotate(new Vector3(180, 0, 0));

        if (Random.Range(1, 15) <= 1)
        {
            string name = NameGenerators.PlanetNameGenerator.GenerateUniqueName();
            GameObject planet = Planet.LoadFrom(Path.Combine(Application.streamingAssetsPath, "Planets\\GenerationTemplate.json"), name);
            planet.transform.parent = sector.transform;
            planet.transform.position = (Random.Range(0, 2) <= 0) ? position + ChoseOffset() : position;
            GameManager.Planets.Add(planet);

            GameObject namePlate = Instantiate(ReferenceHolder.Instance.NamePlate, planet.transform);
            namePlate.transform.SetParent(ReferenceHolder.Instance.WorldSpaceCanvas.transform);
            namePlate.GetComponent<TextMeshProUGUI>().text = name;

            NavMeshObstacle navMeshObstacleComponent = planet.AddComponent<NavMeshObstacle>();
            navMeshObstacleComponent.shape = NavMeshObstacleShape.Capsule;
            navMeshObstacleComponent.carving = true;
            navMeshObstacleComponent.carveOnlyStationary = true;
            navMeshObstacleComponent.radius = 0.4f;
        }

        meshFilter.mesh = GenerateHexagonMesh();
        meshRenderer.material = hexagonMaterial;
        meshCollider.sharedMesh = meshFilter.mesh;

        hexagonDictionary.Add(axialCoords, sector);
    }
    private Vector3 ChoseOffset()
    {
        int offsetter = Random.Range(2, 5);
        return new(Random.Range(0, 2) <= 0 ? offsetter : -offsetter, 0, Random.Range(0, 2) <= 0 ? offsetter : -offsetter);
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
    private Vector3 AxialToWorld(Vector2Int axialCoords)
    {
        float x = hexagonRadius * 1.5f * axialCoords.x;
        float z = hexagonRadius * Mathf.Sqrt(3f) * (axialCoords.y + axialCoords.x * 0.5f);
        return new(x, 0, z);
    }
    private void CalculateNeighbors()
    {
        // These are the axial coordinate offsets for neighbors in a hexagonal grid
        Vector2Int[] neighborOffsets = new Vector2Int[]
        {
        new(1, 0),  // Right
        new(0, 1),  // Top-right
        new(-1, 1), // Top-left
        new(-1, 0), // Left
        new(0, -1), // Bottom-left
        new(1, -1)  // Bottom-right
        };
        foreach (var hexagon in hexagonDictionary)
        {
            Vector2Int gridPosition = hexagon.Key;
            GameObject hexagonObject = hexagon.Value;

            List<GameObject> neighbors = new();
            foreach (var offset in neighborOffsets)
            {
                Vector2Int neighborPosition = gridPosition + offset;
                if (hexagonDictionary.TryGetValue(neighborPosition, out GameObject neighbor)) neighbors.Add(neighbor);
            }
        }
    }
}