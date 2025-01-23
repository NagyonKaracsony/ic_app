using System;
/// <summary>
/// Represents a seed system for generating random numbers based on a seed.
/// </summary>
public class SeedSystem
{
    private Random random;
    /// <summary>
    /// Initializes a new instance of the <see cref="SeedSystem"/> class with a specified seed.
    /// </summary>
    /// <param name="seed">The seed for the random number generator.</param>
    /// <example>
    /// <code>
    /// long seed = 123456789;
    /// SeedSystem seedSystem = new SeedSystem(seed);
    /// int randomInt = seedSystem.GetRandomInt(1, 100);
    /// double randomDouble = seedSystem.GetRandomDouble(1.0, 10.0);
    /// </code>
    /// </example>
    public SeedSystem(long seed)
    {
        random = new Random((int)(seed ^ (seed >> 32))); // XOR the high and low 32 bits for better distribution
    }
    /// <summary>
    /// Gets a random integer within a specified range.
    /// </summary>
    /// <param name="min">The inclusive lower bound of the random number returned.</param>
    /// <param name="max">The exclusive upper bound of the random number returned.</param>
    /// <returns>A random integer between <paramref name="min"/> and <paramref name="max"/>.</returns>
    /// <example>
    /// <code>
    /// SeedSystem seedSystem = new SeedSystem(123456789);
    /// int randomInt = seedSystem.GetRandomInt(1, 100);
    /// Console.WriteLine(randomInt);
    /// </code>
    /// </example>
    public int GetRandomInt(int min, int max)
    {
        return random.Next(min, max);
    }
    /// <summary>
    /// Gets a random double within a specified range.
    /// </summary>
    /// <param name="min">The inclusive lower bound of the random number returned.</param>
    /// <param name="max">The exclusive upper bound of the random number returned.</param>
    /// <returns>A random double between <paramref name="min"/> and <paramref name="max"/>.</returns>
    /// <example>
    /// <code>
    /// SeedSystem seedSystem = new SeedSystem(123456789);
    /// double randomDouble = seedSystem.GetRandomDouble(1.0, 10.0);
    /// Console.WriteLine(randomDouble);
    /// </code>
    /// </example>
    public double GetRandomDouble(double min, double max)
    {
        return min + (random.NextDouble() * (max - min));
    }
}