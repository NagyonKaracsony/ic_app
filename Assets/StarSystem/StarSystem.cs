using Assets;
using UnityEngine;
public class StarSystem : MonoBehaviour
{
    public GameObject StarPrefab;
    private GameObject SystemSectors;
    private GameObject star;
    void Start()
    {
        MaterialsHolder materialsHolder = FindObjectOfType<MaterialsHolder>();
        SystemSectors = new("SystemSectors");
        SystemSectors SystemSectorsComponent = SystemSectors.AddComponent<SystemSectors>();
        SystemSectorsComponent.CreateNew(new Material(materialsHolder.sectorMaterial), 8);

        Instantiate(StarPrefab, transform.transform);
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