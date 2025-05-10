using UnityEngine;
[System.Serializable]
public class PlanetData
{
    public int resolution;
    public string shapeSettings;
    public string colorSettings;
    public string planetProperties;
    public PlanetData(Planet planet)
    {
        Material planetMaterial = new(planet.colorSettings.planetMaterial);
        resolution = planet.resolution;
        shapeSettings = JsonUtility.ToJson(planet.shapeSettings, false);
        ColorSettings deMaterializedColorSettings = planet.colorSettings;
        deMaterializedColorSettings.planetMaterial = null;
        colorSettings = JsonUtility.ToJson(deMaterializedColorSettings, false);
        planet.colorSettings.planetMaterial = planetMaterial;
        Debug.Log(planet.planetProperties);
        Debug.Log(JsonUtility.ToJson(planet.planetProperties, false));
        planetProperties = JsonUtility.ToJson(planet.planetProperties, false);
    }
}