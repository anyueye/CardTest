using System.Collections.Generic;
using System;

namespace CardGame
{
    /// <summary>
    /// Utility extension method to shuffle NativeLists.
    /// </summary>
    public static class ListShuffle
    {
        private static readonly Random Rng = new Random();

        public static void Shuffle<T>(this List<T> list)
        {
            var n = list.Count;
            while (n --> 1)
            {
                var k = Rng.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}