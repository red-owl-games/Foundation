using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace RedOwl.Engine
{
    public class SceneMessage : MessageBase<SceneMetadata> {}
    
    [CreateAssetMenu(menuName = Telegraph.MENU_PATH + "Scene", fileName = "Scene Channel")]
    public class SceneChannel : ChannelBase<SceneMessage, SceneMetadata> {}
}