using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RedOwl.Engine
{
    [Flags]
    public enum SceneStates : int
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
    public class SceneState : MonoBehaviour
    {
        public bool ignoreSceneControllerState = false;
        public bool ensureDisabledOnSaveScene = true;
        
        [SerializeField, ShowInInspector, DisableInPlayMode]
        [OnValueChanged("TestState"), EnumToggleButtons, HideLabel]
        private SceneStates state;

        [Inject] private ISceneController _controller;

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
            ApplyState(SceneStates.None);
        }
#endif
        
        private void TestState()
        {
            ApplyState(state);
        }

        private void Awake()
        {
            if (!ignoreSceneControllerState) StartCoroutine(WaitForState());
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

        public void ApplyState(SceneStates value)
        {
            state = value;
            gameObject.Children(c => c.SetActive(value.HasFlag((SceneStates) Enum.Parse(typeof(SceneStates), c.name))));
        }
    }
}