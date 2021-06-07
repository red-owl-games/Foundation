using System;
using System.Collections;
using System.Collections.Generic;

namespace RedOwl.Engine
{
    public interface IMessageBase : ITelegram
    {
        event Action On;
        void Raise();
        void RaiseAsync();
    }

    public interface IMessageBase<T1> : ITelegram
    {
        event Action<T1> On;
        void Raise(T1 t1);
        void RaiseAsync(T1 t1);
    }
    
    public interface IMessageBase<T1, T2> : ITelegram
    {
        event Action<T1, T2> On;
        void Raise(T1 t1, T2 t2);
        void RaiseAsync(T1 t1, T2 t2);
    }
    
    public interface IMessageBase<T1, T2, T3> : ITelegram
    {
        event Action<T1, T2, T3> On;
        void Raise(T1 t1, T2 t2, T3 t3);
        void RaiseAsync(T1 t1, T2 t2, T3 t3);
    }
    
    public interface IMessageBase<T1, T2, T3, T4> : ITelegram
    {
        event Action<T1, T2, T3, T4> On;
        void Raise(T1 t1, T2 t2, T3 t3, T4 t4);
        void RaiseAsync(T1 t1, T2 t2, T3 t3, T4 t4);
    }
    
    public abstract class MessageBase : IMessageBase
    {
        // TODO: should these actually reside on Telegraph and MessageBase is just a shim to talk to Telegraph?
        private readonly List<Action> _listeners = new List<Action>();

        public event Action On
        {
            add => _listeners.Add(value);
            remove => _listeners.Remove(value);
        }
        
        public void Raise()
        {
            var count = _listeners.Count;
            for (int i = 0; i < count; i++)
            {
                _listeners[i].Invoke();
            }
        }

        public void RaiseAsync()
        {
            Game.StartRoutine(RaiseAsyncInternal());
        }

        private IEnumerator RaiseAsyncInternal()
        {
            var count = _listeners.Count;
            for (int i = 0; i < count; i++)
            {
                _listeners[i].Invoke();
                yield return null;
            }
        }
    }

    public abstract class MessageBase<T1> : IMessageBase<T1>
    {
        private readonly List<Action<T1>> _listeners = new List<Action<T1>>();
        
        public event Action<T1> On
        {
            add => _listeners.Add(value);
            remove => _listeners.Remove(value);
        }
        
        public void Raise(T1 t1)
        {
            var count = _listeners.Count;
            for (int i = 0; i < count; i++)
            {
                _listeners[i].Invoke(t1);
            }
        }

        public void RaiseAsync(T1 t1)
        {
            Game.StartRoutine(RaiseAsyncInternal(t1));
        }

        private IEnumerator RaiseAsyncInternal(T1 t1)
        {
            var count = _listeners.Count;
            for (int i = 0; i < count; i++)
            {
                _listeners[i].Invoke(t1);
                yield return null;
            }
        } 
    }
    
    public abstract class MessageBase<T1, T2> : IMessageBase<T1, T2>
    {
        private readonly List<Action<T1, T2>> _listeners = new List<Action<T1, T2>>();
        
        public event Action<T1, T2> On
        {
            add => _listeners.Add(value);
            remove => _listeners.Remove(value);
        }
        
        public void Raise(T1 t1, T2 t2)
        {
            var count = _listeners.Count;
            for (int i = 0; i < count; i++)
            {
                _listeners[i].Invoke(t1, t2);
            }
        }

        public void RaiseAsync(T1 t1, T2 t2)
        {
            Game.StartRoutine(RaiseAsyncInternal(t1, t2));
        }

        private IEnumerator RaiseAsyncInternal(T1 t1, T2 t2)
        {
            var count = _listeners.Count;
            for (int i = 0; i < count; i++)
            {
                _listeners[i].Invoke(t1, t2);
                yield return null;
            }
        } 
    }
    
    public abstract class MessageBase<T1, T2, T3> : IMessageBase<T1, T2, T3>
    {
        private readonly List<Action<T1, T2, T3>> _listeners = new List<Action<T1, T2, T3>>();
        
        public event Action<T1, T2, T3> On
        {
            add => _listeners.Add(value);
            remove => _listeners.Remove(value);
        }
        
        public void Raise(T1 t1, T2 t2, T3 t3)
        {
            var count = _listeners.Count;
            for (int i = 0; i < count; i++)
            {
                _listeners[i].Invoke(t1, t2, t3);
            }
        }

        public void RaiseAsync(T1 t1, T2 t2, T3 t3)
        {
            Game.StartRoutine(RaiseAsyncInternal(t1, t2, t3));
        }

        private IEnumerator RaiseAsyncInternal(T1 t1, T2 t2, T3 t3)
        {
            var count = _listeners.Count;
            for (int i = 0; i < count; i++)
            {
                _listeners[i].Invoke(t1, t2, t3);
                yield return null;
            }
        } 
    }
    
    public abstract class MessageBase<T1, T2, T3, T4> : IMessageBase<T1, T2, T3, T4>
    {
        private readonly List<Action<T1, T2, T3, T4>> _listeners = new List<Action<T1, T2, T3, T4>>();
        
        public event Action<T1, T2, T3, T4> On
        {
            add => _listeners.Add(value);
            remove => _listeners.Remove(value);
        }
        
        public void Raise(T1 t1, T2 t2, T3 t3, T4 t4)
        {
            var count = _listeners.Count;
            for (int i = 0; i < count; i++)
            {
                _listeners[i].Invoke(t1, t2, t3, t4);
            }
        }

        public void RaiseAsync(T1 t1, T2 t2, T3 t3, T4 t4)
        {
            Game.StartRoutine(RaiseAsyncInternal(t1, t2, t3, t4));
        }

        private IEnumerator RaiseAsyncInternal(T1 t1, T2 t2, T3 t3, T4 t4)
        {
            var count = _listeners.Count;
            for (int i = 0; i < count; i++)
            {
                _listeners[i].Invoke(t1, t2, t3, t4);
                yield return null;
            }
        } 
    }
    
    public class Message : MessageBase {}
}