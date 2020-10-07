using UnityEngine;

namespace RedOwl.Core
{
    public interface ILevelBuilder
    {
        void Build(LevelData levelData, LookupTable lookupTable, Transform root);
    }
    
    public class LevelData : ScriptableObject {}
}