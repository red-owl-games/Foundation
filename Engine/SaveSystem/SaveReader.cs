using System.IO;
using System.Text;
using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEngine;

namespace RedOwl.Core
{
    public class SaveReader : BinaryReader
    {
        public SaveReader([NotNull] Stream input) : base(input) { }

        public SaveReader([NotNull] Stream input, [NotNull] Encoding encoding) : base(input, encoding) { }

        public SaveReader(Stream input, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen) { }

        public Vector2 ReadVector2() => new Vector2(ReadSingle(), ReadSingle());
        public Vector3 ReadVector3() => new Vector3(ReadSingle(), ReadSingle(), ReadSingle());
        public Vector4 ReadVector4() => new Vector4(ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle());
        public quaternion ReadQuaternion() => new quaternion(ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle());
    }
}