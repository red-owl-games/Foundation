using System.IO;
using Unity.Mathematics;

namespace RedOwl.Engine
{
    public static partial class Serializers
    {
        public static void WriteFloat3(BinaryWriter writer, float3 value)
        {
            writer.Write(value.x);
            writer.Write(value.y);
            writer.Write(value.z);
        }

        public static float3 ReadFloat3(BinaryReader reader)
        {
            return new float3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }
    }

    public class SerializerFloat3 : Serializer<float3>
    {
        public SerializerFloat3() : base(Serializers.WriteFloat3, Serializers.ReadFloat3) {}
    }
    
    public class SerializerFloat3Array : SerializerArray<float3>
    {
        public SerializerFloat3Array() : base(Serializers.WriteFloat3, Serializers.ReadFloat3) {}
    }
    
    public class SerializerFloat3List : SerializerList<float3>
    {
        public SerializerFloat3List() : base(Serializers.WriteFloat3, Serializers.ReadFloat3) {}
    }
}