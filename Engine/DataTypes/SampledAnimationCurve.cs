using System;
using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;

namespace RedOwl.Engine
{
    public struct SampledAnimationCurve : IDisposable
    {
        private NativeArray<float> _data;
        
        public SampledAnimationCurve(AnimationCurve ac, int samples)
        {
            _data = new NativeArray<float>(math.clamp(samples, 2, int.MaxValue), Allocator.Persistent);
            float timeFrom = ac.keys[0].time;
            float timeStep = (ac.keys[ac.keys.Length - 1].time - timeFrom) / (samples - 1);
 
            for (int i = 0; i < samples; i++)
            {
                _data[i] = ac.Evaluate(timeFrom + (i * timeStep));
            }
        }
 
        public void Dispose()
        {
            _data.Dispose();
        }
 
        public float Evaluate(float time)
        {
            int len = _data.Length - 1;
            float floatIndex = (math.clamp(time, 0f, 1f) * len);
            int floorIndex = (int)math.floor(floatIndex);
            return floorIndex == len ? _data[len] : math.lerp(_data[floorIndex], _data[floorIndex + 1], math.frac(floatIndex));
        }
    }
    
    public static class SampledAnimationCurveExtensions
    {
        public static SampledAnimationCurve GetSampledCurve(this AnimationCurve self, int samples = 256)
        {
            return new SampledAnimationCurve(self, samples);
        }
    }
}