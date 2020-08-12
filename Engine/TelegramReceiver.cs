using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace RedOwl.Core
{
    public enum TelegramStyles
    {
        EnableDisable,
        AwakeDestroy
    }

    [HideMonoScript]
    public class TelegramReceiver : MonoBehaviour
    {
        public TelegramStyles style;
        public ITelegram telegram;
        public UnityEvent response;
        
        private void Awake()
        {
            if (style == TelegramStyles.AwakeDestroy) telegram.On += OnEvent;
        }

        private void OnEnable()
        {
            if (style == TelegramStyles.EnableDisable) telegram.On += OnEvent;
        }

        private void OnDisable()
        {
            if (style == TelegramStyles.EnableDisable) telegram.On -= OnEvent;
        }

        private void OnDestroy()
        {
            if (style == TelegramStyles.AwakeDestroy) telegram.On -= OnEvent;
        }
        
        private void OnEvent()
        {
            response?.Invoke();
        }
    }
}