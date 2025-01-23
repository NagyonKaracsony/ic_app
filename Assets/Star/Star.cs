using UnityEngine;
namespace Assets
{
    public class Star : MonoBehaviour
    {
        public Material starMaterial;
        public StarSettings starSettings;
        public void GenerateStar()
        {
            MaterialsHolder materialsHolder = FindObjectOfType<MaterialsHolder>();
            starMaterial = new(materialsHolder.starMaterial);
        }
        public void Save()
        {
            Debug.Log(starMaterial.ToString());
        }
    }
}