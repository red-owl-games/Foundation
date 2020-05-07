using System;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace RedOwl.Core
{
    public class RedOwlException : Exception
    {
        public RedOwlException() {}

        public RedOwlException(string message) : base(message) {}

        public RedOwlException(string message, Exception inner) : base(message, inner) {}
    }
    
    public static partial class RedOwl
    {
        public static ServiceCache Services = new ServiceCache();
        public static TypeCache Types = new TypeCache();
        
        public static Random Random;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void Initialize()
        {
            Random = Create();
        }
        
        public static Random Create() => new Random((uint)Environment.TickCount);
        public static Random Create(uint seed) => new Random(seed);
    }
}