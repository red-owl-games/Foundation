using UnityEngine;

namespace RedOwl.Engine
{
    public class FloatMessage : MessageBase<float> {}
    
    [CreateAssetMenu(menuName = Telegraph.MENU_PATH + "Float", fileName = "Float Channel")]
    public class FloatChannel : ChannelBase<FloatMessage, float> {}
}