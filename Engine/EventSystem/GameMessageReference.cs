using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace RedOwl.Engine
{
    public interface IMessage
    {
        event UnityAction OnAny;
    }
    
    [Serializable]
    public class GameEvent : UnityEvent {}

    [Serializable, InlineProperty]
    public class GameMessage : IMessage
    {
        public event UnityAction OnAny;
        public event UnityAction On; 

        [Button(ButtonSizes.Medium)]
        public void Fire()
        {
            OnAny?.Invoke();
            On?.Invoke();
        }
    }
    
    [Serializable, InlineProperty]
    public abstract class GameMessage<T0> : IMessage
    {
        public event UnityAction OnAny;
        public event UnityAction<T0> On; 

        [Button]
        public void Fire(T0 arg0)
        {
            OnAny?.Invoke();
            On?.Invoke(arg0);
        }
    }
    
    [Serializable, InlineProperty]
    public abstract class GameMessage<T0, T1> : IMessage
    {
        public event UnityAction OnAny;
        public event UnityAction<T0, T1> On; 

        [Button]
        public void Fire(T0 arg0, T1 arg1)
        {
            OnAny?.Invoke();
            On?.Invoke(arg0, arg1);
        }
    }
    
    [Serializable, InlineProperty]
    public abstract class GameMessage<T0, T1, T2> : IMessage
    {
        public event UnityAction OnAny;
        public event UnityAction<T0, T1, T2> On; 

        [Button]
        public void Fire(T0 arg0, T1 arg1, T2 arg2)
        {
            OnAny?.Invoke();
            On?.Invoke(arg0, arg1, arg2);
        }
    }
    
    [Serializable, InlineProperty]
    public abstract class GameMessage<T0, T1, T2, T3> : IMessage
    {
        public event UnityAction OnAny;
        public event UnityAction<T0, T1, T2, T3> On; 

        [Button]
        public void Fire(T0 arg0, T1 arg1, T2 arg2, T3 arg3)
        {
            OnAny?.Invoke();
            On?.Invoke(arg0, arg1, arg2, arg3);
        }
    }
    
    [InlineEditor(InlineEditorObjectFieldModes.Foldout)]
    [CreateAssetMenu(menuName = "Red Owl/Messages/Generic", fileName = "Message")]
    public class GameMessageReference : Asset
    {
        [HideLabel]
        public readonly GameMessage message = new GameMessage();
        
        public void Fire() => message.Fire();
    }
    
    [InlineEditor(InlineEditorObjectFieldModes.Foldout)]
    public abstract class GameMessageReference<T, T0> : Asset where T : GameMessage<T0>, new()
    {
        [HideLabel]
        public readonly T message = new T();
        
        public void Fire(T0 arg0) => message.Fire(arg0);
    }
    
    [InlineEditor(InlineEditorObjectFieldModes.Foldout)]
    public abstract class GameMessageReference<T, T0, T1> : Asset where T : GameMessage<T0, T1>, new()
    {
        [HideLabel]
        public readonly T message = new T();
        
        public void Fire(T0 arg0, T1 arg1) => message.Fire(arg0, arg1);
    }
    
    [InlineEditor(InlineEditorObjectFieldModes.Foldout)]
    public abstract class GameMessageReference<T, T0, T1, T2> : Asset where T : GameMessage<T0, T1, T2>, new()
    {
        [HideLabel]
        public readonly T message = new T();
        
        public void Fire(T0 arg0, T1 arg1, T2 arg2) => message.Fire(arg0, arg1, arg2);
    }
    
    [InlineEditor(InlineEditorObjectFieldModes.Foldout)]
    public abstract class GameMessageReference<T, T0, T1, T2, T3> : Asset where T : GameMessage<T0, T1, T2, T3>, new()
    {
        [HideLabel]
        public readonly T message = new T();
        
        public void Fire(T0 arg0, T1 arg1, T2 arg2, T3 arg3) => message.Fire(arg0, arg1, arg2, arg3);
    }
}