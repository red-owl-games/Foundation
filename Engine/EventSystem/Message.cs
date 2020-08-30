using System;
using Sirenix.OdinInspector;

namespace RedOwl.Core
{
    public interface IMessage
    {
        event Action On;
    }
    
    [HideReferenceObjectPicker, InlineProperty, HideLabel]
    public class Message : IMessage
    {
        private string _name;
        
        public Message(string name)
        {
            _name = name.Prettify();
        }
        
        public event Action On;
        
        [Button("@_name", ButtonSizes.Medium)]
        public void Raise()
        {
            Log.Always($"Raising '{_name}'");
            On?.Invoke();
        }
    }
    
    [HideReferenceObjectPicker, InlineProperty, HideLabel]
    public class Message<T1> : IMessage
    {
        private string _name;
        public Message(string name)
        {
            _name = name.Prettify();
        }

        public event Action On;
        public event Action<T1> OnParams;
        
        [Button("@_name", ButtonSizes.Medium, ButtonStyle.FoldoutButton)]
        public void Raise([HideLabel] T1 p1)
        {
            On?.Invoke();
            OnParams?.Invoke(p1);
        }
    }
    
    [HideReferenceObjectPicker, InlineProperty, HideLabel]
    public class Message<T1, T2> : IMessage
    {
        private string _name;
        
        public Message(string name)
        {
            _name = name.Prettify();
        }

        public event Action On;
        public event Action<T1, T2> OnParams;
        
        [Button("@_name", ButtonSizes.Medium, ButtonStyle.FoldoutButton)]
        public void Raise([HideLabel] T1 p1, [HideLabel] T2 p2)
        {
            On?.Invoke();
            OnParams?.Invoke(p1, p2);
        }
    }
}