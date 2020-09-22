using System;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = Unity.Mathematics.Random;

namespace RedOwl.Core
{
    public class RedOwlException : Exception
    {
        public RedOwlException() {}

        public RedOwlException(string message) : base(message) {}

        public RedOwlException(string message, Exception inner) : base(message, inner) {}
    }

    public static partial class Game
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        internal static void Initialize()
        {
            Log.Always("Initialize RedOwl Game!");
        }

        #region Random

        public static Random Random => new Random((uint)Environment.TickCount);

        #endregion
        
        #region Services

        private static ServiceCache _services;
        public static ServiceCache Services => _services ?? (_services = new ServiceCache());

        public static void Bind<T>(T instance) => Services.Bind(instance);
        public static void BindAs<T>(T instance) => Services.BindAs(instance);
        public static T Find<T>() => Services.Find<T>();
        public static void Inject(object obj) => Services.Inject(obj);
        
        #endregion

        
    }
}