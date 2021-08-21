using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Assertions;

namespace RedOwl.Engine
{
    public interface IRedOwlSerialization
    {
        void Serialize(RedOwlSerializer s);
    }

    public class RedOwlSerializer : IDisposable
    {      
        public enum Formats
        {
            Binary,
            Json
        }
        
        public int DataVersion { get; }
        public bool IsWriting { get; }
        
        public int Counter { get; private set; }
        public Stream Stream { get; private set; }
        public IDataWriter Writer { get; private set; }
        public IDataReader Reader { get; private set;  }

        public RedOwlSerializer(Stream stream, int latestVersion = 0, Formats format = Formats.Binary, bool isWriting = false)
        {
            IsWriting = isWriting;
            Stream = stream;
            switch (format)
            {
                // TODO: eventually we should probably write our own of these so the json output can be more like "JsonUtility.ToJson"
                case Formats.Json:
                    Writer = new JsonDataWriter(stream, null);
                    Reader = new JsonDataReader(stream, null);
                    break;
                case Formats.Binary:
                    Writer = new BinaryDataWriter(stream, null);
                    Reader = new BinaryDataReader(stream, null);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(format), format, null);
            }

            if (IsWriting)
            {
                DataVersion = latestVersion - 1;
                Writer.Write(DataVersion);
            } else {
                DataVersion = Reader.ReadInt32();
                // We are reading a file from a version that came after this one!
                if (DataVersion > latestVersion)
                    throw new Exception($"Unable to serialize data from file version '{DataVersion}' because code version is '{latestVersion}'");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Action Serialize<T>(T item) where T : IRedOwlSerialization => () => item.Serialize(this);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Skip<T>()
        {
            if (!IsWriting)
            {
                SerializationUtility.DeserializeValue<T>(Reader);
            }
        }

        public void Flush()
        {
            Writer?.FlushToStream();
        }

        public void Dispose()
        {
            Writer?.Dispose();
            Reader?.Dispose();
            Stream?.Close();

            Writer = null;
            Reader = null;
            Stream = null;
        }
        
        #region VersionMethods
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Init(Action callback) => Add(0, callback);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(int added, Action callback)
        {
            if (DataVersion >= added) callback();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Remove(int added, int removed, Action callback)
        {
            if (DataVersion >= added && DataVersion < removed) callback();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Integrity(int check)
        {
            if (DataVersion >= check)
            {
                var current = Counter;
                this.Serialize(ref current);
                Assert.AreEqual(current, Counter++);
            }
        }
        
        #endregion
        
        #region EnumVersionMethods
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add<T>(T added, Action callback) where T : unmanaged, IConvertible =>
            Add(ConvertToInt(added), callback);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Remove<T>(T added, T removed, Action callback) where T : unmanaged, IConvertible =>
            Remove(ConvertToInt(added), ConvertToInt(removed), callback);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Integrity<T>(T check) where T : unmanaged, IConvertible =>
            Integrity(ConvertToInt(check));
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int ConvertToInt<T>(T e) where T : unmanaged, IConvertible =>
            e.ToInt32(null);
        
        #endregion
    }

    public static class SerializerExtensions
    {
        #region ReaderWriterExtensions
        
        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // public static Vector2 ReadVector2(this BinaryReader reader) => 
        //     new Vector2(reader.ReadSingle(), reader.ReadSingle());
        //
        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // public static Vector2Int ReadVector2Int(this BinaryReader reader) => 
        //     new Vector2Int(reader.ReadInt32(), reader.ReadInt32());
        //
        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // public static Vector3 ReadVector3(this BinaryReader reader) => 
        //     new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        //
        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // public static Vector3Int ReadVector3Int(this BinaryReader reader) => 
        //     new Vector3Int(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
        //
        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // public static Vector4 ReadVector4(this BinaryReader reader) => 
        //     new Vector4(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        //
        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // public static Quaternion ReadQuaternion(this BinaryReader reader) => 
        //     new Quaternion(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        //
        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // public static Color ReadColor(this BinaryReader reader) => 
        //     new Color(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        //
        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // public static Color32 ReadColor32(this BinaryReader reader) => 
        //     new Color32(reader.ReadByte(), reader.ReadByte(), reader.ReadByte(), reader.ReadByte());
        //
        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // public static void Write(this BinaryWriter writer, Vector2 value)
        // {
        //     writer.Write(value.x);
        //     writer.Write(value.y);
        // }
        //
        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // public static void Write(this BinaryWriter writer, Vector2Int value)
        // {
        //     writer.Write(value.x);
        //     writer.Write(value.y);
        // }
        //
        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // public static void Write(this BinaryWriter writer, Vector3 value)
        // {
        //     writer.Write(value.x);
        //     writer.Write(value.y);
        //     writer.Write(value.z);
        // }
        //
        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // public static void Write(this BinaryWriter writer, Vector3Int value)
        // {
        //     writer.Write(value.x);
        //     writer.Write(value.y);
        //     writer.Write(value.z);
        // }
        //
        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // public static void Write(this BinaryWriter writer, Vector4 value)
        // {
        //     writer.Write(value.x);
        //     writer.Write(value.y);
        //     writer.Write(value.z);
        //     writer.Write(value.w);
        // }
        //
        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // public static void Write(this BinaryWriter writer, Quaternion value)
        // {
        //     writer.Write(value.x);
        //     writer.Write(value.y);
        //     writer.Write(value.z);
        //     writer.Write(value.w);
        // }
        //
        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // public static void Write(this BinaryWriter writer, Color value)
        // {
        //     writer.Write(value.r);
        //     writer.Write(value.g);
        //     writer.Write(value.b);
        //     writer.Write(value.a);
        // }
        //
        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // public static void Write(this BinaryWriter writer, Color32 value)
        // {
        //     writer.Write(value.r);
        //     writer.Write(value.g);
        //     writer.Write(value.b);
        //     writer.Write(value.a);
        // }
        
        public static void Write(this IDataWriter self, byte value) => self.WriteByte(null, value);
        public static void Write(this IDataWriter self, sbyte value) => self.WriteSByte(null, value);
        public static void Write(this IDataWriter self, bool value) => self.WriteBoolean(null, value);
        public static void Write(this IDataWriter self, float value) => self.WriteSingle(null, value);
        public static void Write(this IDataWriter self, double value) => self.WriteDouble(null, value);
        public static void Write(this IDataWriter self, decimal value) => self.WriteDecimal(null, value);
        public static void Write(this IDataWriter self, short value) => self.WriteInt16(null, value);
        public static void Write(this IDataWriter self, ushort value) => self.WriteUInt16(null, value);
        public static void Write(this IDataWriter self, int value) => self.WriteInt32(null, value);
        public static void Write(this IDataWriter self, uint value) => self.WriteUInt32(null, value);
        public static void Write(this IDataWriter self, long value) => self.WriteInt64(null, value);
        public static void Write(this IDataWriter self, ulong value) => self.WriteUInt64(null, value);
        public static void Write(this IDataWriter self, char value) => self.WriteChar(null, value);
        public static void Write(this IDataWriter self, string value) => self.WriteString(null, value);
        public static void Write(this IDataWriter self, Vector2 value)
        {
            self.WriteSingle(null, value.x);
            self.WriteSingle(null, value.y);
        }
        
        public static void Write(this IDataWriter self, Vector2Int value)
        {
            self.WriteInt32(null, value.x);
            self.WriteInt32(null, value.y);
        }
        
        public static void Write(this IDataWriter self, Vector3 value)
        {
            self.WriteSingle(null, value.x);
            self.WriteSingle(null, value.y);
            self.WriteSingle(null, value.z);
        }
        
        public static void Write(this IDataWriter self, Vector3Int value)
        {
            self.WriteInt32(null, value.x);
            self.WriteInt32(null, value.y);
            self.WriteInt32(null, value.z);
        }
        
        public static void Write(this IDataWriter self, Vector4 value)
        {
            self.WriteSingle(null, value.x);
            self.WriteSingle(null, value.y);
            self.WriteSingle(null, value.z);
            self.WriteSingle(null, value.w);
        }

        public static void Write(this IDataWriter self, Quaternion value)
        {
            self.WriteSingle(null, value.x);
            self.WriteSingle(null, value.y);
            self.WriteSingle(null, value.z);
            self.WriteSingle(null, value.w);
        }
        
        public static void Write(this IDataWriter self, Color value)
        {
            self.WriteSingle(null, value.r);
            self.WriteSingle(null, value.g);
            self.WriteSingle(null, value.b);
            self.WriteSingle(null, value.a);
        }
        
        public static void Write(this IDataWriter self, Color32 value)
        {
            self.WriteByte(null, value.r);
            self.WriteByte(null, value.g);
            self.WriteByte(null, value.b);
            self.WriteByte(null, value.a);
        }

        public static byte ReadByte(this IDataReader self) => self.ReadByte(out byte v) ? v : default;
        public static sbyte ReadSByte(this IDataReader self) => self.ReadSByte(out sbyte v) ? v : default;
        public static bool ReadBoolean(this IDataReader self) => self.ReadBoolean(out bool v) ? v : default;
        public static float ReadSingle(this IDataReader self) => self.ReadSingle(out float v) ? v : default;
        public static double ReadDouble(this IDataReader self) => self.ReadDouble(out double v) ? v : default;
        public static decimal ReadDecimal(this IDataReader self) => self.ReadDecimal(out decimal v) ? v : default;
        public static short ReadInt16(this IDataReader self) => self.ReadInt16(out short v) ? v : default;
        public static ushort ReadUInt16(this IDataReader self) => self.ReadUInt16(out ushort v) ? v : default;
        public static int ReadInt32(this IDataReader self) => self.ReadInt32(out int v) ? v : default;
        public static uint ReadUInt32(this IDataReader self) => self.ReadUInt32(out uint v) ? v : default;
        public static long ReadInt64(this IDataReader self) => self.ReadInt64(out long v) ? v : default;
        public static ulong ReadUInt64(this IDataReader self) => self.ReadUInt64(out ulong v) ? v : default;
        public static char ReadChar(this IDataReader self) => self.ReadChar(out char v) ? v : default;
        public static string ReadString(this IDataReader self) => self.ReadString(out string v) ? v : default;
        public static Vector2 ReadVector2(this IDataReader self) => new Vector2(self.ReadSingle(), self.ReadSingle());
        public static Vector2Int ReadVector2Int(this IDataReader self) => new Vector2Int(self.ReadInt32(), self.ReadInt32());
        public static Vector3 ReadVector3(this IDataReader self) => new Vector3(self.ReadSingle(), self.ReadSingle(), self.ReadSingle());
        public static Vector3Int ReadVector3Int(this IDataReader self) => new Vector3Int(self.ReadInt32(), self.ReadInt32(), self.ReadInt32());
        public static Vector4 ReadVector4(this IDataReader self) => new Vector4(self.ReadSingle(), self.ReadSingle(), self.ReadSingle(), self.ReadSingle());
        public static Quaternion ReadQuaternion(this IDataReader self) => new Quaternion(self.ReadSingle(), self.ReadSingle(), self.ReadSingle(), self.ReadSingle());

        public static Color ReadColor(this IDataReader self) => new Color(self.ReadSingle(), self.ReadSingle(), self.ReadSingle(), self.ReadSingle());

        public static Color32 ReadColor32(this IDataReader self) => new Color32(self.ReadByte(), self.ReadByte(), self.ReadByte(), self.ReadByte());

        #endregion

        #region Utility

        private static void WriteBytes(this RedOwlSerializer serializer, byte[] value)
        {
            serializer.Writer.Write(value.Length);
            foreach (byte b in value)
            {
                serializer.Writer.WriteByte(null, b);
            }
        }
        
        private static byte[] ReadBytes(this RedOwlSerializer serializer)
        {
            
            int count = serializer.Reader.ReadInt32();
            byte[] output = new byte[count];
            for (int i = 0; i < count; i++)
            {
                output[i] = serializer.Reader.ReadByte();
            }
            return output;
        }
        
        public static RedOwlSerializer Serialize<T>(this RedOwlSerializer s, ref T value, Action<T> writer, Func<T> reader)
        {
            if (s.IsWriting) {
                writer(value);
            } else {
                value = reader();
            }
            return s;
        }
        
        public static void SerializeArray<T>(this RedOwlSerializer serializer,
            ref T[] array,
            Action<T> write,
            Func<T> read)
        {
            int count;
            if (serializer.IsWriting)
            {
                count = array.Length;
                serializer.Writer.Write(count);
                for (int i = 0; i < count; i++)
                {
                    write(array[i]);
                }
                return;
            }

            count = serializer.Reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                array[i] = read();
            }
        }
        
        public static void SerializeCollection<T>(this RedOwlSerializer serializer,
            ref ICollection<T> collection,
            Action<T> write,
            Func<T> read)
        {
            int count;
            if (serializer.IsWriting)
            {
                count = collection.Count;
                serializer.Writer.Write(count);
                foreach (var item in collection)
                {
                    write(item);
                }
                return;
            }

            count = serializer.Reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                collection.Add(read());
            }
        }

        public static RedOwlSerializer SerializeDictionary<TKey, TValue>(this RedOwlSerializer serializer,
            ref IDictionary<TKey, TValue> dictionary,
            Action<TKey> writeKey, Action<TValue> writeValue,
            Func<TKey> readKey, Func<TValue> readValue)
        {
            int count;
            if (serializer.IsWriting)
            {
                count = dictionary.Count;
                serializer.Writer.Write(count);
                foreach (var item in dictionary)
                {
                    writeKey(item.Key);
                    writeValue(item.Value);
                }
            }
            else
            {
                count = serializer.Reader.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    dictionary[readKey()] = readValue();
                }
            }

            return serializer;
        }

