using UnityEngine;

namespace RedOwl.Engine
{
    public class DrawTrail : MonoBehaviour
    {
        private Vector3 _lastPos;

        [SerializeField]
        private Color _color = Color.red;

        [SerializeField]
        private float _duration = 2f;

        [SerializeField]
        private bool _depthTest = true;

        private void Start()
        {
            _lastPos = transform.position;
        }

        private void LateUpdate()
        {
            var pos = transform.position;
            Debug.DrawLine(_lastPos, pos, _color, _duration, _depthTest);
            _lastPos = pos;
        }
    }
}