using System.IO;
using UnityEngine;

namespace RedOwl.Engine
{
    public static partial class Serializers
    {
        public static void WriteQuaternion(BinaryWriter writer, Quaternion value)
        {
            writer.Write(value.x);
            writer.Write(value.y);
            writer.Write(value.z);
            writer.Write(value.w);
        }

        public static Quaternion ReadQuaternion(BinaryReader reader)
        {
            return new Quaternion(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }
    }
    
    public class SerializerQuaternion : Serializer<Quaternion>
    {
        public SerializerQuaternion() : base(Serializers.WriteQuaternion, Serializers.ReadQuaternion) {}
    }
    
    public class SerializerQuaternionArray : SerializerArray<Quaternion>
    {
        public SerializerQuaternionArray() : base(Serializers.WriteQuaternion, Serializers.ReadQuaternion) {}
    }
    
    public class SerializerQuaternionList : SerializerList<Quaternion>
    {
        public SerializerQuaternionList() : base(Serializers.WriteQuaternion, Serializers.ReadQuaternion) {}
    }
}