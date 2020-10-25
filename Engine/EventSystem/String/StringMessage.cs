using System;
using Sirenix.OdinInspector;
using UnityEngine.Events;

namespace RedOwl.Engine
{
    [Serializable]
    public class StringEvent : UnityEvent<string> {}
    
    [Serializable, InlineProperty]
    public class StringMessage : GameMessage<string>, IMessage {}
}