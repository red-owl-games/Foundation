using System;
using Sirenix.OdinInspector;

namespace RedOwl.Core
{
    [HideReferenceObjectPicker, InlineProperty, HideLabel]
    public class Message
    {
        private string _name;
        
        public Message(string name)
        {
            _name = name.Prettify();
        }
        
        public event Action On;
        
        [Button("@_name", ButtonSizes.Medium)]
        public void Raise() => On?.Invoke();
    }
    
    [HideReferenceObjectPicker, InlineProperty, HideLabel]
    public class Message<T1>
    {
        private string _name;
        public Message(string name)
        {
            _name = name.Prettify();
        }

        public event Action<T1> On;
        
        [Button("@_name", ButtonSizes.Medium, ButtonStyle.FoldoutButton)]
        public void Raise([HideLabel] T1 p1) => On?.Invoke(p1);
    }
    
    [HideReferenceObjectPicker, InlineProperty, HideLabel]
    public class Message<T1, T2>
    {
        private string _name;
        
        public Message(string name)
        {
            _name = name.Prettify();
        }
        
        public event Action<T1, T2> On;
        
        [Button("@_name", ButtonSizes.Medium, ButtonStyle.FoldoutButton)]
        public void Raise([HideLabel] T1 p1, [HideLabel] T2 p2) => On?.Invoke(p1, p2);
    }
}