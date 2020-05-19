using Sirenix.Serialization;
using Sirenix.Utilities;
using UnityEngine;

namespace RedOwl.Core
{
    /*
    [HideMonoScript]
    [GlobalConfig("Assets/Game/Resources/MyTest", UseAsset = true)]
    public class MyTestDatabase : Database<MyTestDatabase>
    {
        public float number;
    }
    */
    
    public interface IDatabase { }

    public abstract class Database<T> : GlobalConfig<T>, IDatabase where T : Database<T>, new()
    {
        //TODO: The following is a hack for a bug in GlobalConfig not loading the asset at runtime
        public static T I { get; private set; }
        
        private void Awake()
        {
            I = (T)this;
        }

        private void OnEnable()
        {
            if (I == null) I = (T)this;
        }
    }

#if UNITY_EDITOR
    [UnityEditor.InitializeOnLoad]
    public class DatabaseInitializer
    {
        static DatabaseInitializer()
        {
            if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode) return;
            foreach (var type in AssemblyUtilities.GetTypes(AssemblyTypeFlags.All))
            {
                if (!typeof(IDatabase).IsAssignableFrom(type) || type.IsAbstract) continue;
                //Debug.Log($"Database Initialization of type: {type.Name}");
                var info = typeof(GlobalConfig<>).MakeGenericType(type).GetProperty("Instance",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                if (info?.GetValue(null, null) == null) return;
            }
        }
    }
#endif
}