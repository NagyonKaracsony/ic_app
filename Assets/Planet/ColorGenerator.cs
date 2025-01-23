using UnityEngine;
public class ColorGenerator
{
    ColorSettings settings;
    Texture2D texture;
    const int textureResolution = 50;
    INoiseFilter biomeNoiseFilter;
    public void UpdateSettings(ColorSettings settings)
    {
        this.settings = settings;
        if (texture == null || texture.height != settings.biomeColorSettings.biomes.Length)
        {
            texture = new Texture2D(textureResolution * 2, settings.biomeColorSettings.biomes.Length, TextureFormat.RGBA32, false);
        }
        biomeNoiseFilter = NoiseFilterFactory.CreateNoiseFilter(settings.biomeColorSettings.noise);
    }
    public void UpdateElevation(MinMax elevationMinMax)
    {
        settings.planetMaterial.SetVector("_elevationMinMax", new Vector4(elevationMinMax.Min, elevationMinMax.Max));
    }
    public float BiomePercentFromPoint(Vector3 pointOnUnitSphere)
    {
        float heightPercent = (pointOnUnitSphere.y + 1) / 2f;
        heightPercent += (biomeNoiseFilter.Evaluate(pointOnUnitSphere) - settings.biomeColorSettings.noiseOffset) * settings.biomeColorSettings.noiseStrenght;
        float biomeIndex = 0;
        int numBiomes = settings.biomeColorSettings.biomes.Length;
        float blendRange = settings.biomeColorSettings.blendAmount / 2f + 0.001f;
        for (int i = 0; i < numBiomes; i++)
        {
            float distance = heightPercent - settings.biomeColorSettings.biomes[i].startHeight;
            float weight = Mathf.InverseLerp(-blendRange, blendRange, distance);
            biomeIndex *= (1 - weight);
            biomeIndex += i * weight;
        }
        return biomeIndex / Mathf.Max(1, numBiomes - 1);
    }
    public void UpdateColors()
    {
        // Ensure the colors array is initialized before checking its length
        Color[] colors = new Color[texture.width * texture.height];

        int colorIndex = 0;
        float inverseTextureResolution = 1f / (textureResolution - 1f); // Precompute inverse for performance

        foreach (var biome in settings.biomeColorSettings.biomes)
        {
            for (int i = 0; i < textureResolution * 2; i++)
            {
                // Use the precomputed inverse instead of division
                float percent = (i < textureResolution) ? (i * inverseTextureResolution) : ((i - textureResolution) * inverseTextureResolution);

                // Use ocean or biome gradient based on index
                Color gradientColor = (i < textureResolution) ? settings.oceanColor.Evaluate(percent) : biome.gradient.Evaluate(percent);

                // Apply tint based on the biome's tint percent
                Color tintColor = biome.tint;
                colors[colorIndex] = gradientColor * (1f - biome.tintPercent) + tintColor * biome.tintPercent;
                colorIndex++;
            }
        }

        // Apply colors to the texture and update the material
        texture.SetPixels(colors);
        texture.Apply();
        settings.planetMaterial.SetTexture("_texture", texture);
    }
}