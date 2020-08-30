using System.IO;
using Unity.Mathematics;
using UnityEngine;

namespace RedOwl.Core
{
    public class SaveWriter : BinaryWriter
    {
        public SaveWriter(Stream stream) : base(stream) {}

        /// <summary>
        /// 8 Bytes
        /// </summary>
        /// <param name="value"></param>
        public void Write(Vector2 value)
        {
            Write(value.x);
            Write(value.y);
        }
        
        /// <summary>
        /// 12 Bytes
        /// </summary>
        /// <param name="value"></param>
        public void Write(Vector3 value)
        {
            Write(value.x);
            Write(value.y);
            Write(value.z);
        }
        
        /// <summary>
        /// 16 Bytes
        /// </summary>
        /// <param name="value"></param>
        public void Write(Vector4 value)
        {
            Write(value.x);
            Write(value.y);
            Write(value.z);
            Write(value.w);
        }

        /// <summary>
        /// 16 Bytes
        /// </summary>
        /// <param name="value"></param>
        public void Write(quaternion value)
        {
            Write(value.value.x);
            Write(value.value.y);
            Write(value.value.z);
            Write(value.value.w);
        }
    }
}