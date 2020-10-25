using UnityEngine;

namespace RedOwl.Engine
{
    [CreateAssetMenu(menuName = "Red Owl/Messages/GameObject", fileName = "GameObject Message")]
    public class GameObjectMessageReference : GameMessageReference<GameObjectMessage, GameObject> { }
}