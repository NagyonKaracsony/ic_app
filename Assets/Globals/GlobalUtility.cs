using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ship;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
namespace Assets.Globals
{
    public static class GlobalUtility
    {
        public static string saveData = string.Empty;
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

            string path = Path.Combine(Application.persistentDataPath, "Saves");
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            File.WriteAllText(path + $"/{save.SaveID}.json", json.ToString(Formatting.None));
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
                    else FixJsonStrings(property.Value); // Recursive call for non-string nested structures
                }
            }
            else if (token.Type == JTokenType.Array) foreach (var item in (JArray)token) FixJsonStrings(item);
        }
    }
    [Serializable]
    public class Save
    {
        public string SaveName = "Save";
        public string SaveID = string.Empty;
        public string CreatedDate = DateTime.Now.ToString();
        public string LastPlayedDate = DateTime.Now.ToString();
        public PlanetData[] planets;
        public ShipData[] ships;
        public Save(PlanetData[] planets, ShipData[] ships)
        {
            SaveID = $"{Guid.NewGuid():N}-{CreatedDate}";
            SaveID = SaveID.Replace(" ", "").Replace(".", "").Replace(":", "");
            this.planets = planets;
            this.ships = ships;
        }
    }
}