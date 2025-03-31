using Assets;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    private readonly GameObject[] planets = new GameObject[3]; // planet game objects
    private readonly Vector3[] rotationAxis = new Vector3[3]; // planet rotation axis
    private readonly float[] rotationSpeeds = new float[3]; // planet rotation speeds for their own axis
    private readonly float[] orbitSpeeds = new float[3]; // planet orbit speeds
    private readonly float[] orbitRadii = new float[3]; // planet orbit radiuses
    private float[] orbitAngles = new float[3]; // current angle of the planet in its orbit
    private readonly float[] yOffsets = new float[3]; // vertical offsets of the planets
    private readonly float[] eccentricities = new float[3]; // Eccentricity factors for the planet orbits 
    private readonly float[] orbitPhases = new float[3]; // Phase offsets for the planet orbits

    public GameObject StarPrefab; // star prefab
    private GameObject Star; // star game object

    private struct PlanetData
    {
        public float orbitRadius;
        public float orbitSpeed;
        public float radius;
        public int resolution;
    }

    void Start()
    {
        MaterialsHolder materialsHolder = FindObjectOfType<MaterialsHolder>();

        // create the star game object its light source
        Star = Instantiate(StarPrefab);
        Star.transform.position = new Vector3(0, 0, 0);
        for (int i = 0; i < Star.transform.childCount; i++) Star.transform.GetChild(i).localScale = new Vector3(1, 1, 1);
        Light pointLight = Star.transform.GetChild(Star.transform.childCount - 1).GetComponent<Light>();
        pointLight.intensity = 1f;
        pointLight.range = 500;

        // create the planet game objects 
        for (int i = 0; i < planets.Length; i++) planets[i] = Planet.LoadFrom("Assets\\Templates\\Planets\\GenerationTemplate.json", $"planet", materialsHolder);

        // set up the planets with absolute properties
        PlanetData[] planetData = new PlanetData[] {
            new PlanetData { orbitRadius = 10, orbitSpeed = 20, radius = 1, resolution = 256 },
            new PlanetData { orbitRadius = 16, orbitSpeed = 20, radius = 1, resolution = 256 },
            new PlanetData { orbitRadius = 22, orbitSpeed = 20, radius = 1, resolution = 64 }
        };

        // set up the planets with randomized properties
        for (int i = 0; i < planets.Length && i < planetData.Length; i++)
        {
            orbitSpeeds[i] = planetData[i].orbitSpeed;
            orbitRadii[i] = planetData[i].orbitRadius;
            rotationSpeeds[i] = Random.Range(2.5f, 5) * (Random.value > 0.5f ? 1f : -1f);
            rotationAxis[i] = Random.onUnitSphere;

            Planet planet = planets[i].GetComponent<Planet>();
            planet.shapeSettings.planetRadius = planetData[i].radius;
            planet.resolution = planetData[i].resolution;

            orbitAngles[i] = Random.Range(0f, 360f); // Random starting angle (phase) of the orbit
            yOffsets[i] = Random.Range(-5f, 5f); // Small vertical offset
            eccentricities[i] = Random.Range(0.9f, 1.1f); // Slightly elliptical orbit
            orbitPhases[i] = Random.Range(0f, 360f); // Random phase offset
            planet.GeneratePlanet();
        }
    }

    void Update()
    {
        // for each planet
        for (int i = 0; i < planets.Length; i++)
        {
            // rotate planet around their own axis
            planets[i].transform.Rotate(rotationAxis[i], rotationSpeeds[i] * Time.deltaTime, Space.Self);
            planets[i].transform.Rotate(2.5f * Time.deltaTime * Vector3.up);

            // rotate the camera and canvas around a special axis focusing on the star
            Camera.main.transform.RotateAround(new Vector3(0, 0, 0), Vector3.forward, 0.5f * Time.deltaTime);
            GameObject.Find("Canvas").transform.RotateAround(new Vector3(0, 0, 0), Vector3.forward, 0.5f * Time.deltaTime);

            // handle panet orbit around the star
            orbitAngles[i] += orbitSpeeds[i] * Time.deltaTime;
            float x = Mathf.Cos((orbitAngles[i] + orbitPhases[i]) * Mathf.Deg2Rad) * orbitRadii[i] * eccentricities[i];
            float z = Mathf.Sin((orbitAngles[i] + orbitPhases[i]) * Mathf.Deg2Rad) * orbitRadii[i];
            float y = yOffsets[i];

            // update planet position
            planets[i].transform.position = new Vector3(x, y, z);
        }
    }
}