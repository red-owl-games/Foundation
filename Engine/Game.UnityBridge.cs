using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RedOwl.Engine
{
    public static partial class Game
    {
        private static UnityBridge _unityBridge;

        public static UnityBridge UnityBridge
        {
            get
            {
                if (_unityBridge == null)
                {
                    var go = new GameObject("Game") {hideFlags = HideFlags.DontSave};
                    _unityBridge = go.AddComponent<UnityBridge>();
                    if (IsRunning) Object.DontDestroyOnLoad(go);
                }

                return _unityBridge;
            }
        }

        // TODO: this should not use dotween so we can control the timestep
        // Maybe a generated coroutine function that can wait for the delay amount
        public static void DelayedCall(Action callback, float delay = 0.001f)
        {
            if (IsRunning)
            {
                DOVirtual.DelayedCall(delay, () => callback());
            }
            else
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.delayCall += () => callback();
#endif
            }
        }
        
        public static Coroutine StartRoutine(IEnumerator wrapper)
        {
            return UnityBridge.StartCoroutine(wrapper);
        }

        public static void StopRoutine(Coroutine routine)
        {
            if (routine != null) UnityBridge.StopCoroutine(routine);
        }
        
        public static void StopAllRoutines()
        {
            UnityBridge.StopAllCoroutines();
        }
    }
}