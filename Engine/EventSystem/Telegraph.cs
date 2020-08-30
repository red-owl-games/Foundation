using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace RedOwl.Core
{
    public enum TelegraphStyles
    {
        Normal,
        Once,
        Permanent
    }
    
    public interface IReceiver { };
    
    public interface IReceiver<in T> : IReceiver
    {
        void OnEvent( T evt );
    }
    
    public interface ISignal {}
    
    [Serializable]
    public struct Signal : ISignal
    {
        public readonly string Name;
        
        public Signal(string name)
        {
            Name = name;
        }
    }
    
    public class SignalCache : TypeCache<ISignal>
    {
        protected override bool ShouldCache(Type type)
        {
            return typeof(ISignal).IsAssignableFrom(type);
        }
    }
    
    public class Cable : Dictionary<Type, List<IReceiver>> {}

    public static class Telegraph
    {
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
        
        private static bool SubscriptionExists(Type type, IReceiver receiver, Cable cables)
        {
            return cables.TryGetValue(type, out var receivers) && receivers.Any(t => t == receiver);
        }

        private static void Subscribe<T>(IReceiver<T> listener, Cable cables)
        {
            Type eventType = typeof(T);

            if( !cables.ContainsKey(eventType))
                cables[eventType] = new List<IReceiver>();

            if( !SubscriptionExists(eventType, listener, cables))
                cables[eventType].Add(listener);
        }

        [PublicAPI]
        public static void Subscribe<T>(IReceiver<T> listener, TelegraphStyles style = TelegraphStyles.Normal) where T : struct
        {
            switch (style)
            {
                case TelegraphStyles.Normal:
                    Subscribe(listener, Cables);
                    break;
                case TelegraphStyles.Once:
                    Subscribe(listener, CablesOnce);
                    break;
                case TelegraphStyles.Permanent:
                    Subscribe(listener, CablesPermanent);
                    break;
            }
        }

        private static void Send<T>(T evt, Cable cables, bool clear = false) where T : struct
        {
            Type eventType = typeof(T);

            if (!cables.TryGetValue(eventType, out var receivers)) return;
            foreach (var receiver in receivers)
            {
                ((IReceiver<T>) receiver)?.OnEvent(evt);
            }
            if (clear) receivers.Clear();
        }

        [PublicAPI]
        public static void Send<T>(T evt) where T : struct
        {
            Send(evt, CablesPermanent);
            Send(evt, Cables);
            Send(evt, CablesOnce, true);
        }
        
        private static void Unsubscribe<T>(IReceiver<T> listener, Cable cables)
        {
            Type eventType = typeof(T);

            if (!Cables.TryGetValue(eventType, out var receivers)) return;
            for (int i = receivers.Count - 1; i >= 0; i--)
            {
                if (receivers[i] == listener) receivers.RemoveAt(i);
            }
        }
        
        [PublicAPI]
        public static void Unsubscribe<T>(IReceiver<T> listener) where T : struct
        {
            Unsubscribe(listener, Cables);
            Unsubscribe(listener, CablesOnce);
            Unsubscribe(listener, CablesPermanent);
        }
    }
}