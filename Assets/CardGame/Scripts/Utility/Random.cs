using System.Collections.Generic;
using GameFramework;

namespace CardGame
{
    /// <summary>
    /// Utility extension method to shuffle NativeLists.
    /// </summary>
    public static class Random
    {
        // private static readonly Random Rng = new Random();
        public static void Shuffle<T>(this List<T> list)
        {
            var n = list.Count;
            while (n --> 1)
            {
                var k = Utility.Random.GetRandom(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
        
        public static int GetRatioRandom(int[] rate, int total)
        {
            int r = Utility.Random.GetRandom(1, total + 1);
            int t = 0;
            for (int i = 0; i < rate.Length; i++)
            {
                t += rate[i];
                if (r < t)
                {
                    return i;
                }
            }
            return 0;
        }
    }
    
}