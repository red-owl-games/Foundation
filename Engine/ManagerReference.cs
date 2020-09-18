using System;
using System.Collections;
using UnityEngine;

namespace RedOwl.Core
{
    // Used When<event> to not conflict with unity's On<event>
    public interface IManagerOnAwake
    {
        void WhenAwake();
        void WhenDestroy();
    }
    
    public interface IManagerOnStart { void WhenStart(); }
    public interface IManagerOnStartAsync { IEnumerator WhenStart(); }

    public interface IManagerOnEnable
    {
        void WhenEnable();
        void WhenDisable();
    }
    
    public interface IManagerOnUpdate { void WhenUpdate(); }
    public interface IManagerOnFixedUpdate { void WhenFixedUpdate(); }
    public interface IManagerOnLateUpdate { void WhenLateUpdate(); }

    public abstract class ManagerReference : ScriptableObject
    {
        internal abstract void Bind();
    }

    public abstract class ManagerReference<T> : ManagerReference where T : ManagerReference<T>
    {
        private static readonly Lazy<T> Instance = new Lazy<T>(Game.Find<T>);
        public static T Current => Instance.Value;

        internal override void Bind() => Game.Bind((T)this);
    }
}