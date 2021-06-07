using System.Collections.Generic;
using UnityEngine;

namespace RedOwl.Engine
{
    // Serves as an Example
    [CreateAssetMenu(menuName = "Red Owl/Pooling/List Int", fileName = "List Int Pool")]
    public class ListIntPool : Pool<List<int>>
    {
        protected override List<int> Create()
        {
            return new List<int>(20);
        }
    }
}