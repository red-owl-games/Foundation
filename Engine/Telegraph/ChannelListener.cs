using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RedOwl.Engine
{
    [Serializable]
    public class ChannelMapping
    {
        public Channel channel;

        [UnityEventFoldout]
        public UnityEvent On;
        

        public void OnEnable()
        {
            channel.On += Handler;
        }

        public void OnDisable()
        {
            channel.On -= Handler;
        }
        
        private void Handler()
        {
            On?.Invoke();
        }
    }
    
    public class ChannelListener : MonoBehaviour
    {
        public List<ChannelMapping> Handlers;

        private void OnEnable()
        {
            foreach (var handler in Handlers)
            {
                handler.OnEnable();
            }
        }

        private void OnDisable()
        {
            foreach (var handler in Handlers)
            {
                handler.OnDisable();
            }
        }
    }
}