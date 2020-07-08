using System;
using UnityEngine;

namespace RedOwl.Core
{
    // TODO: Could Make Interfaces which allow the SO to tap into the Unity Events
    // public interface IManagerOnUpdate { void OnUpdate(); }
    // public interface IManagerOnStart { void OnStart(); }

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