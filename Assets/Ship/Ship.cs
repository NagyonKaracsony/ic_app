using System;
using System.Collections.Generic;
using UnityEngine;
public enum ShipType
{
    Transport,
    Miner,
    LigthBattleship,
    MediumBattleship,
    HeavyBattleship,
    ColossalBattleship,
    StealthBattleship,
}
public enum Modules
{
    None,
    Engine,
}

public interface IShip
{
    public string Name { get; set; }
    public ShipType Type { get; set; }
    public float Speed { get; set; }
    public List<Modules> modules { get; set; }
    public void SetDestination(Vector3 destination);
    public void RemoveLastQueuedDestination();
}
public interface ITransport
{
    public int Capacity { get; set; }
    public int CurrentLoad { get; set; }
    public int LoadingSpeed { get; set; }
}
public interface IMiner
{
    public int Capacity { get; set; }
    public int CurrentLoad { get; set; }
    public int MiningSpeed { get; set; }
}
public interface IBattleship
{

}
public class Ship : MonoBehaviour
{
    public string Name { get; private set; }
    public ShipType Type { get; private set; }
    public float Speed { get; private set; } = 1.0f;

    private Vector3? currentDestination;
    private Queue<Vector3> destinationQueue = new Queue<Vector3>();

    public event Action OnDestinationReached;
    private void Update()
    {
        if (currentDestination.HasValue)
        {
            MoveToDestination();
        }
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

    public void RemoveQueuedDestination(Vector3 destination)
    {
        if (destinationQueue.Contains(destination))
        {
            List<Vector3> tempList = new List<Vector3>(destinationQueue);
            tempList.Remove(destination);
            destinationQueue = new Queue<Vector3>(tempList);

            // Refund fuel
            float distance = Vector3.Distance(transform.position, destination);
        }
    }

    private void MoveToDestination()
    {
        if (!currentDestination.HasValue) return;

        transform.position = Vector3.MoveTowards(transform.position, currentDestination.Value, Speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, currentDestination.Value) < 0.1f)
        {
            Debug.Log($"{Name} reached {currentDestination.Value}");

            currentDestination = null;
            OnDestinationReached?.Invoke();

            if (destinationQueue.Count > 0)
            {
                SetDestination(destinationQueue.Dequeue());
            }
        }
    }
}