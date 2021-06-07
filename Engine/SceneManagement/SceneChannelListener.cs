using System;
using UnityEngine;
using UnityEngine.Events;

namespace RedOwl.Engine
{
    public class SceneChannelListener : MonoBehaviour
    {
        [Serializable]
        public class SceneUnityEvent : UnityEvent<SceneMetadata> {}
        
        public SceneChannel channel;

        public SceneUnityEvent On;

        private void OnEnable()
        {
            channel.On += Handler;
        }

        private void OnDisable()
        {
            channel.On -= Handler;
        }

        private void Handler(SceneMetadata metadata)
        {
            On?.Invoke(metadata);
        }
    }
}