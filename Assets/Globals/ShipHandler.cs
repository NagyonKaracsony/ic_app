using Assets;
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
        shipPrefab = new OBJLoader().Load(Path.Combine(Application.streamingAssetsPath, "Ships/stealth.obj"));
        shipPrefab.SetActive(false);
    }
    public static void SetShipTarget(Vector3 destination)
    {
        foreach (var battleship in battleships) battleship.GetComponent<NavMeshAgent>().SetDestination(destination);
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
        ship.transform.localScale = new Vector3(1, 1, 1);
        ship.layer = 6;

        MeshRenderer[] meshRenderers = ship.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in meshRenderers)
        {
            renderer.materials = new Material[] {
                new (ReferenceHolder.Instance.shipMaterial),
                new (ReferenceHolder.Instance.shipMaterial),
                new (ReferenceHolder.Instance.shipMaterial),
                new (ReferenceHolder.Instance.shipMaterial),
            };
        }
        ship.SetActive(true);
        ScaleMesh(ship, 0.00025f);
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
    public static void LoadShip()
    {

    }
}