using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Assets;
using System.Linq;
public class Planet : MonoBehaviour
{
    [Range(2, 256)]
    public int resolution = 16;
    public bool autoUpdate = true;
    public enum FaceRenderMask { All, Top, Bottom, Left, Right, Front, Back };
    public FaceRenderMask faceRenderMask;

    public ShapeSettings shapeSettings;
    public ColorSettings colorSettings;

    public string saveTemplateAs;

    [HideInInspector]
    public bool shapeSettingsFoldout;
    [HideInInspector]
    public bool colorSettingsFoldout;

    ShapeGenerator shapeGenerator = new();
    ColorGenerator colorGenerator = new();

    [HideInInspector]
    MeshFilter[] meshFilters;
    TerrainFace[] terrainFaces;

    public List<GameObject> sectorsData;
    void Initialize()
    {
        shapeGenerator.UpdateSettings(shapeSettings);
        colorGenerator.UpdateSettings(colorSettings);
        if (meshFilters == null || meshFilters.Length == 0) meshFilters = new MeshFilter[6];
        terrainFaces = new TerrainFace[6];
        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };
        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i] == null)
            {
                GameObject meshObj = new GameObject("mesh");
                meshObj.transform.parent = transform;
                meshObj.AddComponent<MeshRenderer>();
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }
            meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = colorSettings.planetMaterial;
            terrainFaces[i] = new TerrainFace(shapeGenerator, meshFilters[i].sharedMesh, resolution, directions[i]);
            bool renderFace = faceRenderMask == FaceRenderMask.All || (int)faceRenderMask - 1 == i;
            meshFilters[i].gameObject.SetActive(renderFace);
        }
    }
    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
        GenerateColours();
    }
    public void OnShapeSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateMesh();
        }
    }
    public void OnColorSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateColours();
        }
    }
    void GenerateMesh()
    {
        for (int i = 0; i < 6; i++) if (meshFilters[i].gameObject.activeSelf) terrainFaces[i].ConstructMesh();
        colorGenerator.UpdateElevation(shapeGenerator.elevationMinMax);
    }
    void GenerateColours()
    {
        colorGenerator.UpdateColors();
        for (int i = 0; i < 6; i++) if (meshFilters[i].gameObject.activeSelf) terrainFaces[i].UpdateUVs(colorGenerator);
    }
    public void SaveTo(string filePath)
    {
        System.IO.File.WriteAllText(filePath, "");
        PlanetData planetData = new(this);
        JObject json = JObject.Parse(JsonUtility.ToJson(planetData, true));
        List<string> errorList = new();
        foreach (var property in json.Properties())
        {
            if (property.Value.Type == JTokenType.String && property.Value.ToString().Trim().StartsWith("{"))
            {
                try
                {
                    JObject nestedObject = JObject.Parse(property.Value.ToString());
                    property.Value = nestedObject;
                }
                catch (JsonReaderException error) { errorList.Add($"{error.StackTrace}\n{error.Message}"); }
            }
        }
        json.Add("saveErrors", JArray.FromObject(errorList));

        System.IO.File.WriteAllText(filePath, json.ToString(Formatting.None));
    }

    public static GameObject LoadFrom(string name, MaterialsHolder materialsHolder, string settingsTarget)
    {
        GameObject planet = new GameObject(name);

        Planet planetComponent = planet.AddComponent<Planet>();
        SphereCollider colliderComponent = planet.AddComponent<SphereCollider>();

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
        colliderComponent.radius = planetComponent.shapeSettings.planetRadius;

        planetComponent.GeneratePlanet();
        return planet;
    }
    public void LogData()
    {
        string planet = JsonUtility.ToJson(this, true);
        string shapeSettings = JsonUtility.ToJson(this.shapeSettings, true);
        string colorSettings = JsonUtility.ToJson(this.colorSettings, true);
    }
}