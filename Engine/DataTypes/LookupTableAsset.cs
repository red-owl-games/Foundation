using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Core
{
    [Serializable]
    [InlineProperty]
    public class LookupTableEntry
    {
        [HorizontalGroup("Config")]
        [HideLabel]
        public string key;
        
        [HorizontalGroup("Config", 0.65f)]
        [AssetsOnly, HideLabel]
        public GameObject value;
    }

    [Serializable]
    public class LookupTable : Dictionary<string, GameObject>
    {
        public LookupTable(IReadOnlyCollection<LookupTableEntry> entries) : base(entries.Count)
        {
            foreach (var entry in entries) Add(entry.key, entry.value);
        }
    }
    
    [HideMonoScript]
    [CreateAssetMenu(fileName = "Lookup Table", menuName = "Red Owl/Lookup Table")]
    public class LookupTableAsset : ScriptableObject
    {
        [ListDrawerSettings(Expanded = true)]
        public List<LookupTableEntry> entries;

        public LookupTable Table => new LookupTable(entries);
    }
}