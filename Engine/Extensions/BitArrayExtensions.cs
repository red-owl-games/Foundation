using System;
using System.Collections;

namespace RedOwl.Engine
{
    public static class BitArrayExtensions
    {
        public static int CountBits(this BitArray self)
        {
            int output = 0;
            for (int i = 0; i < self.Length; i++)
            {
                if (self[i])
                    output += 1;
            }

            return output;
        }

        public static int[] GetEnabledBits(this BitArray self)
        {
            var output = new int[self.CountBits()];
            int count = 0;
            for (int i = 0; i < self.Length; i++)
            {
                if (!self[i]) continue;
                output[count] = i;
                count += 1;
            }
            return output;
        }
        
        public static int[] GetDisabledBits(this BitArray self)
        {
            var output = new int[self.CountBits()];
            int count = 0;
            for (int i = 0; i < self.Length; i++)
            {
                if (!self[i]) continue;
                output[count] = i;
                count += 1;
            }
            return output;
        }
        
        public static int GetNextBit(this BitArray self)
        {
            for (int i = 0; i < self.Length; i++)
            {
                if (self[i]) continue;
                return i;
            }

            return -1;
        }

        public static int ToBitMask(this BitArray self)
        {
            int output = 0;
            for (int i = 0; i < self.Length; i++)
            {
                if (self[i] == false) continue;
                output |= 1 << i;
            }

            return output;
        }

        public static string PrintBitMask(this BitArray self)
        {
            return $"{Convert.ToString(self.ToBitMask(), 2).PadLeft(self.Length, '0')}";
        }
    }
}