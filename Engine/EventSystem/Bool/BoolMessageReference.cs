using UnityEngine;

namespace RedOwl.Engine
{
    [CreateAssetMenu(menuName = "Red Owl/Messages/Bool", fileName = "Bool Message")]
    public class BoolMessageReference : GameMessageReference<BoolMessage, bool> { }
}