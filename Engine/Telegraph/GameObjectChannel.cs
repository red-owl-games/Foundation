using System;
using UnityEngine;

namespace RedOwl.Engine
{
    public class GameObjectMessage : MessageBase<GameObject> {}
    
    [CreateAssetMenu(menuName = Telegraph.MENU_PATH + "Game Object", fileName = "Game Object Channel")]
    public class GameObjectChannel : ChannelBase<GameObjectMessage, GameObject> {}
}