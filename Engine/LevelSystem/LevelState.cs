using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RedOwl.Core
{
    [Flags]
    public enum LevelStates : int
    {
        None = 0,
        L0 = 1 << 0,
        L1 = 1 << 1,
        L2 = 1 << 2,
        L3 = 1 << 3,
        L4 = 1 << 4,
        L5 = 1 << 5,
        L6 = 1 << 6,
        L7 = 1 << 7,
        L8 = 1 << 8,
        L9 = 1 << 9,
        L10 = 1 << 10,
        L11 = 1 << 11,
        L12 = 1 << 12,
        L13 = 1 << 13,
        L14 = 1 << 14,
        L15 = 1 << 15,
        L16 = 1 << 16,
        L17 = 1 << 17,
        L18 = 1 << 18,
        L19 = 1 << 19,
        L20 = 1 << 20,
        L21 = 1 << 21,
        L22 = 1 << 22,
        L23 = 1 << 23,
        L24 = 1 << 24,
        L25 = 1 << 25,
        L26 = 1 << 26,
        L27 = 1 << 27,
        L28 = 1 << 28,
        L29 = 1 << 29,
        L30 = 1 << 30,
        All = int.MaxValue,
    }
    
    [HideMonoScript]
    public class LevelState : IndexedBehaviour<LevelState>
    {
        public bool ensureDisabled = true;
        
        [NonSerialized, ShowInInspector, DisableInPlayMode]
        [OnValueChanged("TestState"), EnumToggleButtons, HideLabel]
        private LevelStates state;
        

#if UNITY_EDITOR
        private void OnValidate()
        {
            UnityEditor.SceneManagement.EditorSceneManager.sceneSaving -= EnsureDisabled;
            UnityEditor.SceneManagement.EditorSceneManager.sceneSaving += EnsureDisabled;
        }

        private void EnsureDisabled(Scene scene, string path)
        {
            if (!ensureDisabled) return;
            if (this == null) return;
            ApplyState(LevelStates.None);
        }
        
        private void TestState()
        {
            ApplyState(state);
        }
#endif

        public void ApplyState(LevelStates value)
        {
            state = value;
            gameObject.Children(c => c.SetActive(value.HasFlag((LevelStates) Enum.Parse(typeof(LevelStates), c.name))));
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        public static void Initialize()
        {
            LevelManager.OnLoaded += OnLoaded;
        }

        private static void OnLoaded(GameLevel level)
        {
            foreach (var component in All)
            {
                component.ApplyState(level.state);
            }
        }
    }
}