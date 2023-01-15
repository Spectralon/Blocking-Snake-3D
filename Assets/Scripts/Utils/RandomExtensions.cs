using Random = System.Random;
using UnityEngine;
using System.Collections.Generic;

public static class RandomExtensions
{
    public static bool Chance(this Random rnd, float chance) => rnd.NextDouble() <= chance;

    public static float Range(this Random rnd, float minInclusive, float maxInclusive) => (float)(rnd.NextDouble() * (maxInclusive - minInclusive) + minInclusive);

    public static T Random<T>(this T[] array) => array.Length == 0 ? default : array[GameController.Random.Next(0, array.Length)];
}
