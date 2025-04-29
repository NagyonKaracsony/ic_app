using System;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
[Serializable]
public enum ShipType
{
    Battleship
}
public interface IShip
{
    public string Name { get; set; }
    public ShipType Type { get; set; }
    public int HullHealthPoints { get; set; }
    public int ShieldHealthPoints { get; set; }
    public float Speed { get; set; }
    public NavMeshAgent NavMeshAgent { get; set; }
    public Collider[] Colliders { get; set; }
    public Vector3? CurrentDestination { get; set; }
    public Queue<Vector3> DestinationQueue { get; set; }
    public event Action OnDestinationReached;
    public void SetDestination(Vector3 destination);
    public void RemoveLastQueuedDestination();
    public byte ownerID { get; set; }
}
public interface IBattleship
{
    public int Damage { get; set; }
#nullable enable
    public Battleship? Target { get; set; }
#nullable disable
}