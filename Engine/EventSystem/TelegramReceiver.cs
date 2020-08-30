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
        public TelegramReference telegramReference;
        public UnityEvent response;
        
        private void Awake()
        {
            if (style == TelegramStyles.AwakeDestroy) telegramReference.Subscribe(OnEvent);
        }

        private void OnEnable()
        {
            if (style == TelegramStyles.EnableDisable) telegramReference.Subscribe(OnEvent);
        }

        private void OnDisable()
        {
            if (style == TelegramStyles.EnableDisable) telegramReference.Unsubscribe(OnEvent);
        }

        private void OnDestroy()
        {
            if (style == TelegramStyles.AwakeDestroy) telegramReference.Unsubscribe(OnEvent);
        }
        
        private void OnEvent()
        {
            response?.Invoke();
        }
    }
}