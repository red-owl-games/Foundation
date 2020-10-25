using System;
using Sirenix.OdinInspector;
using UnityEngine.Events;

namespace RedOwl.Engine
{
    [Serializable]
    public class IntEvent : UnityEvent<int> {}
    
    [Serializable, InlineProperty]
    public class IntMessage : GameMessage<int>, IMessage {}
}