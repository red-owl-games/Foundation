using System;
using Sirenix.OdinInspector;
using UnityEngine.Events;

namespace RedOwl.Engine
{
    [Serializable]
    public class FloatEvent : UnityEvent<float> {}
    
    [Serializable, InlineProperty]
    public class FloatMessage : GameMessage<float>, IMessage {}
}