using System.Collections.Generic;
using Unity.Mathematics;

namespace RedOwl.Core
{
    public static class ListExtensions
    {
        public static List<T> Shuffle<T>(this List<T> self, Random rng)
        {
            //Fisher Yates Shuffle
            int n = self.Count;
            while (n > 1)
            {
                n--;
                int k = rng.NextInt(0, n);
                T value = self[k];
                self[k] = self[n];
                self[n] = value;
            }

            return self;
        }
    }
}