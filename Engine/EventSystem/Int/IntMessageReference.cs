using UnityEngine;

namespace RedOwl.Engine
{
    [CreateAssetMenu(menuName = "Red Owl/Messages/Int", fileName = "Int Message")]
    public class IntMessageReference : GameMessageReference<IntMessage, int> { }
}