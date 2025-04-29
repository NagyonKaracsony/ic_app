using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ship;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Assets.Globals
{
    public static class GlobalUtility
    {
        public static string saveData = string.Empty;
        public static List<SerializableVector3Row> QueueToSerializableMatrix(Queue<Vector3> queue)
        {
            var matrix = new List<SerializableVector3Row>(queue.Count);
            foreach (var vec in queue) matrix.Add(new SerializableVector3Row(vec));
            return matrix;
        }
        public static Queue<Vector3> SerializableMatrixToQueue(List<SerializableVector3Row> matrix)
        {
            var queue = new Queue<Vector3>(matrix.Count);
            foreach (var row in matrix) queue.Enqueue(row.ToVector3());
            return queue;
        }
        public static void SaveGame()
        {
            List<GameObject> planets = GameManager.planets;
            List<Battleship> ships = ShipHandler.battleships;

            PlanetData[] savedPlanets = new PlanetData[planets.Count];
            for (int i = 0; i < planets.Count; i++) savedPlanets[i] = planets[i].GetComponent<Planet>().Save();

            ShipData[] savedShips = new ShipData[ships.Count];
            for (int i = 0; i < ships.Count; i++) savedShips[i] = ships[i].GetComponent<Battleship>().Save();

            Save save = new Save(savedPlanets, savedShips);
            JObject json = JObject.Parse(JsonUtility.ToJson(save, true));
            FixJsonStrings(json);
            System.IO.File.WriteAllText("C:\\Asztali gép\\test\\ship.json", json.ToString(Formatting.None));
        }
        public static void FixJsonStrings(JToken token)
        {
            if (token.Type == JTokenType.Object)
            {
                var obj = (JObject)token;
                foreach (var property in obj.Properties().ToList()) // Use ToList to avoid modifying collection during iteration
                {
                    if (property.Value.Type == JTokenType.String)
                    {
                        string value = property.Value.ToString().Trim();
                        if ((value.StartsWith("{") && value.EndsWith("}")) || (value.StartsWith("[") && value.EndsWith("]")))
                        {
                            try
                            {
                                var parsed = JToken.Parse(value);
                                property.Value = parsed;
                                FixJsonStrings(parsed); // Recursively fix nested strings
                            }
                            catch
                            {
                                // Ignore malformed JSON strings
                            }
                        }
                    }
                    else
                    {
                        FixJsonStrings(property.Value); // Recursive call for non-string nested structures
                    }
                }
            }
            else if (token.Type == JTokenType.Array)
            {
                foreach (var item in (JArray)token) FixJsonStrings(item);
            }
        }
    }
    [System.Serializable]
    public class Save
    {
        public PlanetData[] planets;
        public ShipData[] ships;
        public Save(PlanetData[] planets, ShipData[] ships)
        {
            this.planets = planets;
            this.ships = ships;
        }
    }
}