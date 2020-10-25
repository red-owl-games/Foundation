using Unity.Mathematics;
using UnityEngine;

namespace RedOwl.Engine
{
    [CreateAssetMenu(menuName = "Red Owl/Messages/Float3", fileName = "Float3 Message")]
    public class Float3MessageReference : GameMessageReference<Float3Message, float3> { }
}