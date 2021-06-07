using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    public abstract class ChannelBase : RedOwlScriptableObject {}
    
    public abstract class ChannelBase<T1> : ChannelBase where T1 : class, IMessageBase, new()
    {
        [SerializeField, TelegraphReference, OnValueChanged("Rename")] private string @event;
        
        public event Action On
        {
            add => Telegraph.Get<T1>(@event).On += value;
            remove => Telegraph.Get<T1>(@event).On -= value;
        }

        [Button, ResponsiveButtonGroup("Buttons")]
        public void Raise()
        {
            Telegraph.Get<T1>(@event).Raise();
        }
        
        [Button, ResponsiveButtonGroup("Buttons")]
        public void RaiseAsync()
        {
            Telegraph.Get<T1>(@event).RaiseAsync();
        }

        private void Rename() => Telegraph.RenameChannel(this, @event);

    }
    
    public abstract class ChannelBase<T1, P1> : ChannelBase where T1 : class, IMessageBase<P1>, new()
    {
        [SerializeField, TelegraphReference, OnValueChanged("Rename")] private string @event;
        
        public event Action<P1> On
        {
            add => Telegraph.Get<T1>(@event).On += value;
            remove => Telegraph.Get<T1>(@event).On -= value;
        }

        [Button, ResponsiveButtonGroup("Buttons")]
        public void Raise(P1 p1)
        {
            Telegraph.Get<T1>(@event).Raise(p1);
        }
        
        [Button, ResponsiveButtonGroup("Buttons")]
        public void RaiseAsync(P1 p1)
        {
            Telegraph.Get<T1>(@event).RaiseAsync(p1);
        }
        
        private void Rename() => Telegraph.RenameChannel(this, @event);
        
    }
    
    public abstract class ChannelBase<T1, P1, P2> : ChannelBase where T1 : class, IMessageBase<P1, P2>, new()
    {
        [SerializeField, TelegraphReference, OnValueChanged("Rename")] private string @event;
        
        public event Action<P1, P2> On
        {
            add => Telegraph.Get<T1>(@event).On += value;
            remove => Telegraph.Get<T1>(@event).On -= value;
        }

        [Button, ResponsiveButtonGroup("Buttons")]
        public void Raise(P1 p1, P2 p2)
        {
            Telegraph.Get<T1>(@event).Raise(p1, p2);
        }
        
        [Button, ResponsiveButtonGroup("Buttons")]
        public void RaiseAsync(P1 p1, P2 p2)
        {
            Telegraph.Get<T1>(@event).RaiseAsync(p1, p2);
        }
        
        private void Rename() => Telegraph.RenameChannel(this, @event);
        
    }
    
    public abstract class ChannelBase<T1, P1, P2, P3> : ChannelBase where T1 : class, IMessageBase<P1, P2, P3>, new()
    {
        [SerializeField, TelegraphReference, OnValueChanged("Rename")] private string @event;
        
        public event Action<P1, P2, P3> On
        {
            add => Telegraph.Get<T1>(@event).On += value;
            remove => Telegraph.Get<T1>(@event).On -= value;
        }

        [Button, ResponsiveButtonGroup("Buttons")]
        public void Raise(P1 p1, P2 p2, P3 p3)
        {
            Telegraph.Get<T1>(@event).Raise(p1, p2, p3);
        }
        
        [Button, ResponsiveButtonGroup("Buttons")]
        public void RaiseAsync(P1 p1, P2 p2, P3 p3)
        {
            Telegraph.Get<T1>(@event).RaiseAsync(p1, p2, p3);
        }
        
        private void Rename() => Telegraph.RenameChannel(this, @event);
        
    }
    
    public abstract class ChannelBase<T1, P1, P2, P3, P4> : ChannelBase where T1 : class, IMessageBase<P1, P2, P3, P4>, new()
    {
        [SerializeField, TelegraphReference, OnValueChanged("Rename")] private string @event;
        
        public event Action<P1, P2, P3, P4> On
        {
            add => Telegraph.Get<T1>(@event).On += value;
            remove => Telegraph.Get<T1>(@event).On -= value;
        }

        [Button, ResponsiveButtonGroup("Buttons")]
        public void Raise(P1 p1, P2 p2, P3 p3, P4 p4)
        {
            Telegraph.Get<T1>(@event).Raise(p1, p2, p3, p4);
        }
        
        [Button, ResponsiveButtonGroup("Buttons")]
        public void RaiseAsync(P1 p1, P2 p2, P3 p3, P4 p4)
        {
            Telegraph.Get<T1>(@event).RaiseAsync(p1, p2, p3, p4);
        }
        
        private void Rename() => Telegraph.RenameChannel(this, @event);
        
    }

    [CreateAssetMenu(menuName = Telegraph.MENU_PATH + "Void", fileName = "Void Channel")]
    public class Channel : ChannelBase<Message> {}
}