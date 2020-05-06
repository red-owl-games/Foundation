using Unity.Mathematics;

namespace RedOwl.Core
{
    public static class ArrayExtensions
    {
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
    }
}