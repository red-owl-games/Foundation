using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace RedOwl.Core
{
    public enum TelegraphStyles
    {
        Normal,
        Once,
        Permanent
    }

    public interface ISignal {}
    
    public static class Telegraph
    {
        public class Cable : Dictionary<string, List<Delegate>> {}
        
        public class SignalCache : TypeCache<ISignal>
        {
            protected override bool ShouldCache(Type type)
            {
                return typeof(ISignal).IsAssignableFrom(type);
            }
        }

        public static readonly SignalCache Signals = new SignalCache();

        private static readonly Cable Cables = new Cable();
        private static readonly Cable CablesOnce = new Cable();
        private static readonly Cable CablesPermanent = new Cable();
        
        
        
        [PublicAPI]
        public static void Clear()
        {
            Cables.Clear();
            CablesOnce.Clear();
        }
        
        private static void Subscribe(string key, Delegate listener, Cable cables)
        {
            if (!cables.TryGetValue(key, out var listeners))
            {
                listeners = new List<Delegate>();
                cables.Add(key, listeners);
            }
            listeners.Add(listener);
        }
        
        [PublicAPI]
        public static void Subscribe(string key, Delegate listener, TelegraphStyles style = TelegraphStyles.Normal)
        {
            switch (style)
            {
                case TelegraphStyles.Normal:
                    Subscribe(key, listener, Cables);
                    break;
                case TelegraphStyles.Once:
                    Subscribe(key, listener, CablesOnce);
                    break;
                case TelegraphStyles.Permanent:
                    Subscribe(key, listener, CablesPermanent);
                    break;
            }
        }
        
        [PublicAPI]
        public static void Subscribe<T>(Action<T> listener, TelegraphStyles style = TelegraphStyles.Normal) where T : ISignal =>
            Subscribe(typeof(T).SafeGetName(), listener);


        private static void Unsubscribe(string key, Delegate listener, Cable cables)
        {
            if (!cables.TryGetValue(key, out List<Delegate> listeners)) return;
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                if (listeners[i] == listener)
                {
                    listeners.RemoveAt(i);
                }
            }
        }

        [PublicAPI]
        public static void Unsubscribe(string key, Delegate listener)
        {
            Unsubscribe(key, listener, Cables);
            Unsubscribe(key, listener, CablesOnce);
            Unsubscribe(key, listener, CablesPermanent);
        }
        [PublicAPI]
        public static void Unsubscribe<T>(Action<T> listener) where T : ISignal =>
            Unsubscribe(typeof(T).SafeGetName(), listener);

        private static void Send<T>(string key, T payload, Cable cables, bool clear = false)
        {
            if (!cables.TryGetValue(key, out var listeners)) return;
            foreach (var listener in listeners)
            {
                switch (listener)
                {
                    case Action a:
                        a.Invoke();
                        break;
                    case Action<T> a1:
                        a1.Invoke(payload);
                        break;
                    case Delegate d:
                        d.DynamicInvoke(payload);
                        break;
                }
            }
            if (clear) listeners.Clear();
        }

        [PublicAPI]
        public static void Send(string key)
        {
            var payload = GetDefault(key);
            Send(key, payload, CablesPermanent);
            Send(key, payload, Cables);
            Send(key, payload, CablesOnce, true);
        }

        public static ISignal GetDefault(string key)
        {
            if (!Signals.Get(key, out Type type)) return null;
            return (ISignal) Activator.CreateInstance(type);
        }

        [PublicAPI]
        public static void Send<T>() where T : ISignal, new() => Send(new T());
        [PublicAPI]
        public static void Send<T>(string key, T payload) where T : ISignal
        {
            Send(key, payload, CablesPermanent);
            Send(key, payload, Cables);
            Send(key, payload, CablesOnce, true);
        } 
        
        [PublicAPI]
        public static void Send<T>(T payload) where T : ISignal
        {
            string key = typeof(T).SafeGetName();

            Send(key, payload, CablesPermanent);
            Send(key, payload, Cables);
            Send(key, payload, CablesOnce, true);
        }
    }
}