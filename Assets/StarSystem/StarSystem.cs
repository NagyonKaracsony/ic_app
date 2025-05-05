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
        byte spawnCount = 0;
        byte maxSpawnCount = 80;
        byte totalSpawnCount = 0;
        for (byte i = 0; i < 150; i++)
        {
            spawnCount = (byte)Random.Range(3, 6);
            Vector3 point = Vector3.zero;
            if (CheckSpawn(transform.position, 200, out point))
            {
                ShipHandler.SpawnShip(point, 0, spawnCount);
                totalSpawnCount += spawnCount;
                if (totalSpawnCount >= maxSpawnCount) break;
            }
        }
    }

    bool CheckSpawn(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }
}
