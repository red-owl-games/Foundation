using System;
using System.Collections.Generic;

namespace RedOwl.Core
{
    public enum BetterStackTypes
    {
        Fifo,
        Lifo,
    }
    
    public class BetterStack<T> : LinkedList<T>
    {
        private readonly BetterStackTypes _type;

        public BetterStack(BetterStackTypes type = BetterStackTypes.Lifo) : base()
        {
            _type = type;
        }
        
        public T Peek() => _type == BetterStackTypes.Lifo ? (First == null ? default : First.Value) : (Last == null ? default : Last.Value);

        public void Push(T item) => AddFirst(item);
        
        public T Pop()
        {
            T value;
            switch (_type)
            {
                case BetterStackTypes.Fifo:
                    value = Last.Value;
                    RemoveLast();
                    return value;
                case BetterStackTypes.Lifo:
                    value = First.Value;
                    RemoveFirst();
                    return value;
            }
            return default;
        }
    }
}