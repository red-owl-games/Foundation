using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace RedOwl.Engine
{
    public enum ReceiverStyles
    {
        EnableDisable,
        AwakeDestroy,
        StartDestroy,
    }

    [HideMonoScript]
    public class GameMessageReceiver : MonoBehaviour
    {
        [HorizontalGroup("Settings", 0.3f), HideLabel]
        public ReceiverStyles style;
        [HorizontalGroup("Settings"), HideLabel]
        public GameMessageReference reference;
        [FoldoutGroup("Response"), HideLabel]
        public UnityEvent response;
        
        private void Awake()
        {
            if (style == ReceiverStyles.AwakeDestroy) reference.message.On += OnEvent;
        }

        private void OnEnable()
        {
            if (style == ReceiverStyles.EnableDisable) reference.message.On += OnEvent;
        }

        private void Start()
        {
            if (style == ReceiverStyles.StartDestroy) reference.message.On += OnEvent;
        }

        private void OnDisable()
        {
            if (style == ReceiverStyles.EnableDisable) reference.message.On -= OnEvent;
        }

        private void OnDestroy()
        {
            if (style == ReceiverStyles.AwakeDestroy) reference.message.On -= OnEvent;
            if (style == ReceiverStyles.StartDestroy) reference.message.On += OnEvent;
        }
        
        private void OnEvent()
        {
            response?.Invoke();
        }
    }
    
    [HideMonoScript]
    public abstract class GameMessageReceiver<TR, TE, TM, T0> : MonoBehaviour where TR : GameMessageReference<TM, T0> where TE : UnityEvent<T0>, new() where TM : GameMessage<T0>, new()
    {
        [HorizontalGroup("Settings", 0.3f), HideLabel]
        public ReceiverStyles style;
        [HorizontalGroup("Settings"), HideLabel]
        public TR reference;
        [FoldoutGroup("Response"), HideLabel]
        public TE @event;
        
        private void Awake()
        {
            if (style == ReceiverStyles.AwakeDestroy) reference.message.On += OnEvent;
        }

        private void OnEnable()
        {
            if (style == ReceiverStyles.EnableDisable) reference.message.On += OnEvent;
        }

        private void Start()
        {
            if (style == ReceiverStyles.StartDestroy) reference.message.On += OnEvent;
        }

        private void OnDisable()
        {
            if (style == ReceiverStyles.EnableDisable) reference.message.On -= OnEvent;
        }

        private void OnDestroy()
        {
            if (style == ReceiverStyles.AwakeDestroy) reference.message.On -= OnEvent;
            if (style == ReceiverStyles.StartDestroy) reference.message.On += OnEvent;
        }
        
        private void OnEvent(T0 arg0)
        {
            @event?.Invoke(arg0);
        }
    }
    
    [HideMonoScript]
    public abstract class GameMessageReceiver<TR, TE, TM, T0, T1> : MonoBehaviour where TR : GameMessageReference<TM, T0, T1> where TE : UnityEvent<T0, T1>, new() where TM : GameMessage<T0, T1>, new()
    {
        [HorizontalGroup("Settings", 0.3f), HideLabel]
        public ReceiverStyles style;
        [HorizontalGroup("Settings"), HideLabel]
        public TR reference;
        [FoldoutGroup("Response"), HideLabel]
        public TE @event;
        
        private void Awake()
        {
            if (style == ReceiverStyles.AwakeDestroy) reference.message.On += OnEvent;
        }

        private void OnEnable()
        {
            if (style == ReceiverStyles.EnableDisable) reference.message.On += OnEvent;
        }

        private void Start()
        {
            if (style == ReceiverStyles.StartDestroy) reference.message.On += OnEvent;
        }

        private void OnDisable()
        {
            if (style == ReceiverStyles.EnableDisable) reference.message.On -= OnEvent;
        }

        private void OnDestroy()
        {
            if (style == ReceiverStyles.AwakeDestroy) reference.message.On -= OnEvent;
            if (style == ReceiverStyles.StartDestroy) reference.message.On += OnEvent;
        }
        
        private void OnEvent(T0 arg0, T1 arg1)
        {
            @event?.Invoke(arg0, arg1);
        }
    }
    
    [HideMonoScript]
    public abstract class GameMessageReceiver<TR, TE, TM, T0, T1, T2> : MonoBehaviour where TR : GameMessageReference<TM, T0, T1, T2> where TE : UnityEvent<T0, T1, T2>, new() where TM : GameMessage<T0, T1, T2>, new()
    {
        [HorizontalGroup("Settings", 0.3f), HideLabel]
        public ReceiverStyles style;
        [HorizontalGroup("Settings"), HideLabel]
        public TR reference;
        [FoldoutGroup("Response"), HideLabel]
        public TE @event;
        
        private void Awake()
        {
            if (style == ReceiverStyles.AwakeDestroy) reference.message.On += OnEvent;
        }

        private void OnEnable()
        {
            if (style == ReceiverStyles.EnableDisable) reference.message.On += OnEvent;
        }

        private void Start()
        {
            if (style == ReceiverStyles.StartDestroy) reference.message.On += OnEvent;
        }

        private void OnDisable()
        {
            if (style == ReceiverStyles.EnableDisable) reference.message.On -= OnEvent;
        }

        private void OnDestroy()
        {
            if (style == ReceiverStyles.AwakeDestroy) reference.message.On -= OnEvent;
            if (style == ReceiverStyles.StartDestroy) reference.message.On += OnEvent;
        }
        
        private void OnEvent(T0 arg0, T1 arg1, T2 arg2)
        {
            @event?.Invoke(arg0, arg1, arg2);
        }
    }
    
    [HideMonoScript]
    public abstract class GameMessageReceiver<TR, TE, TM, T0, T1, T2, T3> : MonoBehaviour where TR : GameMessageReference<TM, T0, T1, T2, T3> where TE : UnityEvent<T0, T1, T2, T3>, new() where TM : GameMessage<T0, T1, T2, T3>, new()
    {
        [HorizontalGroup("Settings", 0.3f), HideLabel]
        public ReceiverStyles style;
        [HorizontalGroup("Settings"), HideLabel]
        public TR reference;
        [FoldoutGroup("Response"), HideLabel]
        public TE @event;
        
        private void Awake()
        {
            if (style == ReceiverStyles.AwakeDestroy) reference.message.On += OnEvent;
        }

        private void OnEnable()
        {
            if (style == ReceiverStyles.EnableDisable) reference.message.On += OnEvent;
        }

        private void Start()
        {
            if (style == ReceiverStyles.StartDestroy) reference.message.On += OnEvent;
        }

        private void OnDisable()
        {
            if (style == ReceiverStyles.EnableDisable) reference.message.On -= OnEvent;
        }

        private void OnDestroy()
        {
            if (style == ReceiverStyles.AwakeDestroy) reference.message.On -= OnEvent;
            if (style == ReceiverStyles.StartDestroy) reference.message.On += OnEvent;
        }
        
        private void OnEvent(T0 arg0, T1 arg1, T2 arg2, T3 arg3)
        {
            @event?.Invoke(arg0, arg1, arg2, arg3);
        }
    }
}