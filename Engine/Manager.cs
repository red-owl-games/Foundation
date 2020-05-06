using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Core
{
    public interface IManagerHideFlags
    {
        HideFlags HideFlags { get; }
    }
    
    /// Initialize your Manager with
    /*
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init() => Initialize();
    */
    [HideMonoScript]
    public class Manager<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance {
            get {
                if (_instance == null)
                {
                    _instance = new GameObject($"{typeof(T).Name}").AddComponent<T>();
                    if (_instance is IManagerHideFlags flags)
                        _instance.gameObject.hideFlags = flags.HideFlags;
                }

                return _instance;
            }
        }
        
        protected static void Initialize() { _instance = Instance; }
    }
}