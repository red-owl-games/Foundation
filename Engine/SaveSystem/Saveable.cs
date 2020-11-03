using System;

namespace RedOwl.Engine
{
    public enum SaveableTypes
    {
        Data,
        Meta,
        Pref
    }
    
    public interface ISaveable
    {
        string Key { get; }
        SaveableTypes Type { get; }
        void Write(SaveWriter writer);
        void Read(SaveReader reader);
    }

    public class Saveable<T> : IDisposable, ISaveable
    {
        private readonly ulong _id;

        public string Key { get; }
        public SaveableTypes Type { get; }

        public T Value;

        public Saveable(string key, SaveableTypes type = SaveableTypes.Data, T defaultValue = default)
        {
            Key = key;
            Value = defaultValue;
            Type = type;
            _id = Game.SaveSystem.Subscribe(this);
        }
        
        public static implicit operator T(Saveable<T> self) => self.Value;

        public void Dispose()
        {
            Game.SaveSystem.Unsubscribe(_id);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
        
        public void Write(SaveWriter writer) => writer.Write(Value);
        public void Read(SaveReader reader) => Value = reader.Read<T>();
    }
}