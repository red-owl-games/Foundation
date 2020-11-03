using UnityEngine;
using rng = UnityEngine.Random;

namespace RedOwl.Engine
{
    public static class VectorExtensions
    {
        public static Vector3 Random(float min = -20, float max = 20)
        {
            return new Vector3(rng.Range(min, max), rng.Range(min, max), rng.Range(min, max));
        }
    }
}