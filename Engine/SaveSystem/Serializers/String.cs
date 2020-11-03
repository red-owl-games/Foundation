using System.IO;

namespace RedOwl.Engine
{
    public static partial class Serializers
    {
        public static void WriteString(BinaryWriter writer, string value) => writer.Write(value);

        public static string ReadString(BinaryReader reader) => reader.ReadString();
    }

    public class SerializerString : Serializer<string>
    {
        public SerializerString() : base(Serializers.WriteString, Serializers.ReadString) {}
    }
    
    public class SerializerStringArray : SerializerArray<string>
    {
        public SerializerStringArray() : base(Serializers.WriteString, Serializers.ReadString) {}
    }
    
    public class SerializerStringList : SerializerList<string>
    {
        public SerializerStringList() : base(Serializers.WriteString, Serializers.ReadString) {}
    }
}