using System;
using System.Collections.Generic;
using Random = Unity.Mathematics.Random;

namespace RedOwl.Engine
{
    public static class ArrayExtensions
    {
        public static T[] RemoveAt<T>(this T[] self, int index)
        {
            int length = self.Length;
            var output = new T[length - 1];

            int i = 0;
            int j = 0;
            while (i < length)
            {
                if (i != index)
                {
                    output[j] = self[i];
                    j++;
                }
                i++;
            }

            return output;
        }
        
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
        
        /*
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
        */
        
        public static T SafeGet<T>(this T[] self, int index, T defaultValue)
        {
            return index < self.Length ? self[index] : defaultValue;
        }
        
        public static void Reconcile(int[] self, int[] other, Action<int> onAdd = null, Action<int> onRemove = null, Action<int> onKeep = null)
        {
            int i = 0;
            int j = 0;
            int iLength = self.Length;
            int jLength = other.Length;
            Array.Sort(self);
            Array.Sort(other);
            
            while (i < iLength && j < jLength)
            {
                if (self[i] == other[j])
                {
                    onKeep?.Invoke(self[i]);
                    i += 1;
                    j += 1;
                }
                else if (self[i] < other[j])
                {
                    onRemove?.Invoke(self[i]);
                    i += 1;
                }
                else
                {
                    onAdd?.Invoke(other[j]);
                    j += 1;
                }
            }

            while (i < iLength)
            {
                onRemove?.Invoke(self[i]);
                i += 1;
            }

            while (j < jLength)
            {
                onAdd?.Invoke(other[j]);
                j += 1;
            }
        }
    }
}