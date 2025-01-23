using UnityEngine;
public class TerrainFace
{
    ShapeGenerator shapeGenerator;
    Mesh mesh;
    int resolution;
    Vector3 localUp;
    Vector3 axisA;
    Vector3 axisB;
    public TerrainFace(ShapeGenerator shapeGenerator, Mesh mesh, int resolution, Vector3 localUp)
    {
        this.shapeGenerator = shapeGenerator;
        this.mesh = mesh;
        this.resolution = resolution;
        this.localUp = localUp;

        axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        axisB = Vector3.Cross(localUp, axisA);
    }
    public void ConstructMesh()
    {
        // If the mesh vertex count matches, reuse the arrays, otherwise create new ones
        Vector3[] vertices = (mesh.vertexCount == resolution * resolution) ? mesh.vertices : new Vector3[resolution * resolution];
        int[] triangles = (mesh.triangles.Length == (resolution - 1) * (resolution - 1) * 6) ? mesh.triangles : new int[(resolution - 1) * (resolution - 1) * 6];
        Vector2[] uv = (mesh.uv.Length == vertices.Length) ? mesh.uv : new Vector2[vertices.Length];

        int triIndex = 0;
        float inverseResolution = 1f / (resolution - 1); // Precompute the inverse to avoid repeated division

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int i = x + y * resolution;
                float percentX = x * inverseResolution;  // Replace division with multiplication by the inverse
                float percentY = y * inverseResolution;

                // Combine unit cube point calculation into one step
                Vector3 pointOnUnitCube = localUp + (percentX - 0.5f) * 2 * axisA + (percentY - 0.5f) * 2 * axisB;
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;

                // Elevation calculations (assumed to be expensive)
                float unscaledElevation = shapeGenerator.CalculateUnscaledElevation(pointOnUnitSphere);
                vertices[i] = pointOnUnitSphere * shapeGenerator.GetScaledElevation(unscaledElevation);
                uv[i].y = unscaledElevation;

                // Only generate triangles where appropriate
                if (x != resolution - 1 && y != resolution - 1)
                {
                    int a = i;
                    int b = i + resolution + 1;
                    int c = i + resolution;
                    int d = i + 1;

                    // Write the triangles in order, reduces index calculations
                    triangles[triIndex] = a;
                    triangles[triIndex + 1] = b;
                    triangles[triIndex + 2] = c;

                    triangles[triIndex + 3] = a;
                    triangles[triIndex + 4] = d;
                    triangles[triIndex + 5] = b;
                    triIndex += 6;
                }
            }
        }

        // Clear the mesh but reuse the arrays
        mesh.Clear();

        // Apply vertices and triangles to the mesh, defer normal recalculation to minimize updates
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;

        // Only recalculate normals once after all operations
        mesh.RecalculateNormals();
    }
    public void UpdateUVs(ColorGenerator colorGenerator)
    {
        // Reuse the UV array if its length matches
        Vector2[] uv = mesh.uv.Length == resolution * resolution ? mesh.uv : new Vector2[resolution * resolution];

        float inverseResolution = 1f / (resolution - 1f); // Precompute inverse for performance

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int i = x + y * resolution;

                // Calculate percentage without repeated division
                float percentX = x * inverseResolution;
                float percentY = y * inverseResolution;

                // Combine unit cube point calculation
                Vector3 pointOnUnitCube = localUp + (percentX - 0.5f) * 2 * axisA + (percentY - 0.5f) * 2 * axisB;
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;

                // Assign UV based on biome percent from point
                uv[i].x = colorGenerator.BiomePercentFromPoint(pointOnUnitSphere);
            }
        }
        mesh.uv = uv;
    }
}