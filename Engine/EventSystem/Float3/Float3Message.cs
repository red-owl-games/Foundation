using System;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine.Events;

namespace RedOwl.Engine
{
    [Serializable]
    public class Float3Event : UnityEvent<float3> {}
    
    [Serializable, InlineProperty]
    public class Float3Message : GameMessage<float3>, IMessage {}
}