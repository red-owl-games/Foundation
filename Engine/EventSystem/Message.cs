using System;
using Sirenix.OdinInspector;

namespace RedOwl.Core
{
    [HideReferenceObjectPicker, InlineProperty, HideLabel]
    public class Message : ITelegram
    {
        private readonly string _key;
        
        public Message(string key) => _key = key;

        [Button("@_key")]
        public void Raise() => Telegraph.Send(_key);

        public void Subscribe(Action listener) => Telegraph.Subscribe(_key, listener);

        public void Unsubscribe(Action listener) => Telegraph.Subscribe(_key, listener);
    }

    public interface ITelegram<T1>
    {
        void Raise(T1 t1);
        void Subscribe(Action<T1> listener);
        void Unsubscribe(Action<T1> listener); 
    }

    [HideReferenceObjectPicker, InlineProperty, HideLabel]
    public class Message<T1> : ITelegram<T1>
    {
        public struct Signal : ISignal
        {
            public T1 value1;
        }
        
        private readonly string _key;
        
        public Message(string key) => _key = key;

        [Button("@_key")]
        public void Raise(T1 t1) => Telegraph.Send(_key, new Signal{ value1 = t1 });

        public void Subscribe(Action<T1> listener) => Telegraph.Subscribe(_key, listener);

        public void Unsubscribe(Action<T1> listener) => Telegraph.Subscribe(_key, listener);
    }
    
    public interface ITelegram<T1, T2>
    {
        void Raise(T1 t1, T2 t2);
        void Subscribe(Action<T1, T2> listener);
        void Unsubscribe(Action<T1, T2> listener); 
    }

    [HideReferenceObjectPicker, InlineProperty, HideLabel]
    public class Message<T1, T2> : ITelegram<T1, T2>
    {
        public struct Signal : ISignal
        {
            public T1 value1;
            public T2 value2;
        }
        
        private readonly string _key;
        
        public Message(string key) => _key = key;

        [Button("@_key")]
        public void Raise(T1 t1, T2 t2) => Telegraph.Send(_key, new Signal{ value1 = t1, value2 = t2 });

        public void Subscribe(Action<T1, T2> listener) => Telegraph.Subscribe(_key, listener);

        public void Unsubscribe(Action<T1, T2> listener) => Telegraph.Subscribe(_key, listener);
    }
}