using System;
using System.Collections.Generic;

namespace RedOwl.Engine
{
    public struct ManagedObjectRef<T> where T : class
    {
        public readonly int Id;

        public ManagedObjectRef(int id)
        {
            Id = id;
        }

        public static implicit operator int(ManagedObjectRef<T> self)
        {
            return self.Id;
        }
    }
    
    public class ManagedObjectWorld
    {
        private int _nextId;
        private readonly Dictionary<int, object> _objects;
 
        public ManagedObjectWorld(int initialCapacity = 1000)
        {
            _nextId = 1;
            _objects = new Dictionary<int, object>(initialCapacity);
        }
 
        public ManagedObjectRef<T> Add<T>(T obj)
            where T : class
        {
            int id = _nextId;
            _nextId++;
            _objects[id] = obj;
            return new ManagedObjectRef<T>(id);
        }
 
        public T Get<T>(ManagedObjectRef<T> objRef)
            where T : class
        {
            return (T)_objects[objRef.Id];
        }
 
        public void Remove<T>(ManagedObjectRef<T> objRef)
            where T : class
        {
            _objects.Remove(objRef.Id);
        }
    }
}