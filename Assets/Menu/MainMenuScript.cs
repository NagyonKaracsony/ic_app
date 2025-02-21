using Assets;
using UnityEngine;
public class MainMenuScript : MonoBehaviour
{
    private readonly GameObject[] planets = new GameObject[3];
    private readonly Vector3[] rotationAxis = new Vector3[3];
    private readonly float[] rotationSpeeds = new float[3];

    public GameObject StarPrefab;
    private GameObject Star;

    private readonly float StarOrbitRadius = 200;
    private readonly float StarOrbitSpeed = 5;
    private readonly float StarTiltAngle = 10;
    private float StarOrbitAngle;
    private Vector3 StarTiltAxis;
    private struct PlanetData
    {
        public Vector3 position;
        public float radius;
        public int resolution;
    }
    void Start()
    {
        MaterialsHolder materialsHolder = FindObjectOfType<MaterialsHolder>();
        Star = Instantiate(StarPrefab);

        for (int i = 0; i < Star.transform.childCount; i++) Star.transform.GetChild(i).localScale = new Vector3(10, 10, 10);

        Light pointLight = Star.transform.GetChild(Star.transform.childCount - 1).GetComponent<Light>();
        pointLight.intensity = 2f;
        pointLight.range = 400;

        for (int i = 0; i < planets.Length; i++)
        {
            planets[i] = Planet.LoadFrom("Assets\\Templates\\Planets\\GenerationTemplate.json", $"planet", materialsHolder);
            rotationAxis[i] = Random.onUnitSphere;
            rotationSpeeds[i] = Random.Range(2.5f, 5) * (Random.value > 0.5f ? 1f : -1f);
        }

        PlanetData[] planetData = new PlanetData[] {
            new PlanetData { position = new Vector3(40, 0, 0), radius = 25, resolution = 256 },
            new PlanetData { position = new Vector3(-20, -25, -45), radius = 10, resolution = 256 },
            new PlanetData { position = new Vector3(-75, 40, 400), radius = 10, resolution = 64 }
        };

        for (int i = 0; i < planets.Length && i < planetData.Length; i++)
        {
            Planet planet = planets[i].GetComponent<Planet>();
            planets[i].transform.position = planetData[i].position;
            planet.shapeSettings.planetRadius = planetData[i].radius;
            planet.resolution = planetData[i].resolution;
        }

        for (int i = 0; i < planets.Length; i++) planets[i].GetComponent<Planet>().GeneratePlanet();
        StarTiltAxis = Quaternion.Euler(Random.Range(-StarTiltAngle, StarTiltAngle), 0, Random.Range(-StarTiltAngle, StarTiltAngle)) * Vector3.forward;
        UpdateStarPosition();
    }
    void Update()
    {
        for (int i = 0; i < planets.Length; i++)
        {
            planets[i].transform.Rotate(rotationAxis[i], rotationSpeeds[i] * Time.deltaTime, Space.Self);
            planets[i].transform.Rotate(2.5f * Time.deltaTime * Vector3.up);
        }
        UpdateStarPosition();
    }
    void UpdateStarPosition()
    {
        StarOrbitAngle += StarOrbitSpeed * Time.deltaTime;
        if (StarOrbitAngle >= 360f) StarOrbitAngle -= 360f;

        float x = Mathf.Cos(StarOrbitAngle * Mathf.Deg2Rad) * StarOrbitRadius;
        float z = Mathf.Sin(StarOrbitAngle * Mathf.Deg2Rad) * StarOrbitRadius;

        Vector3 orbitOffset = new(x, 0, z);
        orbitOffset += StarTiltAxis * Mathf.Sin(StarOrbitAngle * Mathf.Deg2Rad) * StarOrbitRadius * 0.1f;
        Star.transform.position = orbitOffset;
    }
}