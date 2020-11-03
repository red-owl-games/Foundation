using System.IO;
using RedOwl.Engine;
using UnityEngine;

namespace Project
{
    public static partial class Serializers
    {
        public static void WriteVector3(BinaryWriter writer, Vector3 value)
        {
            writer.Write(value.x);
            writer.Write(value.y);
            writer.Write(value.z);
        }

        public static Vector3 ReadVector3(BinaryReader reader)
        {
            return new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }
    }

    public class SerializerVector3 : Serializer<Vector3>
    {
        public SerializerVector3() : base(Serializers.WriteVector3, Serializers.ReadVector3) {}
    }
    
    public class SerializerVector3Array : SerializerArray<Vector3>
    {
        public SerializerVector3Array() : base(Serializers.WriteVector3, Serializers.ReadVector3) {}
    }
    
    public class SerializerVector3List : SerializerList<Vector3>
    {
        public SerializerVector3List() : base(Serializers.WriteVector3, Serializers.ReadVector3) {}
    }
}