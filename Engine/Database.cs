using Sirenix.Serialization;
using Sirenix.Utilities;
using UnityEngine;

namespace RedOwl.Core
{
    /*
    [HideMonoScript]
    [GlobalConfig("Game/Resources/MyTest", UseAsset = true)]
    public class MyTestDatabase : Database<MyTestDatabase>
    {
        public float number;
    }
    */
    
    public interface IDatabase { }

    public abstract class Database<T> : GlobalConfig<T>, IDatabase where T : Database<T>, new() { }

#if UNITY_EDITOR

    [UnityEditor.InitializeOnLoad]
    public class DatabaseInitializer
    {
        static DatabaseInitializer()
        {
            foreach (var type in AssemblyUtilities.GetTypes(AssemblyTypeFlags.All))
            {
                if (!typeof(IDatabase).IsAssignableFrom(type) || type.IsAbstract) continue;
                var info = typeof(GlobalConfig<>).MakeGenericType(type).GetProperty("Instance",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                if (info?.GetValue(null, null) == null) return;
            }
        }
    }

#endif

}