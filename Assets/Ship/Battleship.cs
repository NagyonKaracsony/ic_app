using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Battleship : MonoBehaviour, IShip, IBattleship
{
    public string Name { get; set; }
    public ShipType Type { get; set; }
    public int HullHealthPoints { get; set; } = 5000;
    public int ShieldHealthPoints { get; set; } = 2000;
    public int Damage { get; set; } = 350;
#nullable enable
    public Battleship? Target { get; set; } = null;
#nullable disable
    public float Speed { get; set; } = 0.75f;
    public Vector3? currentDestination { get; set; }
    public Queue<Vector3> destinationQueue { get; set; } = new Queue<Vector3>();
    public event Action OnDestinationReached;
    public Collider[] Colliders { get; set; } = new Collider[255];
    public byte ColliderHits { get; set; } = 0;
    public NavMeshAgent NavMeshAgent { get; set; } = new NavMeshAgent();
    public byte ownerID { get; set; } = 0;
    public void OnDestroy()
    {
        ShipHandler.battleships.Remove(this);
    }
    public void UpdateColliders()
    {
        ColliderHits = (byte)Physics.OverlapSphereNonAlloc(transform.position, 1f, Colliders);
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
    public void LoseTarget()
    {
        Target = null;
    }
    public void Die()
    {
        ShipHandler.battleships.Remove(this);
        Destroy(gameObject);
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