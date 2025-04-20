using Assets;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
public class Planet : MonoBehaviour
{
    [Range(2, 256)]
    public int resolution = 16;
    public bool autoUpdate = true;
    public enum FaceRenderMask { All, Top, Bottom, Left, Right, Front, Back };
    public FaceRenderMask faceRenderMask;

    public ShapeSettings shapeSettings;
    public ColorSettings colorSettings;
    public PlanetProperties planetProperties;

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
        GenerateColors();
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
            GenerateColors();
        }
    }
    void GenerateMesh()
    {
        for (int i = 0; i < 6; i++) if (meshFilters[i].gameObject.activeSelf) terrainFaces[i].ConstructMesh();
        colorGenerator.UpdateElevation(shapeGenerator.elevationMinMax);
    }
    void GenerateColors()
    {
        colorGenerator.UpdateColors();
        for (int i = 0; i < 6; i++) if (meshFilters[i].gameObject.activeSelf) terrainFaces[i].UpdateUVs(colorGenerator);
    }
    public void SaveTo(string filePath)
    {
        PlanetData planetData = new(this);
        JObject json = JObject.Parse(JsonUtility.ToJson(planetData, true));
        foreach (var property in json.Properties())
        {
            if (property.Value.Type == JTokenType.String && property.Value.ToString().Trim().StartsWith("{"))
            {
                JObject nestedObject = JObject.Parse(property.Value.ToString());
                property.Value = nestedObject;
            }
        }
        System.IO.File.WriteAllText(filePath, "");
        System.IO.File.WriteAllText(filePath, json.ToString(Formatting.None));
    }
    public static GameObject LoadFrom(string filePath, string name)
    {
        GameObject planet = new(name);
        Planet planetComponent = planet.AddComponent<Planet>();
        SphereCollider colliderComponent = planet.AddComponent<SphereCollider>();

        planetComponent.resolution = 64;
        planetComponent.shapeSettings = ScriptableObject.CreateInstance<ShapeSettings>();
        planetComponent.colorSettings = ScriptableObject.CreateInstance<ColorSettings>();

        JObject jsonObject = JObject.Parse(System.IO.File.ReadAllText(filePath));
        jsonObject["colorSettings"]["planetMaterial"] = null;

        planetComponent.shapeSettings = JsonConvert.DeserializeObject<ShapeSettings>(jsonObject["shapeSettings"].ToString());
        planetComponent.colorSettings = JsonConvert.DeserializeObject<ColorSettings>(jsonObject["colorSettings"].ToString());
        planetComponent.colorSettings.planetMaterial = new Material(ReferenceHolder.Instance.planetMaterial);

        planetComponent.colorSettings.oceanColor = DeserializeGradient((JObject)jsonObject["colorSettings"]["oceanColor"], 0.3f);
        var landColorData = (JObject)jsonObject["colorSettings"]["biomeColorSettings"];
        for (int i = 0; i < landColorData["biomes"].Count(); i++) planetComponent.colorSettings.biomeColorSettings.biomes[i].gradient = DeserializeGradient((JObject)landColorData["biomes"][i]["gradient"], 0.5f);

        colliderComponent.radius = planetComponent.shapeSettings.planetRadius;

        // planetComponent.planetProperties.

        GameObject atmosphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        atmosphere.transform.parent = planet.transform;
        atmosphere.transform.position = new Vector3(0, 0, 0);
        atmosphere.GetComponent<MeshRenderer>().material = new(ReferenceHolder.Instance.atmosphereMaterial);

        atmosphere.transform.localScale = new Vector3(2.05f, 2.05f, 2.05f);

        planetComponent.GeneratePlanet();
        return planet;
    }
    private static Gradient DeserializeGradient(JObject gradientData, float randomRange)
    {
        int keyCount = gradientData["m_NumColorKeys"].Value<int>();
        var colors = new GradientColorKey[keyCount];
        var alphas = new GradientAlphaKey[keyCount];

        for (int i = 0; i < keyCount; i++)
        {
            colors[i] = new GradientColorKey(
                new Color(
                    gradientData[$"key{i}"]["r"].Value<float>() + Random.Range(-randomRange, randomRange),
                    gradientData[$"key{i}"]["g"].Value<float>() + Random.Range(-randomRange, randomRange),
                    gradientData[$"key{i}"]["b"].Value<float>() + Random.Range(-randomRange, randomRange),
                    gradientData[$"key{i}"]["a"].Value<float>()
                ),
                gradientData[$"ctime{i}"].Value<int>() / 65535f
            );
            alphas[i] = new GradientAlphaKey(1.0f, 1.0f);
        }
        var gradient = new Gradient();
        gradient.SetKeys(colors, alphas);
        return gradient;
    }
}