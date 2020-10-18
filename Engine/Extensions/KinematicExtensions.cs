using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

namespace RedOwl.Core
{
    public static class Kinematic
    {
        //s = displacement
        //u = initial velocity
        //v = final velocity
        //a = acceleration
        //t = time

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float CalculateGravity_Jump(float jumpHeight, float jumpTime)
        {
            return -(2 * jumpHeight) / math.pow(jumpTime, 2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float CalculateVelocity_Jump(float jumpHeight, float gravity)
        {
            return math.sqrt(-2 * math.abs(gravity) * jumpHeight);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 CalculateInitialVelocity_Arc(float3 position, float3 target, float gravity, float arcHeight)
        {
            return new float3(target.x - position.x, 0, target.z - position.z) 
                   / (math.sqrt(-2 * arcHeight / gravity) 
                      + math.sqrt(2 * (target.y - position.y - arcHeight) / gravity))
                   + new float3(0, 1, 0) * math.sqrt(-2 * gravity * arcHeight) * -math.sign(gravity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float CalculateTime_Arc(float3 position, float3 target, float gravity, float arcHeight)
        {
            return math.sqrt(-2 * arcHeight / gravity) + math.sqrt(2 * (target.y - position.y - arcHeight) / gravity);
        }
    }
}