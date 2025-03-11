using System;
using System.Collections.Generic;
using UnityEngine;
public class Ship : MonoBehaviour
{
    public string Name { get; private set; }
    public ShipType Type { get; private set; }
    public float Speed { get; private set; } = 1.0f;
    public float Fuel { get; private set; } = 100f;
    public float FuelConsumptionRate { get; private set; } = 1f; // Fuel per unit distance

    private Vector3? currentDestination;
    private Queue<Vector3> destinationQueue = new Queue<Vector3>();

    public event Action OnDestinationReached;
    public event Action<float> OnFuelChanged; // Notify UI or other systems
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
        float fuelNeeded = distance * FuelConsumptionRate;

        if (Fuel >= fuelNeeded)
        {
            Fuel -= fuelNeeded;
            currentDestination = destination;
        }
        else
        {
            Debug.Log($"{Name} does not have enough fuel to reach this destination!");
        }
        OnFuelChanged?.Invoke(Fuel);
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
            Fuel += distance * FuelConsumptionRate;
            OnFuelChanged?.Invoke(Fuel);
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
public enum ShipType
{
    Transport,
    Battleship,
    Miner
}
