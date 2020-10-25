using System;
using Sirenix.OdinInspector;
using UnityEngine.Events;

namespace RedOwl.Engine
{
    [Serializable]
    public class BoolEvent : UnityEvent<bool> {}
    
    [Serializable, InlineProperty]
    public class BoolMessage : GameMessage<bool>, IMessage {}
}