using System;
using System.Collections.Generic;
namespace Assets
{
    [Serializable]
    public class Civilization
    {
        // Empire Basics
        public string empireName;
        public string governmentType;
        public string speciesName;

        // Resources
        public int Energy;
        public int Metals;
        public int Food;
        public int Alloy;
        public int Consumer;
        public int sciencePhysics;
        public int scienceSociety;
        public int scienceEngineering;

        // Military & Power
        public int totalFleetPower;
        public int numberOfShips;
        public int navalCap;

        // Territory
        public int numberOfPlanets;
        public int numberOfSystems;
        public List<string> controlledSystems = new();

        // Traits / Policies
        public List<string> traits = new();
        public Dictionary<string, string> policies = new();

        // Diplomacy
        public List<string> knownEmpires = new();
        public List<string> rivals = new();
        public List<string> allies = new();

        // Technologies
        public HashSet<string> unlockedTechnologies = new();

        // Custom values (for scripting & event flexibility)
        public Dictionary<string, int> customVariables = new();

        public Civilization(string name)
        {
            empireName = name;
            governmentType = "Democracy";
            speciesName = "Humanity";
            Energy = 2000;
            Metals = 1000;
            Food = 2000;
            Alloy = 500;
            Consumer = 1000;
            sciencePhysics = 0;
            scienceSociety = 0;
            scienceEngineering = 0;
            totalFleetPower = 0;
            numberOfShips = 0;
            navalCap = 20;
            numberOfPlanets = 1;
            numberOfSystems = 1;
        }

        // Example condition helpers
        public bool HasTechnology(string techId) => unlockedTechnologies.Contains(techId);
        public bool HasTrait(string traitId) => traits.Contains(traitId);
        public bool HasFleetPowerAbove(int amount) => totalFleetPower >= amount;

        // Modify resources
        public void AddResource(string resourceName, int amount)
        {
            switch (resourceName.ToLower())
            {
                case "energy": Energy += amount; break;
                case "metal": Metals += amount; break;
                case "food": Food += amount; break;
                case "alloy": Alloy += amount; break;
                case "consumer": Consumer += amount; break;
                case "physics": sciencePhysics += amount; break;
                case "society": scienceSociety += amount; break;
                case "engineering": scienceEngineering += amount; break;
                default:
                    if (!customVariables.ContainsKey(resourceName)) customVariables[resourceName] = 0;
                    customVariables[resourceName] += amount;
                    break;
            }
        }
        // Get resources
        public int GetResource(string resourceName)
        {
            return resourceName.ToLower() switch
            {
                "energy" => Energy,
                "metal" => Metals,
                "food" => Food,
                "alloy" => Alloy,
                "consumer" => Consumer,
                "physics" => sciencePhysics,
                "society" => scienceSociety,
                "engineering" => scienceEngineering,
                _ => customVariables.TryGetValue(resourceName, out var value) ? value : 0
            };
        }
        // Set resources
        public void SetResource(string resourceName, int amount)
        {
            switch (resourceName.ToLower())
            {
                case "energy":
                    Energy = amount;
                    break;
                case "metal":
                    Metals = amount;
                    break;
                case "food":
                    Metals = amount;
                    break;
                case "alloy":
                    Metals = amount;
                    break;
                case "consumer":
                    Consumer = amount;
                    break;
                case "physics":
                    Metals = amount;
                    break;
                case "society":
                    Metals = amount;
                    break;
                case "engineering":
                    Metals = amount;
                    break;
                default:
                    if (!customVariables.ContainsKey(resourceName)) customVariables[resourceName] = 0;
                    customVariables[resourceName] = amount;
                    break;
            }
        }
    }
}