        #endregion
        
        #region BaseTypes

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Serialize(this RedOwlSerializer s, string name, ref byte value)
        {
            if (s.IsWriting) {
                s.Writer.WriteByte(name, value);
            } else {
                value = s.Reader.ReadByte(out byte v) ? v : default;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RedOwlSerializer Serialize(this RedOwlSerializer s, ref byte[] value) => 
            s.Serialize(ref value, s.WriteBytes, s.ReadBytes);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RedOwlSerializer Serialize(this RedOwlSerializer s, ref sbyte value) => 
            s.Serialize(ref value, s.Writer.Write, s.Reader.ReadSByte);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Serialize(this RedOwlSerializer s, ref sbyte[] value) => 
            s.SerializeArray(ref value, s.Writer.Write, s.Reader.ReadSByte);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Serialize(this RedOwlSerializer s, ref ICollection<sbyte> value) => 
            s.SerializeCollection(ref value, s.Writer.Write, s.Reader.ReadSByte);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RedOwlSerializer Serialize(this RedOwlSerializer s, ref bool value) => 
            s.Serialize(ref value, s.Writer.Write, s.Reader.ReadBoolean);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Serialize(this RedOwlSerializer s, ref bool[] value) => 
            s.SerializeArray(ref value, s.Writer.Write, s.Reader.ReadBoolean);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Serialize(this RedOwlSerializer s, ref ICollection<bool> value) => 
            s.SerializeCollection(ref value, s.Writer.Write, s.Reader.ReadBoolean);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RedOwlSerializer Serialize(this RedOwlSerializer s, ref float value) => 
            s.Serialize(ref value, s.Writer.Write, s.Reader.ReadSingle);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Serialize(this RedOwlSerializer s, ref float[] value) => 
            s.SerializeArray(ref value, s.Writer.Write, s.Reader.ReadSingle);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Serialize(this RedOwlSerializer s, ref ICollection<float> value) => 
            s.SerializeCollection(ref value, s.Writer.Write, s.Reader.ReadSingle);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RedOwlSerializer Serialize(this RedOwlSerializer s, ref double value) => 
            s.Serialize(ref value, s.Writer.Write, s.Reader.ReadDouble);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Serialize(this RedOwlSerializer s, ref double[] value) => 
            s.SerializeArray(ref value, s.Writer.Write, s.Reader.ReadDouble);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Serialize(this RedOwlSerializer s, ref ICollection<double> value) => 
            s.SerializeCollection(ref value, s.Writer.Write, s.Reader.ReadDouble);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RedOwlSerializer Serialize(this RedOwlSerializer s, ref decimal value) => 
            s.Serialize(ref value, s.Writer.Write, s.Reader.ReadDecimal);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Serialize(this RedOwlSerializer s, ref decimal[] value) => 
            s.SerializeArray(ref value, s.Writer.Write, s.Reader.ReadDecimal);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Serialize(this RedOwlSerializer s, ref ICollection<decimal> value) => 
            s.SerializeCollection(ref value, s.Writer.Write, s.Reader.ReadDecimal);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RedOwlSerializer Serialize(this RedOwlSerializer s, ref short value) => 
            s.Serialize(ref value, s.Writer.Write, s.Reader.ReadInt16);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Serialize(this RedOwlSerializer s, ref short[] value) => 
            s.SerializeArray(ref value, s.Writer.Write, s.Reader.ReadInt16);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Serialize(this RedOwlSerializer s, ref ICollection<short> value) => 
            s.SerializeCollection(ref value, s.Writer.Write, s.Reader.ReadInt16);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RedOwlSerializer Serialize(this RedOwlSerializer s, ref ushort value) => 
            s.Serialize(ref value, s.Writer.Write, s.Reader.ReadUInt16);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Serialize(this RedOwlSerializer s, ref ushort[] value) => 
            s.SerializeArray(ref value, s.Writer.Write, s.Reader.ReadUInt16);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Serialize(this RedOwlSerializer s, ref ICollection<ushort> value) => 
            s.SerializeCollection(ref value, s.Writer.Write, s.Reader.ReadUInt16);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Serialize(this RedOwlSerializer s, ref int value) => 
            s.Serialize(ref value, s.Writer.Write, s.Reader.ReadInt32);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Serialize(this RedOwlSerializer s, ref int[] value) => 
            s.SerializeArray(ref value, s.Writer.Write, s.Reader.ReadInt32);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Serialize(this RedOwlSerializer s, ref ICollection<int> value) => 
            s.SerializeCollection(ref value, s.Writer.Write, s.Reader.ReadInt32);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RedOwlSerializer Serialize(this RedOwlSerializer s, ref uint value) => 
            s.Serialize(ref value, s.Writer.Write, s.Reader.ReadUInt32);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Serialize(this RedOwlSerializer s, ref uint[] value) => 
            s.SerializeArray(ref value, s.Writer.Write, s.Reader.ReadUInt32);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Serialize(this RedOwlSerializer s, ref ICollection<uint> value) => 
            s.SerializeCollection(ref value, s.Writer.Write, s.Reader.ReadUInt32);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RedOwlSerializer Serialize(this RedOwlSerializer s, ref long value) => 
            s.Serialize(ref value, s.Writer.Write, s.Reader.ReadInt64);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Serialize(this RedOwlSerializer s, ref long[] value) => 
            s.SerializeArray(ref value, s.Writer.Write, s.Reader.ReadInt64);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Serialize(this RedOwlSerializer s, ref ICollection<long> value) => 
            s.SerializeCollection(ref value, s.Writer.Write, s.Reader.ReadInt64);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RedOwlSerializer Serialize(this RedOwlSerializer s, ref ulong value) => 
            s.Serialize(ref value, s.Writer.Write, s.Reader.ReadUInt64);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Serialize(this RedOwlSerializer s, ref ulong[] value) => 
            s.SerializeArray(ref value, s.Writer.Write, s.Reader.ReadUInt64);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Serialize(this RedOwlSerializer s, ref ICollection<ulong> value) => 
            s.SerializeCollection(ref value, s.Writer.Write, s.Reader.ReadUInt64);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RedOwlSerializer Serialize(this RedOwlSerializer s, ref char value) => 
            s.Serialize(ref value, s.Writer.Write, s.Reader.ReadChar);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Serialize(this RedOwlSerializer s, ref char[] value) => 
            s.SerializeArray(ref value, s.Writer.Write, s.Reader.ReadChar);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Serialize(this RedOwlSerializer s, ref ICollection<char> value) => 
            s.SerializeCollection(ref value, s.Writer.Write, s.Reader.ReadChar);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Serialize(this RedOwlSerializer s, string name, ref string value)
        {
            if (s.IsWriting) {
                s.Writer.WriteString(name, value);
            } else {
                value = s.Reader.ReadString(out string v) ? v : default;
            }
        }

        #endregion
        
        #region ExtendedTypes

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Serialize(this RedOwlSerializer s, ref Guid value) => 
            s.Serialize(ref value, (x) => s.Writer.Write(x.ToString()), () => Guid.Parse(s.Reader.ReadString()));
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RedOwlSerializer Serialize(this RedOwlSerializer s, ref IDictionary<string, byte[]> value) => 
            s.SerializeDictionary(ref value, s.Writer.Write, s.WriteBytes, s.Reader.ReadString, s.ReadBytes);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Serialize(this RedOwlSerializer s, ref IDictionary<string, string> value) => 
            s.SerializeDictionary(ref value, s.Writer.Write, s.Writer.Write, s.Reader.ReadString, s.Reader.ReadString);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Serialize(this RedOwlSerializer s, ref IDictionary<string, float> value) => 
            s.SerializeDictionary(ref value, s.Writer.Write, s.Writer.Write, s.Reader.ReadString, s.Reader.ReadSingle);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Serialize(this RedOwlSerializer s, ref IDictionary<string, int> value) => 
            s.SerializeDictionary(ref value, s.Writer.Write, s.Writer.Write, s.Reader.ReadString, s.Reader.ReadInt32);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Serialize(this RedOwlSerializer s, ref IDictionary<string, bool> value) => 
            s.SerializeDictionary(ref value, s.Writer.Write, s.Writer.Write, s.Reader.ReadString, s.Reader.ReadBoolean);

        #endregion
        
        #region UnityTypes
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Serialize(this RedOwlSerializer s, ref Color value) => 
            s.Serialize(ref value, s.Writer.Write, s.Reader.ReadColor);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Serialize(this RedOwlSerializer s, ref Color32 value) => 
            s.Serialize(ref value, s.Writer.Write, s.Reader.ReadColor32);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Serialize(this RedOwlSerializer s, ref Vector2 value) => 
            s.Serialize(ref value, s.Writer.Write, s.Reader.ReadVector2);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Serialize(this RedOwlSerializer s, ref Vector3 value) => 
            s.Serialize(ref value, s.Writer.Write, s.Reader.ReadVector3);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Serialize(this RedOwlSerializer s, ref Vector4 value) => 
            s.Serialize(ref value, s.Writer.Write, s.Reader.ReadVector4);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Serialize(this RedOwlSerializer s, ref Vector2Int value) => 
            s.Serialize(ref value, s.Writer.Write, s.Reader.ReadVector2Int);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Serialize(this RedOwlSerializer s, ref Vector3Int value) => 
            s.Serialize(ref value, s.Writer.Write, s.Reader.ReadVector3Int);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Serialize(this RedOwlSerializer s, ref Quaternion value) => 
            s.Serialize(ref value, s.Writer.Write, s.Reader.ReadQuaternion);
        
        public static void Serialize(this RedOwlSerializer s, ref Plane value)
        {
            if (s.IsWriting)
            {
                s.Writer.Write(value.normal);
                s.Writer.Write(value.distance);
                return;
            }
            
            value = new Plane(s.Reader.ReadVector3(), s.Reader.ReadSingle());
        }

        public static void Serialize(this RedOwlSerializer s, ref Bounds value)
        {
            if (s.IsWriting)
            {
                s.Writer.Write(value.center);
                s.Writer.Write(value.size);
                return;
            }
            
            value = new Bounds(s.Reader.ReadVector3(), s.Reader.ReadVector3());
        }
        
        public static void Serialize(this RedOwlSerializer s, ref BoundsInt value)
        {
            if (s.IsWriting)
            {
                s.Writer.Write(value.position);
                s.Writer.Write(value.size);
                return;
            }
            
            value = new BoundsInt(s.Reader.ReadVector3Int(), s.Reader.ReadVector3Int());
        }
        
        public static void Serialize(this RedOwlSerializer s, ref Rect value)
        {
            if (s.IsWriting)
            {
                s.Writer.Write(value.x);
                s.Writer.Write(value.y);
                s.Writer.Write(value.width);
                s.Writer.Write(value.height);
                return;
            }

            value = new Rect(s.Reader.ReadSingle(), s.Reader.ReadSingle(), s.Reader.ReadSingle(), s.Reader.ReadSingle());
        }
        
        public static void Serialize(this RedOwlSerializer s, ref RectInt value)
        {
            if (s.IsWriting)
            {
                s.Writer.Write(value.x);
                s.Writer.Write(value.y);
                s.Writer.Write(value.width);
                s.Writer.Write(value.height);
                return;
            }

            value = new RectInt(s.Reader.ReadInt32(), s.Reader.ReadInt32(), s.Reader.ReadInt32(), s.Reader.ReadInt32());
        }
        
        public static void Serialize(this RedOwlSerializer s, ref RectOffset value)
        {
            if (s.IsWriting)
            {
                s.Writer.Write(value.left);
                s.Writer.Write(value.right);
                s.Writer.Write(value.top);
                s.Writer.Write(value.bottom);
                return;
            }

            value = new RectOffset(s.Reader.ReadInt32(), s.Reader.ReadInt32(), s.Reader.ReadInt32(), s.Reader.ReadInt32());
        }

        public static void Serialize(this RedOwlSerializer s, ref Transform value)
        {
            if (s.IsWriting)
            {
                s.Writer.Write(value.localPosition);
                s.Writer.Write(value.localRotation);
                s.Writer.Write(value.localScale);
                return;
            }

            value.localPosition = s.Reader.ReadVector3();
            value.localRotation = s.Reader.ReadQuaternion();
            value.localScale = s.Reader.ReadVector3();
        }

        public static void Serializer(this RedOwlSerializer serializer, ref RectTransform value)
        {
            if (serializer.IsWriting)
            {
                serializer.Writer.Write(value.localPosition);
                serializer.Writer.Write(value.localRotation);
                serializer.Writer.Write(value.localScale);
                serializer.Writer.Write(value.anchorMin);
                serializer.Writer.Write(value.anchorMax);
                serializer.Writer.Write(value.anchoredPosition);
                serializer.Writer.Write(value.sizeDelta);
                serializer.Writer.Write(value.pivot);
                serializer.Writer.Write(value.offsetMin);
                serializer.Writer.Write(value.offsetMax);
                return;
            }

            value.localPosition = serializer.Reader.ReadVector3();
            value.localRotation = serializer.Reader.ReadQuaternion();
            value.localScale = serializer.Reader.ReadVector3();
            value.anchorMin = serializer.Reader.ReadVector2();
            value.anchorMax = serializer.Reader.ReadVector2();
            value.anchoredPosition = serializer.Reader.ReadVector2();
            value.sizeDelta = serializer.Reader.ReadVector2();
            value.pivot = serializer.Reader.ReadVector2();
            value.offsetMin = serializer.Reader.ReadVector2();
            value.offsetMax = serializer.Reader.ReadVector2();
        }
        
        #endregion
    }
    
    #region Json
    
	public static class JsonExtensions
	{
        public static void Serialize<T>(this JsonSerializer self, JsonWriter writer, string key, T value)
        {
            writer.WritePropertyName(key);
            self.Serialize(writer, value, typeof(T));
        }
        
        public static void Write<T>(this JsonWriter self, string key, T value)
		{
			self.WritePropertyName(key);
			self.WriteValue(value);
		}

		public static void TryGet<T>(this JObject self, string key, ref T output)
		{
			if (self.TryGetValue(key, out var token))
			{
				output = token.ToObject<T>();
			}
		}
        
        public static T TryGet<T>(this JObject self, string key, T output)
        {
            return self.TryGetValue(key, out var token) ? token.ToObject<T>() : output;
        }
	}

	public abstract class BaseConverter<T> : JsonConverter<T>
	{
		public override void WriteJson(JsonWriter writer, T value, JsonSerializer serializer)
		{
			writer.WriteStartObject();
			Write(writer, value, serializer);
			writer.WriteEndObject();
		}

		public override T ReadJson(JsonReader reader, Type objectType, T existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			var output = hasExistingValue ? existingValue : GetDefault();
			if (reader.TokenType != JsonToken.Null) Read(reader, JObject.Load(reader), ref output, serializer);
			return output;
		}
		
		protected abstract T GetDefault();

		protected abstract void Write(JsonWriter writer, T value, JsonSerializer serializer);
		protected abstract void Read(JsonReader reader, JObject jObject, ref T existingValue, JsonSerializer serializer);
	}
	
	public class ColorConverter : BaseConverter<Color>
	{
		protected override Color GetDefault() => Color.black;

		protected override void Write(JsonWriter writer, Color value, JsonSerializer serializer)
		{
			writer.Write("r", value.r);
			writer.Write("g", value.g);
			writer.Write("b", value.b);
			writer.Write("a", value.a);
		}

		protected override void Read(JsonReader reader, JObject jObject, ref Color existingValue, JsonSerializer serializer)
		{
			jObject.TryGet("r", ref existingValue.r);
			jObject.TryGet("g", ref existingValue.g);
			jObject.TryGet("b", ref existingValue.b);
			jObject.TryGet("a", ref existingValue.a);
		}
	}
	
	public class Color32Converter : BaseConverter<Color32>
	{
		protected override Color32 GetDefault() => Color.black;

		protected override void Write(JsonWriter writer, Color32 value, JsonSerializer serializer)
		{
			writer.Write("r", value.r);
			writer.Write("g", value.g);
			writer.Write("b", value.b);
			writer.Write("a", value.a);
		}

		protected override void Read(JsonReader reader, JObject jObject, ref Color32 existingValue, JsonSerializer serializer)
		{
			jObject.TryGet("r", ref existingValue.r);
			jObject.TryGet("g", ref existingValue.g);
			jObject.TryGet("b", ref existingValue.b);
			jObject.TryGet("a", ref existingValue.a);
		}
	}
    
    public class Vector2IntConverter : BaseConverter<Vector2Int>
    {
        protected override Vector2Int GetDefault() => Vector2Int.zero;

        protected override void Write(JsonWriter writer, Vector2Int value, JsonSerializer serializer)
        {
            writer.Write("x", value.x);
            writer.Write("y", value.y);
        }

        protected override void Read(JsonReader reader, JObject jObject, ref Vector2Int existingValue, JsonSerializer serializer)
        {
            existingValue.x = jObject.TryGet("x", existingValue.x);
            existingValue.y = jObject.TryGet("y", existingValue.y);
        }
    }
    
    public class Vector3IntConverter : BaseConverter<Vector3Int>
    {
        protected override Vector3Int GetDefault() => Vector3Int.zero;

        protected override void Write(JsonWriter writer, Vector3Int value, JsonSerializer serializer)
        {
            writer.Write("x", value.x);
            writer.Write("y", value.y);
            writer.Write("z", value.z);
        }

        protected override void Read(JsonReader reader, JObject jObject, ref Vector3Int existingValue, JsonSerializer serializer)
        {
            existingValue.x = jObject.TryGet("x", existingValue.x);
            existingValue.y = jObject.TryGet("y", existingValue.y);
            existingValue.z = jObject.TryGet("z", existingValue.z);
        }
    }
    
	public class Vector2Converter : BaseConverter<Vector2>
	{
		protected override Vector2 GetDefault() => Vector2.zero;

		protected override void Write(JsonWriter writer, Vector2 value, JsonSerializer serializer)
		{
			writer.Write("x", value.x);
			writer.Write("y", value.y);
		}

		protected override void Read(JsonReader reader, JObject jObject, ref Vector2 existingValue, JsonSerializer serializer)
		{
			jObject.TryGet("x", ref existingValue.x);
			jObject.TryGet("y", ref existingValue.y);
		}
	}
	
	public class Vector3Converter : BaseConverter<Vector3>
	{
		protected override Vector3 GetDefault() => Vector3.zero;

		protected override void Write(JsonWriter writer, Vector3 value, JsonSerializer serializer)
		{
			writer.Write("x", value.x);
			writer.Write("y", value.y);
			writer.Write("z", value.z);
		}

		protected override void Read(JsonReader reader, JObject jObject, ref Vector3 existingValue, JsonSerializer serializer)
		{
			jObject.TryGet("x", ref existingValue.x);
			jObject.TryGet("y", ref existingValue.y);
			jObject.TryGet("z", ref existingValue.z);
		}
	}
	
	public class Vector4Converter : BaseConverter<Vector4>
	{
		protected override Vector4 GetDefault() => Vector4.zero;

		protected override void Write(JsonWriter writer, Vector4 value, JsonSerializer serializer)
		{
			writer.Write("x", value.x);
			writer.Write("y", value.y);
			writer.Write("z", value.z);
			writer.Write("w", value.w);
		}

		protected override void Read(JsonReader reader, JObject jObject, ref Vector4 existingValue, JsonSerializer serializer)
		{
			jObject.TryGet("x", ref existingValue.x);
			jObject.TryGet("y", ref existingValue.y);
			jObject.TryGet("z", ref existingValue.z);
			jObject.TryGet("w", ref existingValue.w);
		}
	}
	
	public class QuaternionConverter : BaseConverter<Quaternion>
	{
		protected override Quaternion GetDefault() => Quaternion.identity;

		protected override void Write(JsonWriter writer, Quaternion value, JsonSerializer serializer)
		{
			writer.Write("x", value.x);
			writer.Write("y", value.y);
			writer.Write("z", value.z);
			writer.Write("w", value.w);
		}

		protected override void Read(JsonReader reader, JObject jObject, ref Quaternion existingValue, JsonSerializer serializer)
		{
			jObject.TryGet("x", ref existingValue.x);
			jObject.TryGet("y", ref existingValue.y);
			jObject.TryGet("z", ref existingValue.z);
			jObject.TryGet("w", ref existingValue.w);
		}
	}
    
    public class PlaneConverter : BaseConverter<Plane>
    {
        protected override Plane GetDefault() => new Plane();

        protected override void Write(JsonWriter writer, Plane value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, "normal", value.normal);
            serializer.Serialize(writer, "distance", value.distance);
        }

        protected override void Read(JsonReader reader, JObject jObject, ref Plane existingValue, JsonSerializer serializer)
        {
            existingValue.normal = jObject.TryGet("normal", existingValue.normal);
            existingValue.distance = jObject.TryGet("distance", existingValue.distance);
        }
    }
    
