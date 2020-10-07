using System;
using UnityEngine.Events;

namespace RedOwl.Core
{
    [Serializable]
    public class BoolEvent : UnityEvent<bool> {}
    
    [Serializable]
    public class FloatEvent : UnityEvent<float> {}
    
    [Serializable]
    public class StringEvent : UnityEvent<string> {}
}