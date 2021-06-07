using System.Collections;
using UnityEngine;

namespace RedOwl.Engine
{
    public interface IUnityBridgeTarget
    {
        IEnumerator AsyncUpdate(float dt);
        void Update(float dt);
        void LateUpdate(float dt);
        void FixedUpdate(float dt);
    }
    
    public class UnityBridge : MonoBehaviour
    {
        public IUnityBridgeTarget Target { get; set; }

        private void Start()
        {
            StartCoroutine(AsyncUpdate());
        }

        private IEnumerator AsyncUpdate()
        {
            while (true)
            {
                yield return Target?.AsyncUpdate(Time.deltaTime);
            }
        }

        private void Update()
        {
            Target?.Update(Time.deltaTime);
        }

        private void LateUpdate()
        {
            Target?.LateUpdate(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            Target?.FixedUpdate(Time.deltaTime);
        }
    }
}
