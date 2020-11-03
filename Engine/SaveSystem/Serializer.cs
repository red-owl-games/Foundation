using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RedOwl.Engine
{
    public class SerializerCache
    {
        private Dictionary<Type, Serializer> _cache;

        public Serializer Get<T>()
        {
            ShouldBuildCache();
            if (!_cache.TryGetValue(typeof(T), out var s))
            {
                Log.Warn($"Unable to find Save Serializer for {typeof(T)} - Using NullSerializer");
                s = new NullSerializer();
            }
            return s;
        }
        
        public void ShouldBuildCache()
        {
            if (_cache == null) BuildCache();
        }

        private void BuildCache()
        {
            _cache = new Dictionary<Type, Serializer>();
            foreach (var type in TypeExtensions.GetAllTypes<Serializer>())
            {
                if (type == typeof(NullSerializer)) continue;
                //Log.Always($"Registering Serializer - {type.FullName}");
                var serializer = (Serializer) Activator.CreateInstance(type);
                _cache.Add(serializer.For, serializer);
            }
        }
    }

    public interface ISerializer<T>
    {
        void Write(BinaryWriter writer, T value);
        T Read(BinaryReader reader);
    }

    public abstract class Serializer
    {
        public static readonly SerializerCache Cache = new SerializerCache();
        
        public abstract Type For { get; }
    }

    public abstract class Serializer<T> : Serializer, ISerializer<T>
    {
        public override Type For => typeof(T);
        
        private readonly Action<BinaryWriter, T> _write;
        private readonly Func<BinaryReader, T> _read;

        protected Serializer(Action<BinaryWriter, T> write, Func<BinaryReader, T> read)
        {
            _write = write;
            _read = read;
        }
        
        public void Write(BinaryWriter writer, T value) => _write(writer, value);
        public T Read(BinaryReader reader) => _read(reader);
    }
    
    public abstract class SerializerArray<T> : Serializer, ISerializer<T[]>
    {
        public override Type For => typeof(T[]);
        
        private readonly Action<BinaryWriter, T> _write;
        private readonly Func<BinaryReader, T> _read;

        protected SerializerArray(Action<BinaryWriter, T> write, Func<BinaryReader, T> read)
        {
            _write = write;
            _read = read;
        }
        
        public void Write(BinaryWriter writer, T[] value)
        {
            int count = value.Length;
            writer.Write(count);
            for (int i = 0; i < count; i++)
            {
                _write(writer, value[i]);
            }
        }

        public T[] Read(BinaryReader reader)
        {
            int count = reader.ReadInt32();
            var output = new T[count];
            for (int i = 0; i < count; i++)
            {
                output[i] = _read(reader);
            }

            return output;
        }
    }
    
    public abstract class SerializerList<T> : Serializer, ISerializer<IList<T>>
    {
        public override Type For => typeof(IList<T>);
        
        private readonly Action<BinaryWriter, T> _write;
        private readonly Func<BinaryReader, T> _read;

        protected SerializerList(Action<BinaryWriter, T> write, Func<BinaryReader, T> read)
        {
            _write = write;
            _read = read;
        }
        
        public void Write(BinaryWriter writer, IList<T> value)
        {
            int count = value.Count;
            writer.Write(count);
            for (int i = 0; i < count; i++)
            {
                _write(writer, value[i]);
            }
        }

        public IList<T> Read(BinaryReader reader)
        {
            int count = reader.ReadInt32();
            var output = new T[count];
            for (int i = 0; i < count; i++)
            {
                output[i] = _read(reader);
            }

            return output;
        }
    }
    
    public class SaveReader : BinaryReader
    {
        public SaveReader(Stream stream, Encoding encoding, bool leaveOpen) : base(stream, encoding, leaveOpen) {}

        public T Read<T>() => ((ISerializer<T>)Serializer.Cache.Get<T>()).Read(this);
    }
    
    public class SaveWriter : BinaryWriter
    {
        public SaveWriter(Stream stream, Encoding encoding, bool leaveOpen) : base(stream, encoding, leaveOpen) {}

        public void Write<T>(T value) => ((ISerializer<T>)Serializer.Cache.Get<T>()).Write(this, value);
    }
}