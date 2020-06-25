/*
using System;
using Flag = Project.ExampleFlags;

namespace Project
{
    [Flags]
    public enum ExampleFlags
    {
        None = 0,
        Flag1 = 1,
        Flag2 = 2,
        Flag3 = 4
    }

    static class ExampleFlagsExtensions
    {
        public static bool Has(this Flag type, Flag value)
        {
            return (type & value) == value;
        }

        public static bool Is(this Flag type, Flag value)
        {
            return type == value;
        }

        public static Flag Add(this Flag type, Flag value)
        {
            return type | value;
        }

        public static Flag Remove(this Flag type, Flag value)
        {
            return type & ~value;
        }
    }
}
*/