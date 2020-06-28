using System.Collections.Generic;
using UnityEngine;

namespace RedOwl.Core
{
    public abstract class RedOwlAsset : ScriptableObject
    {
        public abstract void ProcessData(List<List<string>> data);
    }
}