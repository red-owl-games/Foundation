using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;

namespace RedOwl.Core
{
    public static class ArrayExtensions
    {
        /// <summary>
        /// Performs a fisher-yates shuffle on the array.
        /// </summary>
        /// <typeparam name="T">The type of the array.</typeparam>
        /// <param name="self">The array to shuffle.</param>
        /// <param name="rng">The random number generator to use.</param>
        /// <returns>A fisher-yates shuffled array.</returns>
        public static T[] Shuffle<T>(this T[] self, Random rng)
        {
            //Fisher Yates Shuffle
            int n = self.Length;
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
        
        /// <summary>
        /// Splits an array into several smaller arrays.
        /// </summary>
        /// <typeparam name="T">The type of the array.</typeparam>
        /// <param name="self">The array to split.</param>
        /// <param name="size">The size of the smaller arrays.</param>
        /// <returns>An array containing smaller arrays.</returns>
        public static IEnumerable<IEnumerable<T>> GroupByCount<T>(this T[] self, int size)
        {
            for (int i = 0; i < (float)self.Length / size; i++)
            {
                yield return self.Skip(i * size).Take(size);
            }
        }
    }
}