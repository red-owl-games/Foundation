using UnityEngine;

namespace RedOwl.Engine
{
    public class StringMessage : MessageBase<string> {}
    
    [CreateAssetMenu(menuName = Telegraph.MENU_PATH + "String", fileName = "String Channel")]
    public class StringChannel : ChannelBase<StringMessage, string> {}
}