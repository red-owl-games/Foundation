using System;
using UnityEngine;
using UnityEngine.Events;

namespace RedOwl.Engine
{
    public class LocationChannelListener : MonoBehaviour
    {
        [Serializable]
        public class LocationUnityEvent : UnityEvent<LocationRef> {}
        
        public LocationChannel channel;

        public LocationUnityEvent On;

        private void OnEnable()
        {
            channel.On += Handler;
        }

        private void OnDisable()
        {
            channel.On -= Handler;
        }

        private void Handler(LocationRef metadata)
        {
            On?.Invoke(metadata);
        }
    }
}