using UnityEngine;

namespace RedOwl.Engine
{
    public static class RedOwlTools
    {
        public static bool IsRunning => Application.isPlaying;
        
        public static bool IsShuttingDown { get; internal set; }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void BeforeSplashScreen()
        {
            Application.quitting += HandleQuit;
        }

        public static T Create<T>(GameObject parent = null, string name = "", bool selectGameObjectAfterCreation = true) where T : Component
        {
            var type = typeof(T);
            string label = string.IsNullOrEmpty(name) ? type.Name : name;
            GameObject go = new GameObject(label, type);

#if UNITY_EDITOR
            if (parent != null)
            {
                UnityEditor.GameObjectUtility.SetParentAndAlign(go, parent);
                if (go.GetComponent<RectTransform>() != null) go.GetComponent<RectTransform>().Stretch(true);
            }
            UnityEditor.Undo.RegisterCreatedObjectUndo(go, $"Created {label}");
            if (selectGameObjectAfterCreation) UnityEditor.Selection.activeObject = go;
#endif
            return go.GetComponent<T>();
        }

        private static void HandleQuit()
        {
            CoroutineManager.StopAllRoutines();
            IsShuttingDown = true;
        }

        public static void Quit()
        {
            IsShuttingDown = true;
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#elif UNITY_STANDALONE 
            Application.Quit();
#elif UNITY_WEBGL
            Application.OpenURL("about:blank");
#endif
        }
    }
}