using Assets;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
public class GameManager : MonoBehaviour
{
    [Header("Planet Settings")]
    public GameObject planet; // The planet GameObject
    public Vector3 starPosition = new Vector3(-300, 0, 0); // The point in 3D space representing the star
    public float planetRotationSpeed = 0.0025f; // Speed of planet's self-rotation
    public float planetOrbitSpeed = 0.05f; // Speed of planet's orbit around the star
    public Vector3 planetTiltAxis = new Vector3(0.5f, 1, 0); // Tilt axis of the planet

    [Header("Moon Settings")]
    public GameObject moon; // The moon GameObject
    public float moonOrbitSpeed = 2f; // Speed of moon's orbit around the planet

    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<GameManager>();
                    singletonObject.name = typeof(GameManager).ToString() + " (Singleton)";
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this) Destroy(gameObject);
    }
    private void Start()
    {
        MaterialsHolder materialsHolder = FindObjectOfType<MaterialsHolder>();
        if (materialsHolder == null)
        {
            Debug.LogError("MaterialsHolder not found in the scene!");
            return;
        }

        Random.InitState(10);

        int index = 0;
        GameObject[] planets = new GameObject[2];
        for (int asd = 0; asd < 2; asd++)
        {
            for (int xy = 0; xy < 1; xy++)
            {
                planets[index] = new GameObject($"Planet {xy}");
                Planet planetComponent = planets[index].AddComponent<Planet>();
                TrailRenderer trailRenderer = planets[index].AddComponent<TrailRenderer>();
                SphereCollider colliderComponent = planets[index].AddComponent<SphereCollider>();

                planets[index].transform.position = new Vector3(0, 0, 0);

                index++;
                planetComponent.resolution = 256;
                planetComponent.shapeSettings = Instantiate(new ShapeSettings());
                planetComponent.colorSettings = Instantiate(new ColorSettings());

                planetComponent.colorSettings = ScriptableObject.CreateInstance<ColorSettings>();
                planetComponent.shapeSettings = ScriptableObject.CreateInstance<ShapeSettings>();

                string savedData = System.IO.File.ReadAllText("C:\\Asztali gép\\test/test.json");
                JObject jsonObject = JObject.Parse(savedData);
                jsonObject["colorSettings"]["planetMaterial"] = null;

                planetComponent.shapeSettings = JsonConvert.DeserializeObject<ShapeSettings>(jsonObject["shapeSettings"]?.ToString());
                planetComponent.colorSettings = JsonConvert.DeserializeObject<ColorSettings>(jsonObject["colorSettings"]?.ToString());

                planetComponent.colorSettings.planetMaterial = new Material(materialsHolder.planetMaterial);

                colliderComponent.radius = planetComponent.shapeSettings.planetRadius;

                JObject oceanColorData = (JObject)jsonObject["colorSettings"]["oceanColor"];

                var oceanGradient = new Gradient();
                var oceanColors = new GradientColorKey[oceanColorData["m_NumColorKeys"].Value<int>()];
                var oceanAlphas = new GradientAlphaKey[oceanColorData["m_NumColorKeys"].Value<int>()];

                for (int i = 0; i < oceanColorData["m_NumColorKeys"].Value<int>(); i++)
                {
                    Color oceanColorBit = new(
                        oceanColorData[$"key{i}"]["r"].Value<float>() + Random.Range(-0.3f, 0.3f),
                        oceanColorData[$"key{i}"]["g"].Value<float>() + Random.Range(-0.3f, 0.3f),
                        oceanColorData[$"key{i}"]["b"].Value<float>() + Random.Range(-0.3f, 0.3f),
                        oceanColorData[$"key{i}"]["a"].Value<float>());
                    oceanColors[i] = new GradientColorKey(oceanColorBit, oceanColorData[$"ctime{i}"].Value<int>() / 65535f);
                    oceanAlphas[i] = new GradientAlphaKey(1.0f, 1.0f);
                }
                oceanGradient.SetKeys(oceanColors, oceanAlphas);
                planetComponent.colorSettings.oceanColor = oceanGradient;

                JObject landColorData = (JObject)jsonObject["colorSettings"]["biomeColorSettings"];
                for (int i = 0; i < landColorData["biomes"].Count(); i++)
                {
                    var biomeColorGradient = new Gradient();
                    JObject biome = (JObject)landColorData["biomes"][i]["gradient"];
                    var biomeColors = new GradientColorKey[biome["m_NumColorKeys"].Value<int>()];
                    var biomeAlphas = new GradientAlphaKey[biome["m_NumColorKeys"].Value<int>()];
                    for (int j = 0; j < biome["m_NumColorKeys"].Value<int>(); j++)
                    {
                        Color biomeColorBit = new(
                            biome[$"key{j}"]["r"].Value<float>() + Random.Range(-0.5f, 0.5f),
                            biome[$"key{j}"]["g"].Value<float>() + Random.Range(-0.5f, 0.5f),
                            biome[$"key{j}"]["b"].Value<float>() + Random.Range(-0.5f, 0.5f),
                            biome[$"key{j}"]["a"].Value<float>());
                        biomeColors[j] = new GradientColorKey(biomeColorBit, biome[$"ctime{j}"].Value<int>() / 65535f);
                        biomeAlphas[j] = new GradientAlphaKey(1.0f, 1.0f);
                    }
                    biomeColorGradient.SetKeys(biomeColors, biomeAlphas);
                    planetComponent.colorSettings.biomeColorSettings.biomes[i].gradient = biomeColorGradient;
                }

                trailRenderer.time = 60;
                trailRenderer.startWidth = 0.025f;
                trailRenderer.endWidth = 0.05f;
                trailRenderer.enabled = true;

                planetComponent.GeneratePlanet();
            }
        }

        planets[0].GetComponent<TrailRenderer>().material.color = Color.red;
        planets[1].GetComponent<TrailRenderer>().material.color = Color.blue;

        int index2 = 0;
        for (int asd = 0; asd < 2; asd++)
        {
            for (int xy = 0; xy < 1; xy++)
            {
                DrawHexasphere sectors = planets[index2].AddComponent<DrawHexasphere>();
                planets[index2].transform.position = new Vector3(0, 0, 0);
                sectors.radius = 11;
                sectors.sectorMaterial = materialsHolder.sectorMaterial;
                sectors.divisions = 3;
                index2++;
            }
        }
        this.planet = planets[0];
        moon = planets[1];
    }
    private void Update()
    {
        // Time multiplier to control the speed of time
        float timeMultiplier = 1.0f; // Change this value to adjust the speed of orbits

        // Orbital speeds in degrees per second, based on real-life data:
        // Earth's orbital speed around the sun: ~0.9856 degrees per day (~360 degrees/year)
        // Moon's orbital speed around Earth: ~13.1764 degrees per day (~360 degrees/month)

        float realEarthOrbitSpeed = 0.9856f;  // Degrees per second
        float realMoonOrbitSpeed = 13.1764f; // Degrees per second

        // Apply the time multiplier
        float adjustedPlanetOrbitSpeed = realEarthOrbitSpeed * timeMultiplier;
        float adjustedMoonOrbitSpeed = realMoonOrbitSpeed * 24 * timeMultiplier;

        // Update the rotations
        moon.transform.RotateAround(planet.transform.position, Vector3.up, adjustedMoonOrbitSpeed * Time.deltaTime);
        planet.transform.RotateAround(new Vector3(-300, 0, 0), Vector3.up, adjustedPlanetOrbitSpeed * Time.deltaTime);

        // Add deviation to the planet's orbit for more realistic orbits
        planet.transform.RotateAround(moon.transform.position, Vector3.up, Time.deltaTime * timeMultiplier);
    }
}