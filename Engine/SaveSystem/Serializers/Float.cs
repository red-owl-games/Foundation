using System.IO;

namespace RedOwl.Engine
{
    public static partial class Serializers
    {
        public static void WriteFloat(BinaryWriter writer, float value) => writer.Write(value);

        public static float ReadFloat(BinaryReader reader) => reader.ReadSingle();
    }

    public class SerializerFloat : Serializer<float>
    {
        public SerializerFloat() : base(Serializers.WriteFloat, Serializers.ReadFloat) {}
    }
    
    public class SerializerFloatArray : SerializerArray<float>
    {
        public SerializerFloatArray() : base(Serializers.WriteFloat, Serializers.ReadFloat) {}
    }
    
    public class SerializerFloatList : SerializerList<float>
    {
        public SerializerFloatList() : base(Serializers.WriteFloat, Serializers.ReadFloat) {}
    }
}