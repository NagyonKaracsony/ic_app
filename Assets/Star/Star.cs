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
            lightComponent.intensity = 2;
            lightComponent.range = 128;
            lightGameObject.transform.position = transform.position;
            lightGameObject.transform.SetParent(transform, false);
        }
    }
}