    public class BoundsConverter : BaseConverter<Bounds>
    {
        protected override Bounds GetDefault() => new Bounds();

        protected override void Write(JsonWriter writer, Bounds value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, "center", value.center);
            serializer.Serialize(writer, "size", value.size);
        }

        protected override void Read(JsonReader reader, JObject jObject, ref Bounds existingValue, JsonSerializer serializer)
        {
            existingValue.center = jObject.TryGet("center", existingValue.center);
            existingValue.size = jObject.TryGet("size", existingValue.size);
        }
    }
    
    public class BoundsIntConverter : BaseConverter<BoundsInt>
    {
        protected override BoundsInt GetDefault() => new BoundsInt();

        protected override void Write(JsonWriter writer, BoundsInt value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, "position", value.position);
            serializer.Serialize(writer, "size", value.size);
        }

        protected override void Read(JsonReader reader, JObject jObject, ref BoundsInt existingValue, JsonSerializer serializer)
        {
            existingValue.position = jObject.TryGet("position", existingValue.position);
            existingValue.size = jObject.TryGet("size", existingValue.size);
        }
    }
    
    public class RectConverter : BaseConverter<Rect>
    {
        protected override Rect GetDefault() => new Rect();

        protected override void Write(JsonWriter writer, Rect value, JsonSerializer serializer)
        {
            writer.Write("x", value.x);
            writer.Write("y", value.y);
            writer.Write("width", value.width);
            writer.Write("height", value.height);
        }

        protected override void Read(JsonReader reader, JObject jObject, ref Rect existingValue, JsonSerializer serializer)
        {
            existingValue.x = jObject.TryGet("x", existingValue.x);
            existingValue.y = jObject.TryGet("y", existingValue.y);
            existingValue.width = jObject.TryGet("width", existingValue.width);
            existingValue.height = jObject.TryGet("height", existingValue.height);
        }
    }
    
    public class RectIntConverter : BaseConverter<RectInt>
    {
        protected override RectInt GetDefault() => new RectInt();

        protected override void Write(JsonWriter writer, RectInt value, JsonSerializer serializer)
        {
            writer.Write("x", value.x);
            writer.Write("y", value.y);
            writer.Write("width", value.width);
            writer.Write("height", value.height);
        }

        protected override void Read(JsonReader reader, JObject jObject, ref RectInt existingValue, JsonSerializer serializer)
        {
            existingValue.x = jObject.TryGet("x", existingValue.x);
            existingValue.y = jObject.TryGet("y", existingValue.y);
            existingValue.width = jObject.TryGet("width", existingValue.width);
            existingValue.height = jObject.TryGet("height", existingValue.height);
        }
    }
    
    public class RectOffsetConverter : BaseConverter<RectOffset>
    {
        protected override RectOffset GetDefault() => new RectOffset();

        protected override void Write(JsonWriter writer, RectOffset value, JsonSerializer serializer)
        {
            writer.Write("left", value.left);
            writer.Write("right", value.right);
            writer.Write("top", value.top);
            writer.Write("bottom", value.bottom);
        }

        protected override void Read(JsonReader reader, JObject jObject, ref RectOffset existingValue, JsonSerializer serializer)
        {
            existingValue.left = jObject.TryGet("left", existingValue.left);
            existingValue.right = jObject.TryGet("right", existingValue.right);
            existingValue.top = jObject.TryGet("top", existingValue.top);
            existingValue.bottom = jObject.TryGet("bottom", existingValue.bottom);
        }
    }
    
    #endregion
}