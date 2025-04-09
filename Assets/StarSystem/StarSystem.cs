using Assets;
using UnityEngine;
public class StarSystem : MonoBehaviour
{
    private GameObject SystemSectors;
    private GameObject star;
    void Start()
    {
        MaterialsHolder materialsHolder = FindObjectOfType<MaterialsHolder>();
        RefrenceHolder refrenceHolder = FindObjectOfType<RefrenceHolder>();
        SystemSectors = new("SystemSectors");
        SystemSectors SystemSectorsComponent = SystemSectors.AddComponent<SystemSectors>();
        SystemSectorsComponent.CreateNew(new Material(materialsHolder.sectorMaterial), 8);

        Instantiate(refrenceHolder.StarPrefab, transform.transform);
    }
    void Update()
    {

    }
    private StarSystem LoadStarSystemFromSave(string saveId)
    {
        return null;
    }
    private StarSystem CreateSystem()
    {
        return null;
    }
}