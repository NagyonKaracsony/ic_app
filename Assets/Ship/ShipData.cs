using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
namespace Ship
{
    [System.Serializable]
    public class ShipData
    {
        public string Name;
        public ShipType Type;
        public int HullHealthPoints;
        public int ShieldHealthPoints;
        public int Damage;
        public float Range;
        public float Speed;
#nullable enable
        public float[] currentPosition;
        public float[] currentDestination;
        public List<SerializableVector3Row>? destinationMatrix = new();
#nullable disable
        public byte ColliderHits = 0;
        public byte ownerID;
        public Battleship Target = null;
        public Collider[] Colliders = null;
        public NavMeshAgent NavMeshAgent = null;
        public GameObject shipRange = null;
        public ShipData(Battleship ship)
        {
            Name = ship.Name;
            Type = ship.Type;
            HullHealthPoints = ship.HullHealthPoints;
            ShieldHealthPoints = ship.ShieldHealthPoints;
            Damage = ship.Damage;
            Range = ship.Range;
            Speed = ship.Speed;
            currentPosition = SerializableVector3Row.Vector3NullableToArray(ship.transform.position);

            if (ship.DestinationQueue.Count == 0) currentDestination = null;
            else currentDestination = SerializableVector3Row.Vector3NullableToArray(ship.DestinationQueue.First());

            if (ship.DestinationQueue.Count == 0) destinationMatrix = null;
            else destinationMatrix = VectorQueueConverter.QueueToSerializableMatrix(ship.DestinationQueue);

            ownerID = ship.ownerID;
            DestinationMatrixWrapper wrapper = new() { destinationMatrix = VectorQueueConverter.QueueToSerializableMatrix(ship.DestinationQueue) };
        }
    }
    [System.Serializable]
    public class SerializableVector3Row
    {
        public float x;
        public float y;
        public float z;
        public SerializableVector3Row(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public SerializableVector3Row(Vector3 v)
        {
            x = v.x;
            y = v.y;
            z = v.z;
        }
        public Vector3 ToVector3() => new(x, y, z);
        public static float[] Vector3NullableToArray(Vector3? vec)
        {
            return vec.HasValue ? new[] { vec.Value.x, vec.Value.y, vec.Value.z } : null;
        }
        public static Vector3? ArrayToVector3Nullable(float[] arr)
        {
            if (arr == null || arr.Length < 3) return null;
            return new(arr[0], arr[1], arr[2]);
        }
    }
    [System.Serializable]
    public class DestinationMatrixWrapper
    {
        public List<SerializableVector3Row> destinationMatrix;
        public DestinationMatrixWrapper() { destinationMatrix = new(); }
    }
    public static class VectorQueueConverter
    {
        public static List<SerializableVector3Row> QueueToSerializableMatrix(Queue<Vector3> queue)
        {
            List<SerializableVector3Row> matrix = new(queue.Count);
            foreach (var vec in queue) matrix.Add(new(vec));
            return matrix;
        }
        public static Queue<Vector3> SerializableMatrixToQueue(List<SerializableVector3Row> matrix)
        {
            Queue<Vector3> queue = new(matrix.Count);
            foreach (var row in matrix) queue.Enqueue(row.ToVector3());
            return queue;
        }
    }
}