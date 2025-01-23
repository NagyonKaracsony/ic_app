using UnityEngine;
[CreateAssetMenu()]
[System.Serializable]
public class ShapeSettings : ScriptableObject {
    public float planetRadius = 1;
    public NoiseLayer[] noiseLayers;
    [System.Serializable]
    public class NoiseLayer
    {
        public bool enabled = true;
        public bool useFirstLayerAsMask;
        public NoiseSettings noiseSettings;
    }
}