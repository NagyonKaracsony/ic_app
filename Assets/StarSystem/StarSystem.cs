using Assets;
using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
public class StarSystem : MonoBehaviour
{
    private GameObject SystemSectors;
    private void Start()
    {
        SystemSectors = new("SystemSectors");
        SystemSectors SystemSectorsComponent = SystemSectors.AddComponent<SystemSectors>();
        SystemSectorsComponent.CreateNew(new Material(ReferenceHolder.Instance.sectorMaterial), 8);

        Instantiate(ReferenceHolder.Instance.StarPrefab, transform.transform);
        StartCoroutine(CreateNavMesh());
    }
    public IEnumerator CreateNavMesh()
    {
        NavMeshObstacle navMeshObstacleComponent = SystemSectors.AddComponent<NavMeshObstacle>();
        navMeshObstacleComponent.shape = NavMeshObstacleShape.Capsule;
        navMeshObstacleComponent.carving = true;
        navMeshObstacleComponent.carveOnlyStationary = true;
        navMeshObstacleComponent.radius = 3f;

        yield return new WaitForEndOfFrame();
        gameObject.GetComponent<NavMeshSurface>().BuildNavMesh();

        ShipHandler.SpawnShip(new Vector3(10, 0, 20), 0, 255);
        ShipHandler.SpawnShip(new Vector3(10, 0, 30), 1, 255);
        ShipHandler.SetShipTarget(new Vector3(20, 0, 20));
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