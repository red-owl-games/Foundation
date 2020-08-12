using System;
using System.Collections.Generic;
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
    public class LevelState : MonoBehaviour
    {
        private static readonly List<LevelState> Instances = new List<LevelState>(5);
        
        [NonSerialized, ShowInInspector, DisableInPlayMode]
        [OnValueChanged("TestState"), EnumToggleButtons, HideLabel]
        private LevelStates State;

        private void Awake()
        {
            Instances.Add(this);
        }

        private void OnDestroy()
        {
            Instances.Remove(this);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            UnityEditor.SceneManagement.EditorSceneManager.sceneSaving -= EnsureDisabled;
            UnityEditor.SceneManagement.EditorSceneManager.sceneSaving += EnsureDisabled;
        }

        private void EnsureDisabled(Scene scene, string path)
        {
            ApplyState(LevelStates.None);
        }
#endif

        private void TestState()
        {
            ApplyState(State);
        }

        public void ApplyState(LevelStates state)
        {
            State = state;
            gameObject.Children(c => c.SetActive(state.HasFlag((LevelStates) Enum.Parse(typeof(LevelStates), c.name))));
        }


        public static void SetState(LevelStates state)
        {
            foreach (var component in Instances)
            {
                component.ApplyState(state);
            }
        }
    }
}