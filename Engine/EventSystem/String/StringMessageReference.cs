using UnityEngine;

namespace RedOwl.Engine
{
    [CreateAssetMenu(menuName = "Red Owl/Messages/String", fileName = "String Message")]
    public class StringMessageReference : GameMessageReference<StringMessage, string> { }
}