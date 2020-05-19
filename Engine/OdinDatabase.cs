using Sirenix.Serialization;
using Sirenix.Utilities;
using UnityEngine;

namespace RedOwl.Core
{
    /*
    [HideMonoScript]
    [ShowOdinSerializedPropertiesInInspector]
    [GlobalConfig("Assets/Game/Resources/MyTest", UseAsset = true)]
    public class MyTestDatabase : OdinDatabase<MyTestDatabase>
    {
        public HashSet<string> strings;
        public Dictionary<string, HashSet<string>> tags;
    }
    */
    
    public interface IOdinDatabase {}
    
    public abstract class OdinDatabase<T> : GlobalConfig<T>, IOdinDatabase, ISerializationCallbackReceiver where T : OdinDatabase<T>, new()
    {
        public static T I { get; private set; }
        
        private void Awake()
        {
            I = (T)this;
        }

        private void OnEnable()
        {
            if (I == null) I = (T)this;
        }
        
        [SerializeField, HideInInspector] 
        private SerializationData serializationData;
        
        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            UnitySerializationUtility.SerializeUnityObject(this, ref serializationData);
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            UnitySerializationUtility.DeserializeUnityObject(this, ref serializationData);
        }
    }

#if UNITY_EDITOR
    
    [UnityEditor.InitializeOnLoad]
    public class OdinDatabaseInitializer
    {
        static OdinDatabaseInitializer()
        {
            foreach (var type in AssemblyUtilities.GetTypes(AssemblyTypeFlags.All))
            {
                if (!typeof(IOdinDatabase).IsAssignableFrom(type) || type.IsAbstract) continue;
                var info = typeof(GlobalConfig<>).MakeGenericType(type).GetProperty("Instance", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                if (info?.GetValue(null, null) == null) return;
            }
        }
    }
    
#endif
}