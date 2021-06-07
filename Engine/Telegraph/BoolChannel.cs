using UnityEngine;

namespace RedOwl.Engine
{
    public class BoolMessage : MessageBase<bool> {}
    
    [CreateAssetMenu(menuName = Telegraph.MENU_PATH + "Bool", fileName = "Bool Channel")]
    public class BoolChannel : ChannelBase<BoolMessage, bool> {}
}