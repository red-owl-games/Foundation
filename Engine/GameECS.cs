using System.Collections.Generic;
using Unity.Entities;

namespace RedOwl.Engine
{
#if UNITY_EDITOR
    [UnityEditor.InitializeOnLoad]
#endif
    public class DotsInit
    {
#if UNITY_EDITOR
        static DotsInit()
        {
            Game.Initialize();
        }
#endif
    }
    
    public partial class Game
    {
        private static bool _ecsInitialized;
        private static List<ComponentSystemBase> _ecsSystems;

        private static void DotsInit()
        {
            _ecsSystems = new List<ComponentSystemBase>();
#if UNITY_EDITOR
            if (!IsRunning || IsShuttingDown) DefaultWorldInitialization.DefaultLazyEditModeInitialize();
            UnityEditor.EditorApplication.update -= DotsUpdate;
            UnityEditor.EditorApplication.update += DotsUpdate;
#endif
            _ecsInitialized = true;
        }
        
        private static void DotsUpdate()
        {
            if (!_ecsInitialized) return;
            foreach (var system in _ecsSystems)
            {
                system.Update();
            }
        }
        
        public static void AddSystemForUpdate(ComponentSystemBase system)
        {
            if (!Game.IsRunning) _ecsSystems.Add(system);
        }
    }
}