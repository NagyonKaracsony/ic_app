using System.Collections.Generic;
using UnityEngine;
// This class is responsible for generating unique names for planets.
// It uses a combination of prefixes and suffixes to create names.
public static class NameGenerators
{
    public static class PlanetNameGenerator
    {
        // Lists of name parts
        private static readonly string[] prefixes = { "Xan", "Vor", "Zel", "Tar", "Kry", "Lum", "Ael", "Sol", "Neb", "Quo" };
        private static readonly string[] suffixes = { "ion", "ara", "os", "aris", "ium", "ora", "ex", "ax", "ea", "on" };

        // Used names tracker
        private static HashSet<string> usedNames = new HashSet<string>();

        // How many tries before giving up on finding a unique name
        private const int maxAttempts = 100;

        // Public method to generate a unique name
        public static string GenerateUniqueName()
        {
            for (int i = 0; i < maxAttempts; i++)
            {
                string name = GenerateName();
                if (!usedNames.Contains(name))
                {
                    usedNames.Add(name);
                    return name;
                }
            }

            Debug.LogWarning($"[PlanetNameGenerator] Failed to find a unique name after {maxAttempts} attempts.");
            return null;
        }

        // Combines prefix and suffix
        private static string GenerateName()
        {
            string prefix = prefixes[Random.Range(0, prefixes.Length)];
            string suffix = suffixes[Random.Range(0, suffixes.Length)];
            return prefix + suffix;
        }

        // Optionally: clear all used names (e.g. on game restart)
        public static void ResetNameHistory()
        {
            usedNames.Clear();
        }
    }
    public class ShipNameGenerator
    {
        private const string Letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private static HashSet<string> usedNames = new HashSet<string>();
        private static System.Random rng = new System.Random();
        private const int maxAttempts = 100;
        /// <summary>
        /// Generates a ship name in the format "ABC-123", ensuring it hasn't been used before.
        /// Returns null (and logs a warning) if it can't find a unique name after maxAttempts.
        /// </summary>
        public static string GenerateUniqueName()
        {
            string name;
            int attempts = 0;

            do
            {
                name = $"{RandomLetters(3)}-{RandomDigits(3)}";
                attempts++;
            }
            while (usedNames.Contains(name) && attempts < maxAttempts);

            if (usedNames.Contains(name))
            {
                Debug.LogWarning($"[ShipNameGenerator] Failed to find a unique name after {maxAttempts} attempts.");
                return null;
            }

            usedNames.Add(name);
            return name;
        }

        /// <summary>
        /// Returns true if the given ship name has already been issued.
        /// </summary>
        public static bool IsNameUsed(string name)
        {
            return usedNames.Contains(name);
        }

        /// <summary>
        /// Clears the record of used ship names.
        /// </summary>
        public static void ResetUsedNames()
        {
            usedNames.Clear();
        }

        // Helpers
        private static string RandomLetters(int count)
        {
            char[] chars = new char[count];
            for (int i = 0; i < count; i++) chars[i] = Letters[rng.Next(Letters.Length)];
            return new string(chars);
        }
        private static string RandomDigits(int count)
        {
            char[] digits = new char[count];
            for (int i = 0; i < count; i++) digits[i] = (char)('0' + rng.Next(10));
            return new string(digits);
        }
    }
}