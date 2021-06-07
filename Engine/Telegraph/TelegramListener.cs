using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace RedOwl.Engine
{
    [Serializable]
    public class MessageMapping
    {
        [TelegraphReference]
        public string message;

        [UnityEventFoldout]
        public UnityEvent On;
        
        private Message _message => Telegraph.Get<Message>(message);

        public void OnEnable()
        {
            _message.On += Handler;
        }

        public void OnDisable()
        {
            _message.On -= Handler;
        }
        
        private void Handler()
        {
            On?.Invoke();
        }
    }
    
    [HideMonoScript]
    public class TelegramListener : MonoBehaviour
    {
        public List<MessageMapping> Handlers;

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