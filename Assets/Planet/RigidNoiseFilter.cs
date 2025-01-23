using UnityEngine;
public class RigidNoiseFilter : INoiseFilter
{
    NoiseSettings.RigidNoiseSettigns settings;
    Noise noise = new Noise();
    public RigidNoiseFilter(NoiseSettings.RigidNoiseSettigns settings)
    {
        this.settings = settings;
    }
    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0;
        float frequency = settings.baseRoughness;
        float amplitude = 1;
        float weight = 1;
        for (int i = 0; i < settings.numLayers; i++)
        {
            float v = 1 - Mathf.Abs(noise.Evaluate(point * frequency + new Vector3(settings.center[0], settings.center[1], settings.center[2])));
            v *= v;
            v *= weight;
            weight = Mathf.Clamp01(v * settings.weightMultiplier);

            noiseValue += v * amplitude;
            frequency *= settings.roughness;
            amplitude *= settings.persistence;
        }

        noiseValue = noiseValue - settings.minValue;
        return noiseValue * settings.strength;
    }
}