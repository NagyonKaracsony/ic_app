using UnityEngine;
[System.Serializable]
public class PlanetData
{
    public int resolution;
    public string shapeSettings;
    public string colorSettings;
    public PlanetData(Planet planet)
    {
        Material planetMaterial = new Material(planet.colorSettings.planetMaterial);
        resolution = planet.resolution;
        shapeSettings = JsonUtility.ToJson(planet.shapeSettings, true);
        ColorSettings deMaterializedColorSettings = planet.colorSettings;
        deMaterializedColorSettings.planetMaterial = null;
        colorSettings = JsonUtility.ToJson(deMaterializedColorSettings, true);
        planet.colorSettings.planetMaterial = planetMaterial;
    }
}