using System.Collections.Generic;

namespace RedOwl.Engine
{
    public abstract class DatabaseBuilder<T, TBuilder, TData> : Asset<T> where T : DatabaseBuilder<T, TBuilder, TData> where TBuilder : Builder<TData>, new() where TData : new()
    {
        private TData[] _all;
        public TData[] All => _all ?? InternalConstruct();

        private Dictionary<int, TBuilder> _builders;
        
        private int MaxIndex()
        {
            int i = 0;
            foreach (int key in _builders.Keys)
            {
                if (key > i) i = key;
            }
            return i;
        }
        
        private int NextIndex()
        {
            int i = 0;
            while (_builders.ContainsKey(i)) i++;
            return i;
        }

        private TData[] InternalConstruct()
        {
            _builders = new Dictionary<int, TBuilder>();
            Construct();
            _all = new TData[MaxIndex() + 1];
            foreach (var kvp in _builders)
            {
                _all[kvp.Key] = kvp.Value;
            }
            _builders.Clear();
            return _all;
        }

        protected TBuilder Add(int index, TBuilder builder)
        {
            _builders.Add(index, builder);
            return builder;
        }

        protected TBuilder Add(int index) => Add(index, new TBuilder());
        
        protected TBuilder Add(TBuilder builder)
        {
            _builders.Add(NextIndex(), builder);
            return builder;
        }

        protected TBuilder Add() => Add(new TBuilder());

        protected abstract void Construct();
    }
}