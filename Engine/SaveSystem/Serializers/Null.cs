using System;
using System.IO;

namespace RedOwl.Engine
{
    public class NullSerializer : Serializer
    {
        public override Type For => typeof(object);

        public void Write(BinaryWriter writer, object value) {}
        public object Read(BinaryReader reader) => null;
    }
}