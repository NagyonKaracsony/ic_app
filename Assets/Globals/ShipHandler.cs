using Dummiesman;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;
public class ShipHandler : MonoBehaviour
{
    public static List<Battleship> battleships = new List<Battleship>();
    public static GameObject shipPrefab;
    private void Awake()
    {
        shipPrefab = new OBJLoader().Load(Path.Combine(Application.streamingAssetsPath, "Ships/ship.obj"));
        shipPrefab.SetActive(false);
    }
    public static void SetShipTarget(Vector3 destinition)
    {
        foreach (var battleship in battleships) battleship.GetComponent<NavMeshAgent>().SetDestination(destinition);
    }
    public static void HandleCombatTick()
    {
        // a tad bit wasteful, but it should work
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
                    int colliderHits = battleship.ColliderHits; // saves a few cycles  
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
                    int targetsCount = targets.Count; // saves a few cycles
                    for (int y = 0; y < targetsCount; y++)
                    {
                        // this is also to prevent the battleship from seeing itself
                        // OverlapSphereNonAlloc also returns the collider of the battleship itself
                        // not sure if this is just a quirk of unity or if this is a bug
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
        ship.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        ship.layer = 6;
        ship.SetActive(true);
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
            GameObject ship = CreateShipInstance(position, civilizationID);
            Battleship shipComponent = ship.AddComponent<Battleship>();
            SphereCollider colliderComponent = ship.AddComponent<SphereCollider>();
            NavMeshAgent navMeshAgent = ship.AddComponent<NavMeshAgent>();
            shipComponent.ownerID = civilizationID;
            navMeshAgent.speed = 1f;
            battleships.Add(shipComponent);
        }
    }
}