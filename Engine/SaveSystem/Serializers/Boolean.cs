using System.IO;

namespace RedOwl.Engine
{
    public static partial class Serializers
    {
        public static void WriteBoolean(BinaryWriter writer, bool value) => writer.Write(value);

        public static bool ReadBoolean(BinaryReader reader) => reader.ReadBoolean();
    }
    
    public class SerializerBool : Serializer<bool>
    {
        public SerializerBool() : base(Serializers.WriteBoolean, Serializers.ReadBoolean) {}
    }
    
    public class SerializerBoolArray : SerializerArray<bool>
    {
        public SerializerBoolArray() : base(Serializers.WriteBoolean, Serializers.ReadBoolean) {}
    }
    
    public class SerializerBoolList : SerializerList<bool>
    {
        public SerializerBoolList() : base(Serializers.WriteBoolean, Serializers.ReadBoolean) {}
    }
}