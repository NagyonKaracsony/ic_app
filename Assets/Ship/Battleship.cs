using Ship;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[Serializable]
public class Battleship : MonoBehaviour, IShip, IBattleship
{
    [SerializeField] private new string name = NameGenerators.ShipNameGenerator.GenerateUniqueName();
    [SerializeField] private ShipType type;
    [SerializeField] private int hullHealthPoints = 5000;
    [SerializeField] private int shieldHealthPoints = 2000;
    [SerializeField] private int damage = 350;
    [SerializeField] private float range = 10;
    [SerializeField] private float speed = 0.75f;
    [SerializeField] private Vector3? currentDestination = null;
    [SerializeField] private Queue<Vector3> destinationQueue = new();
    [SerializeField] private Collider[] colliders = new Collider[255];
    [SerializeField] private byte colliderHits = 0;
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private byte ownerId = 0;

    [SerializeField] private Battleship target;
    [SerializeField] private GameObject shipRange;
    public int ColliderHits
    {
        get => colliderHits;
        set => colliderHits = (byte)value;
    }
    public float Range
    {
        get => range;
        set => range = value;
    }
    public GameObject ShipRange
    {
        get => shipRange;
        set => shipRange = value;
    }
    public Battleship? Target
    {
        get => target;
        set => target = value;
    }
    public string Name
    {
        get => name;
        set => name = value;
    }
    public ShipType Type
    {
        get => type;
        set => type = value;
    }
    public int HullHealthPoints
    {
        get => hullHealthPoints;
        set => hullHealthPoints = value;
    }
    public int ShieldHealthPoints
    {
        get => shieldHealthPoints;
        set => shieldHealthPoints = value;
    }
    public int Damage
    {
        get => damage;
        set => damage = value;
    }
    public float Speed
    {
        get => speed;
        set => speed = value;
    }
    public NavMeshAgent NavMeshAgent
    {
        get => navMeshAgent;
        set => navMeshAgent = value;
    }
    public Collider[] Colliders
    {
        get => colliders;
        set => colliders = value;
    }
    public Vector3? CurrentDestination
    {
        get => currentDestination;
        set => currentDestination = value;
    }
    public Queue<Vector3> DestinationQueue
    {
        get => destinationQueue;
        set => destinationQueue = value;
    }
    public byte ownerID
    {
        get => ownerId;
        set => ownerId = value;
    }
    public event Action OnDestinationReached;
    public void OnDestroy()
    {
        ShipHandler.battleships.Remove(this);
    }
    public void UpdateColliders()
    {
        colliderHits = (byte)Physics.OverlapSphereNonAlloc(transform.position, Range / 2f, Colliders);
    }
    public void SetTarget(Battleship target)
    {
        Target = target;
    }
    public int GetTotalHealthPoints()
    {
        return HullHealthPoints + ShieldHealthPoints;
    }
    public void DamageTarget()
    {
        int DamageToDeal = Damage;
        if (Target.ShieldHealthPoints <= DamageToDeal)
        {
            DamageToDeal -= Target.ShieldHealthPoints;
            Target.ShieldHealthPoints = 0;
        }
        else
        {
            Target.ShieldHealthPoints -= DamageToDeal;
            DamageToDeal = 0;
        }
        if (DamageToDeal > 0)
        {
            if (Target.HullHealthPoints <= DamageToDeal)
            {
                Target.HullHealthPoints = 0;
                Target.Die();
                LoseTarget();
            }
            else Target.HullHealthPoints -= DamageToDeal;
        }
    }
    public void ContributeToCapture()
    {
        if (ColliderHits != 0)
        {
            List<Sector> NerbySectors = new();
            for (int y = 0; y < colliderHits; y++)  if (Colliders[y].gameObject.layer == 10) NerbySectors.Add(Colliders[y].gameObject.GetComponent<Sector>());

            Sector closestSector = null;
            float closestSqrDistance = float.MaxValue;
            for (int y = 0; y < NerbySectors.Count; y++)
            {
                float sqrDist = (NerbySectors[y].transform.position - transform.position).sqrMagnitude;
                if (sqrDist < closestSqrDistance)
                {
                    closestSector = NerbySectors[y];
                    closestSqrDistance = sqrDist;
                }
            }
            if (closestSector != null)
            {
                if (closestSector.OwnerID != ownerId) closestSector.IncreaseCaptureStatus(ownerId, 1f);
            }
        }
    }
    public void LoseTarget()
    {
        Target = null;
    }
    public void Die()
    {
        ShipHandler.battleships.Remove(this);
        Destroy(gameObject);
    }
    public ShipData Save()
    {
        return new ShipData(this);
    }
    public void ShowRange()
    {
        Vector3 temp = transform.position;
        temp.y = transform.position.y + 0.025f;
        shipRange = Instantiate(ShipHandler.ShipRange, temp, Quaternion.identity);
        shipRange.transform.parent = transform;
        shipRange.transform.localScale = new Vector3(range, 0.1f, range);
        shipRange.SetActive(true);
    }
    public void HideRange()
    {
        Destroy(shipRange);
        shipRange = null;
    }
    public void SetDestination(Vector3 destination)
    {
        if (currentDestination.HasValue)
        {
            destinationQueue.Enqueue(destination);
            return;
        }
        float distance = Vector3.Distance(transform.position, destination);
        currentDestination = destination;
    }
    // obsolete
    public void RemoveLastQueuedDestination()
    {
        // removes the last destination<vector3> from the queue
        // most performance efficient way to do it?
        Queue<Vector3> tempQueue = new Queue<Vector3>();
        int count = destinationQueue.Count;
        for (int i = 0; i < count - 1; i++) tempQueue.Enqueue(destinationQueue.Dequeue());
        destinationQueue = tempQueue;
    }
    private void MoveToDestination()
    {
        if (!currentDestination.HasValue) return;
        transform.position = Vector3.MoveTowards(transform.position, currentDestination.Value, Speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, currentDestination.Value) < 0.1f)
        {
            currentDestination = null;
            OnDestinationReached?.Invoke();
            if (destinationQueue.Count > 0) SetDestination(destinationQueue.Dequeue());
        }
    }
}