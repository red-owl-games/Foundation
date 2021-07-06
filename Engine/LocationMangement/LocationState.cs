using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RedOwl.Engine
{
    [Flags]
    public enum LocationStates : int
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
    public class LocationState : MonoBehaviour
    {
        [ToggleLeft]
        public bool ignoreSceneState = false;
        [ToggleLeft]
        public bool ensureDisabledOnSaveScene = true;
        
        [SerializeField, ShowInInspector, DisableInPlayMode]
        [OnValueChanged("TestState"), EnumToggleButtons, HideLabel]
        private LocationStates state;

        [Inject] private LocationService _controller;

#if UNITY_EDITOR
        private void OnValidate()
        {
            UnityEditor.SceneManagement.EditorSceneManager.sceneSaving -= EnsureDisabled;
            UnityEditor.SceneManagement.EditorSceneManager.sceneSaving += EnsureDisabled;
        }

        private void EnsureDisabled(Scene scene, string path)
        {
            if (!ensureDisabledOnSaveScene) return;
            if (this == null) return;
            ApplyState(LocationStates.None);
        }
#endif
        
        private void TestState()
        {
            // TODO: Ensure Child Named after 'state' ?
            ApplyState(state);
        }

        private void Awake()
        {
            if (!ignoreSceneState) StartCoroutine(WaitForState());
        }

        private IEnumerator WaitForState()
        {
            while (_controller == null)
            {
                yield return null;
                Game.Inject(this);
            }
            ApplyState(_controller.History.Current.state);
        }

        public void ApplyState(LocationStates value)
        {
            state = value;
            gameObject.Children(c => c.SetActive(value.HasFlag((LocationStates) Enum.Parse(typeof(LocationStates), c.name))));
        }
    }
}