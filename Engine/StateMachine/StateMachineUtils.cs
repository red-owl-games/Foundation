using System;

namespace RedOwl.Core
{
    internal static class StateMachineUtils
    {
        public static State GetState(string name) => 
            StateCache.TryGet(name, out Type type) ? (State) Activator.CreateInstance(type) : new NullState();
    }
}