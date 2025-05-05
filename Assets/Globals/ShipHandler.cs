using Ship;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ShipHandler : MonoBehaviour
{
    public static List<Battleship> battleships = new();
    public static GameObject shipPrefab;
    public static GameObject ShipRange;
    private void Awake()
    {
        ShipRange = Resources.Load<GameObject>("Prefabs/Ships/Range");
        ShipRange.SetActive(false);
        shipPrefab = ReferenceHolder.Instance.StealthShipModel;
    }
    public static void SetShipTarget(Vector3 destination)
    {
        foreach (var battleship in battleships) battleship.GetComponent<NavMeshAgent>().SetDestination(destination);
    }
    public static void HandleCombatTick()
    {
        for (int i = 0; i < battleships.Count; i++)
        {
            Battleship battleship = battleships[i];
            if (battleship.Target != null) battleship.DamageTarget();
            else
            {
                battleship.UpdateColliders();
                if (battleship.ColliderHits != 0)
                {
                    List<Battleship> targets = new();
                    int colliderHits = battleship.ColliderHits;
                    for (int y = 0; y < colliderHits; y++)
                    {
                        if (battleship.Colliders[y].gameObject.layer == 6)
                        {
                            if (battleship.ownerID == battleship.Colliders[y].gameObject.GetComponent<Battleship>().ownerID) continue;
                            targets.Add(battleship.Colliders[y].gameObject.GetComponent<Battleship>());
                        }
                    }
                    Battleship closestTarget = null;
                    float closestSqrDistance = float.MaxValue;
                    int targetsCount = targets.Count;
                    for (int y = 0; y < targetsCount; y++)
                    {
                        float sqrDist = (targets[y].transform.position - battleship.transform.position).sqrMagnitude;
                        if (sqrDist < closestSqrDistance)
                        {
                            closestTarget = targets[y];
                            closestSqrDistance = sqrDist;
                        }
                    }
                    battleship.SetTarget(closestTarget);
                }
            }
        }
    }
    public static GameObject CreateShipInstance(Vector3 position, byte civilizationID)
    {
        GameObject ship = Instantiate(shipPrefab, position, Quaternion.identity);
        ship.name = $"Ship - {civilizationID}";
        ship.layer = 6;
        ship.SetActive(true);
        ScaleMesh(ship, 0.00015f);
        return ship;
    }
    public static void SpawnShip(Vector3 position, byte civilizationID)
    {
        GameObject ship = CreateShipInstance(position, civilizationID);
        Battleship shipComponent = ship.AddComponent<Battleship>();
        SphereCollider colliderComponent = ship.AddComponent<SphereCollider>();
        NavMeshAgent navMeshAgent = ship.AddComponent<NavMeshAgent>();
        shipComponent.ownerID = civilizationID;
        navMeshAgent.speed = 1f;
        battleships.Add(shipComponent);
    }
    public static void SpawnShip(Vector3 position, byte civilizationID, byte amount)
    {
        for (int i = 0; i < amount; i++)
        {
            position.x += Random.Range(-i, i);
            position.z += Random.Range(-i, i);
            GameObject ship = CreateShipInstance(position, civilizationID);
            Battleship shipComponent = ship.AddComponent<Battleship>();
            SphereCollider colliderComponent = ship.AddComponent<SphereCollider>();
            NavMeshAgent navMeshAgent = ship.AddComponent<NavMeshAgent>();

            colliderComponent.radius = 1f;
            shipComponent.ownerID = civilizationID;
            navMeshAgent.speed = 1f;
            battleships.Add(shipComponent);
        }
    }
    public static void SpawnShip(ShipData shipData)
    {
        Vector3 currentPosition = new(shipData.currentPosition[0], shipData.currentPosition[1], shipData.currentPosition[2]);
        byte civilizationID = shipData.ownerID;
        GameObject ship = CreateShipInstance(currentPosition, civilizationID);
        Battleship shipComponent = ship.AddComponent<Battleship>();

        shipComponent.name = shipData.Name;
        shipComponent.Type = shipData.Type;
        shipComponent.HullHealthPoints = shipData.HullHealthPoints;
        shipComponent.ShieldHealthPoints = shipData.ShieldHealthPoints;
        shipComponent.Damage = shipData.Damage;
        shipComponent.Range = shipData.Range;

        SphereCollider colliderComponent = ship.AddComponent<SphereCollider>();
        NavMeshAgent navMeshAgent = ship.AddComponent<NavMeshAgent>();
        shipComponent.ownerID = civilizationID;
        battleships.Add(shipComponent);
    }

    // Scales the mesh of the ship to the given scale factor
    // Needed because the OBJ loader loads the scale of the original object
    // Resulting in the model being crazy huge, like 10-40.000 times bigger than it should be
    public static void ScaleMesh(GameObject obj, float scaleFactor)
    {
        MeshFilter[] filters = obj.GetComponentsInChildren<MeshFilter>();
        foreach (MeshFilter filter in filters)
        {
            Mesh mesh = filter.mesh;
            Vector3[] verts = mesh.vertices;
            for (int i = 0; i < verts.Length; i++) verts[i] *= scaleFactor;
            mesh.vertices = verts;
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
        }
    }
}