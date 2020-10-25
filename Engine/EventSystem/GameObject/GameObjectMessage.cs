using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace RedOwl.Engine
{
    [Serializable]
    public class GameObjectEvent : UnityEvent<GameObject> {}
    
    [Serializable, InlineProperty]
    public class GameObjectMessage : GameMessage<GameObject>, IMessage {}
}