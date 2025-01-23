using UnityEngine;
[System.Serializable]
public class NoiseSettings
{
    public enum FilterType { Simple, Rigid }
    public FilterType filterType;

    [ConditionalHide("filterType", 0)]
    public SimpleNoiseSettings simpleNoiseSettings;
    [ConditionalHide("filterType", 1)]
    public RigidNoiseSettigns rigidNoiseSettings;

    [System.Serializable]
    public class SimpleNoiseSettings
    {
        public float strength = 1;
        [Range(1, 8)]
        public int numLayers = 5;
        public float baseRoughness = 1;
        public float roughness = 2;
        public float persistence = .5f;
        public float[] center = new float[3];
        public float minValue;
    }
    [System.Serializable]
    public class RigidNoiseSettigns : SimpleNoiseSettings
    {
        public float weightMultiplier = .8f;
    }
}