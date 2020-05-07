using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

namespace RedOwl.Core
{
    public sealed class mathExt
    {
        public static readonly float3 Zero = new float3(0);
        public static readonly float3 One = new float3(1);
        public static readonly float3 Right = new float3(1, 0, 0);
        public static readonly float3 Left = new float3(-1, 0, 0);
        public static readonly float3 Up = new float3(0, 1, 0);
        public static readonly float3 Down = new float3(0, -1, 0);
        public static readonly float3 Forward = new float3(0, 0, 1);
        public static readonly float3 Backward = new float3(0, 0, -1);
        
        public static float Angle(float2 from, float2 to)
        {
            float num = math.sqrt(math.lengthsq(from) * math.lengthsq(to));
            return num < 1.00000000362749E-15 ? 0.0f : math.degrees(math.acos(math.clamp(math.dot(from, to) / num, -1f, 1f)));
        }
        
        public static float Angle(float3 from, float3 to)
        {
            float num = math.sqrt(math.lengthsq(from) * math.lengthsq(to));
            return num < 1.00000000362749E-15 ? 0.0f : math.degrees(math.acos(math.clamp(math.dot(from, to) / num, -1f, 1f)));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2 Round(float value, int decimalPlaces = 4)
        {
            float multiplier = 1;
            for (int i = 0; i < decimalPlaces; i++)
            {
                multiplier *= 10f;
            }

            return math.round(value * multiplier) / multiplier;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2 Round(float2 value, int decimalPlaces = 4)
        {
            float multiplier = 1;
            for (int i = 0; i < decimalPlaces; i++)
            {
                multiplier *= 10f;
            }

            return new float2(
                math.round(value.x * multiplier) / multiplier,
                math.round(value.y * multiplier) / multiplier);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 Round(float3 value, int decimalPlaces = 4)
        {
            float multiplier = 1;
            for (int i = 0; i < decimalPlaces; i++)
            {
                multiplier *= 10f;
            }
            return new float3(
                math.round(value.x * multiplier) / multiplier,
                math.round(value.y * multiplier) / multiplier,
                math.round(value.z * multiplier) / multiplier);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float4 Round(float4 value, int decimalPlaces = 4)
        {
            float multiplier = 1;
            for (int i = 0; i < decimalPlaces; i++)
            {
                multiplier *= 10f;
            }
            return new float4(
                math.round(value.x * multiplier) / multiplier,
                math.round(value.y * multiplier) / multiplier,
                math.round(value.z * multiplier) / multiplier,
                math.round(value.w * multiplier) / multiplier);
        }

        public static quaternion ClampRotationX(quaternion value, float min, float max)
        {
            var q = value.value;
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;
 
            float angle = math.degrees(2f) * math.atan(q.x);
            angle = math.clamp(angle, min, max);
            q.x = math.tan(math.radians(0.5f) * angle);
 
            return q;
        }
        
        public static quaternion ClampRotationY(quaternion value, float min, float max)
        {
            var q = value.value;
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;
 
            float angle = math.degrees(2f) * math.atan(q.y);
            angle = math.clamp(angle, min, max);
            q.y = math.tan(math.radians(0.5f) * angle);
 
            return q;
        }
        
        public static quaternion ClampRotationZ(quaternion value, float min, float max)
        {
            var q = value.value;
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;
 
            float angle = math.degrees(2f) * math.atan(q.z);
            angle = math.clamp(angle, min, max);
            q.z = math.tan(math.radians(0.5f) * angle);
 
            return q;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Clamp01(float value)
        {
            return math.clamp(value, 0f, 1f);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Remap(float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Distance(float3 a, float3 b)
        {
            float num1 = a.x - b.x;
            float num2 = a.y - b.y;
            float num3 = a.z - b.z;
            return math.sqrt(num1 * num1 + num2 * num2 + num3 * num3);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SqrMagnitude(float3 value)
        {
            return value.x * value.x + value.y * value.y + value.z * value.z;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Magnitude(float3 value)
        {
            return math.sqrt(SqrMagnitude(value));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 Normalize(float3 value)
        {
            float num = Magnitude(value);
            return num > 9.99999974737875E-06 ? value / num : Zero;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Parabola(float value, float min = 0f, float max = 1f)
        {
            float x = Remap(value, min, max, 0f, 1f);
            return 4.0f * x * (1.0f - x );
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 CalculateTargetVelocity(float3 targetPosition, float3 currentPosition, float3 currentVelocity, float acceleration)
        {
            float3 direction = targetPosition - currentPosition;
            float targetDistance = Distance(currentPosition, targetPosition);
            float stoppingDistance = CalculateStoppingDistance(Magnitude(currentVelocity), acceleration);
            float3 targetVelocity = direction;
            if (targetDistance <= stoppingDistance)
            {
                targetVelocity = -1 * targetVelocity;
            }
            else if (targetDistance <= (stoppingDistance * 1.05))
            {
                targetVelocity = Zero;
            }

            return targetVelocity;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float CalculateStoppingDistance(float velocity, float acceleration){
            return (velocity / acceleration) * velocity * 0.5f;
        }

        // Original Source: http://wiki.unity3d.com/index.php/Mathfx
        #region MathFX
        
        //Ease in out
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Hermite(float start, float end, float value)
        {
            return math.lerp(start, end, value * value * (3.0f - 2.0f * value));
        }

        public static float2 Hermite(float2 start, float2 end, float value)
        {
            return new float2(
                Hermite(start.x, end.x, value),
                Hermite(start.y, end.y, value));
        }

        public static float3 Hermite(float3 start, float3 end, float value)
        {
            return new float3(
                Hermite(start.x, end.x, value),
                Hermite(start.y, end.y, value),
                Hermite(start.z, end.z, value));
        }

        //Ease out
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Sinerp(float start, float end, float value)
        {
            return math.lerp(start, end, math.sin(value * math.PI * 0.5f));
        }

        public static float2 Sinerp(float2 start, float2 end, float value)
        {
            return new float2(
                math.lerp(start.x, end.x, math.sin(value * math.PI * 0.5f)),
                math.lerp(start.y, end.y, math.sin(value * math.PI * 0.5f)));
        }

        public static float3 Sinerp(float3 start, float3 end, float value)
        {
            return new float3(
                math.lerp(start.x, end.x, math.sin(value * math.PI * 0.5f)),
                math.lerp(start.y, end.y, math.sin(value * math.PI * 0.5f)),
                math.lerp(start.z, end.z, math.sin(value * math.PI * 0.5f)));
        }

        //Ease in
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Coserp(float start, float end, float value)
        {
            return math.lerp(start, end, 1.0f - math.cos(value * math.PI * 0.5f));
        }

        public static float2 Coserp(float2 start, float2 end, float value)
        {
            return new float2(
                Coserp(start.x, end.x, value),
                Coserp(start.y, end.y, value));
        }

        public static float3 Coserp(float3 start, float3 end, float value)
        {
            return new float3(
                Coserp(start.x, end.x, value),
                Coserp(start.y, end.y, value),
                Coserp(start.z, end.z, value));
        }

        //Bounce
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Bounce(float x)
        {
            return math.abs(math.sin(6.28f * (x + 1f) * (x + 1f)) * (1f - x));
        }

        public static float2 Bounce(float2 vec)
        {
            return new float2(
                Bounce(vec.x),
                Bounce(vec.y));
        }

        public static float3 Bounce(float3 vec)
        {
            return new float3(
                Bounce(vec.x),
                Bounce(vec.y),
                Bounce(vec.z));
        }

        //Boing
        public static float Berp(float start, float end, float value)
        {
            value = math.clamp(value, 0, 1);
            value = (math.sin(value * math.PI * (0.2f + 2.5f * value * value * value)) * math.pow(1f - value, 2.2f) +
                     value) * (1f + 1.2f * (1f - value));
            return start + (end - start) * value;
        }

        public static float2 Berp(float2 start, float2 end, float value)
        {
            return new float2(
                Berp(start.x, end.x, value),
                Berp(start.y, end.y, value));
        }

        public static float3 Berp(float3 start, float3 end, float value)
        {
            return new float3(
                Berp(start.x, end.x, value),
                Berp(start.y, end.y, value),
                Berp(start.z, end.z, value));
        }

        //Like lerp with ease in ease out
        public static float SmoothStep(float x, float min, float max)
        {
            x = math.clamp(x, min, max);
            float v1 = (x - min) / (max - min);
            float v2 = (x - min) / (max - min);
            return -2 * v1 * v1 * v1 + 3 * v2 * v2;
        }

        public static float2 SmoothStep(float2 vec, float min, float max)
        {
            return new float2(
                SmoothStep(vec.x, min, max),
                SmoothStep(vec.y, min, max));
        }

        public static float3 SmoothStep(float3 vec, float min, float max)
        {
            return new float3(
                SmoothStep(vec.x, min, max),
                SmoothStep(vec.y, min, max),
                SmoothStep(vec.z, min, max));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Lerp(float start, float end, float value)
        {
            return (1.0f - value) * start + value * end;
        }

        public static float2 Lerp(float2 start, float2 end, float value)
        {
            return new float2(
                Lerp(start.x, end.x, value),
                Lerp(start.y, end.y, value));
        }

        public static float3 Lerp(float3 start, float3 end, float value)
        {
            return new float3(
                Lerp(start.x, end.x, value),
                Lerp(start.y, end.y, value),
                Lerp(start.z, end.z, value));
        }

        public static float3 NearestPoint(float3 lineStart, float3 lineEnd, float3 point)
        {
            var lineDirection = math.normalize(lineEnd - lineStart);
            float closestPoint = math.dot((point - lineStart), lineDirection);
            return lineStart + (closestPoint * lineDirection);
        }

        public static float3 NearestPointStrict(float3 lineStart, float3 lineEnd, float3 point)
        {
            var fullDirection = lineEnd - lineStart;
            var lineDirection = math.normalize(fullDirection);
            float closestPoint = math.dot((point - lineStart), lineDirection);
            return lineStart + (math.clamp(closestPoint, 0.0f, math.length(fullDirection)) * lineDirection);
        }

        // test for value that is near specified float (due to floating point inprecision)
        // all thanks to Opless for this!
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Approx(float val, float about, float range)
        {
            return (math.abs(val - about) < range);
        }

        // test if a float2 is close to another float3 (due to floating point inprecision)
        // compares the square of the distance to the square of the range as this 
        // avoids calculating a square root which is much slower than squaring the range
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Approx(float2 val, float2 about, float range)
        {
            return math.lengthsq(val - about) < range * range;
        }

        // test if a float3 is close to another float3 (due to floating point inprecision)
        // compares the square of the distance to the square of the range as this 
        // avoids calculating a square root which is much slower than squaring the range
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Approx(float3 val, float3 about, float range)
        {
            return math.lengthsq(val - about) < range * range;
        }

        /*
        * CLerp - Circular Lerp - is like lerp but handles the wraparound from 0 to 360.
        * This is useful when interpolating eulerAngles and the object
        * crosses the 0/360 boundary.  The standard Lerp function causes the object
        * to rotate in the wrong direction and looks stupid. Clerp fixes that.
        */
        private const float CLerpMin = 0.0f;
        private const float CLerpMax = 360.0f;

        public static float CLerp(float start, float end, float value)
        {

            float half = math.abs((CLerpMax - CLerpMin) / 2.0f); //half the distance between CLerpMin and CLerpMax
            float retval = 0.0f;
            float diff = 0.0f;
            if (end - start < -half)
            {
                diff = (CLerpMax - start + end) * value;
                retval = start + diff;
            }
            else if (end - start > half)
            {
                diff = -((CLerpMax - end) + start) * value;
                retval = start + diff;
            }
            else retval = start + (end - start) * value;

            // Log.Debug("Start: "  + start + "   End: " + end + "  Value: " + value + "  Half: " + half + "  Diff: " + diff + "  Retval: " + retval);
            return retval;
        }
        
        #endregion
    }
}
