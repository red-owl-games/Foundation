using System;

namespace RedOwl.Engine
{
    public interface ISaveable
    {
        void Save();
        void Load();
    }
    
    public enum SaveableTypes
    {
        Data,
        Meta
    }
    
    public class Saveable<T> : IDisposable, ISaveable
    {
        private readonly SaveableTypes _type;
        private readonly string _key;
        private readonly T _defaultValue;
        private readonly ulong _id;

        private bool _initialized;
        private T _value;
        public T Value
        {
            get
            {
                if (_initialized) return _value;
                Load();
                _initialized = true;
                return _value;
            }
            set
            {
                _value = value;
                Save();
            }
        }

        public Saveable(string key, SaveableTypes type = SaveableTypes.Data, T defaultValue = default)
        {
            _type = type;
            _key = key;
            _defaultValue = defaultValue;
            _id = Game.Subscribe(this);
        }
        
        public static implicit operator T(Saveable<T> self) => self.Value;

        public void Dispose()
        {
            Save();
            Game.Unsubscribe(_id);
        }

        public void Save()
        {
            switch (_type)
            {
                case SaveableTypes.Data:
                    Game.SaveData.Set(_key, _value);
                    break;
                case SaveableTypes.Meta:
                    Game.SaveMeta.Set(_key, _value);
                    break;
            }
        }

        public void Load()
        {
            switch (_type)
            {
                case SaveableTypes.Data:
                    _value = Game.SaveData.Get(_key, _defaultValue);
                    break;
                case SaveableTypes.Meta:
                    _value = Game.SaveMeta.Get(_key, _defaultValue);
                    break;
            }
        }
    }
}