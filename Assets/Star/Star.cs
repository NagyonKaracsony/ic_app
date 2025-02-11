using UnityEngine;
namespace Assets
{
    public class Star : MonoBehaviour
    {
        public Material starMaterial;
        public StarSettings starSettings;
        public void Start()
        {
            GameObject lightGameObject = new GameObject("StarLight");
            Light lightComponent = lightGameObject.AddComponent<Light>();
            lightComponent.type = LightType.Point;
            lightComponent.color = Color.white;
            lightComponent.intensity = 1.75f;
            lightComponent.range = 224;
            lightGameObject.transform.position = transform.position;
            lightGameObject.transform.SetParent(transform, false);
        }
    }
}