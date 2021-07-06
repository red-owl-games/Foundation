using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    public static class AddressableDatabase
    {
        public static Dictionary<Type, IList<string>> TypeTable;
        public static Dictionary<string, IList<string>> AddressTable;

        public static void Add<T>(string guid, IEnumerable<string> keys) where T : UnityEngine.Object
        {
            if (TypeTable == null) TypeTable = new Dictionary<Type, IList<string>>();
            if (AddressTable == null) AddressTable = new Dictionary<string, IList<string>>();
            var type = typeof(T);
            if (!TypeTable.TryGetValue(type, out var currentGuids))
            {
                currentGuids = new List<string>();
                TypeTable[type] = currentGuids;
            }
            if (!AddressTable.TryGetValue(guid, out var currentAddresses))
            {
                currentAddresses = new List<string>();
                AddressTable[guid] = currentAddresses;
            }
            currentGuids.Add(guid);
            foreach (string key in keys)
            {
                currentAddresses.Add(key);
            }
        }

        public static IEnumerable<KeyValuePair<string, IList<string>>> Get<T>() where T : UnityEngine.Object
        {
            if (!TypeTable.TryGetValue(typeof(T), out var guids)) yield break;
            foreach (string guid in guids)
            {
                if (AddressTable.TryGetValue(guid, out var addresses))
                    yield return new KeyValuePair<string, IList<string>>(guid, addresses);
            }
        }
    }

    public interface IAssetRef
    {
        string Address { get; }
    }
    
    [Serializable, InlineProperty]
    public abstract class AssetRef<T> : IAssetRef where T : UnityEngine.Object
    {
        [SerializeField, HideLabel, ValueDropdown("PossibleScenes")]
        private string address;

        public string Address => address;

        private ValueDropdownList<string> PossibleScenes
        {
            get
            {
                var output = new ValueDropdownList<string>();
#if UNITY_EDITOR
                foreach (var scene in AddressableDatabase.Get<T>())
                {
                    output.Add(scene.Value[0], scene.Value[0]);
                }
#endif
                return output;
            }
        }

        public static implicit operator string(AssetRef<T> self) => self.address;
    }
    
    [Serializable, InlineProperty]
    public class SceneRef : IAssetRef
    {
        [SerializeField, HideLabel, ValueDropdown("PossibleScenes")]
        private string address;

        public string Address => address;
        
        private ValueDropdownList<string> PossibleScenes
        {
            get
            {
                var output = new ValueDropdownList<string>();
#if UNITY_EDITOR
                foreach (var scene in AddressableDatabase.Get<UnityEditor.SceneAsset>())
                {
                    
                    output.Add(scene.Value[0], scene.Value[0]);
                }
#endif
                return output;
            }
        }

        public static implicit operator string(SceneRef self) => self.address;
    }
}
