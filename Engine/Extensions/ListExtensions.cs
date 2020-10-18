using System.Collections.Generic;
using Unity.Mathematics;

namespace RedOwl.Core
{
    public static class ListExtensions
    {
        public static IList<T> Shuffle<T>(this IList<T> self, Random rng)
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
        
        public static T SafeGet<T>(this IList<T> self, int index, T defaultValue)
        {
            return index < self.Count ? self[index] : defaultValue;
        }
    }
}