using UnityEngine;

namespace RedOwl.Engine
{
    public class TransformMessage : MessageBase<Transform> {}
    
    [CreateAssetMenu(menuName = Telegraph.MENU_PATH + "Transform", fileName = "Transform Channel")]
    public class TransformChannel : ChannelBase<TransformMessage, Transform> {}
}