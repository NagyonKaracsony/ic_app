using UnityEngine;
[System.Serializable]
public class SimpleNoiseFilter : INoiseFilter
{
    NoiseSettings.SimpleNoiseSettings settings;
    Noise noise = new Noise();
    public SimpleNoiseFilter(NoiseSettings.SimpleNoiseSettings settings)
    {
        this.settings = settings;
    }
    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0;
        float frequency = settings.baseRoughness;
        float amplitude = 1;
        for (int i = 0; i < settings.numLayers; i++)
        {
            float v = noise.Evaluate(point * frequency + new Vector3(settings.center[0], settings.center[1], settings.center[2]));
            noiseValue += (v + 1) * .5f * amplitude;
            frequency *= settings.roughness;
            amplitude *= settings.persistence;
        }

        noiseValue = noiseValue - settings.minValue;
        return noiseValue * settings.strength;
    }
}