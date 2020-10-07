using System;
using UnityEngine;

namespace RedOwl.Core
{
    public interface IConnectable
    {
        GameObject gameObject { get; }
    }
    
    public interface IConnectionTarget : IConnectable
    {
        void RegisterSource(IConnectionSource source);
    }

    public interface IConnectionSource : IConnectable
    {
        event Action ConnectionTriggered;
        bool ConnectionState { get; }
    }


}