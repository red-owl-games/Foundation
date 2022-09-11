using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    public class Hitbox : MonoBehaviour
    {
        public enum Shape
        {
            Box = 1,
            Sphere = 2
        }

        public Shape shape = Shape.Box;
        [ShowIf("IsBox")] public Vector3 size;
        [HideIf("IsBox")] public float radius;
        public Vector3 offset;
        [TagSelector] public string targetTag;
        public LayerMask targetLayers;
        public int hitBuffer = 4;

        public event Action<GameObject> OnHit;

        private Transform _transform;
        private Collider[] _results;
        private Vector3 _halfExtents;

        private bool IsBox => shape == Shape.Box;

        private void Awake()
        {
            Setup();
        }

        private void OnValidate()
        {
            Setup();
        }

        private void Setup()
        {
            _transform = transform;
            _results = new Collider[hitBuffer];
            _halfExtents = size * 0.5f;
        }

        [Button]
        private void Fire()
        {
            var hitCount = OverlapTest();
            for (var i = 0; i < hitCount; i++)
            {
                var result = _results[i].gameObject;
                if (result.CompareTag(targetTag)) 
                    OnHit?.Invoke(result);
            }
        }

        private int OverlapTest()
        {
            var position = _transform.TransformPoint(offset);
            var rotation = _transform.rotation;
            return IsBox ?
                Physics.OverlapBoxNonAlloc(position, _halfExtents, _results, rotation, targetLayers) :
                Physics.OverlapSphereNonAlloc(position, radius, _results, targetLayers);
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            if (IsBox)
            {
                Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
                Gizmos.DrawWireCube(offset, size);
            }
            else
            {
                Gizmos.DrawWireSphere(_transform.TransformPoint(offset), radius);
            }
        }
    }
}