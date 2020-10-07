using Unity.Mathematics;
using UnityEngine;

namespace RedOwl.Core
{
    public struct SimpleTransform
    {
        public float3 position;
        public quaternion rotation;
        public float3 scale;
        public float scaleMagnitude => (float) math.sqrt(scale.x * (double) scale.x + scale.y * (double) scale.y + scale.z * (double) scale.z);

        public SimpleTransform(float3 position) : this(position, new float3(0), new float3(1)) {}
        
        public SimpleTransform(float3 position, float3 rotation) : this(position, rotation, new float3(1)) {}
        
        public SimpleTransform(float3 position, Quaternion rotation) : this(position, rotation, new float3(1)) {}
        
        public SimpleTransform(float3 position, float3 rotation, float3 scale)
        {
            this.position = position;
            this.rotation = quaternion.Euler(rotation);
            this.scale = scale;
        }
        
        public SimpleTransform(float3 position, Quaternion rotation, float3 scale)
        {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
        }

        public static implicit operator SimpleTransform(Transform transform)
        {
            return new SimpleTransform
            {
                position = transform.position,
                rotation = transform.rotation,
                scale = transform.localScale
            };
        }
    }
}