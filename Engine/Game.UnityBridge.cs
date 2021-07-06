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
        
        public static void DelayedCall(Action callback, float delay = 0.001f)
        {
            if (IsRunning)
            {
                StartRoutine(DelayedCallWrapper(callback, delay));
            }
            else
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.delayCall += () => callback();
#endif
            }
        }
        
        private static IEnumerator DelayedCallWrapper(Action callback, float delay)
        {
            yield return new WaitForSeconds(delay);
            callback();
        }

        public static void GuardedCall(Func<bool> guard, Action callback, float sleepInterval = 1f)
        {
            if (IsRunning)
            {
                StartRoutine(GuardedCallWrapper(guard, callback, sleepInterval));
            }
            else
            {
                // TODO: Editor Coroutines
#if UNITY_EDITOR
                throw new Exception("Guarded Call - Not Implemented For Unity Editor");
#endif
            }
        }

        private static IEnumerator GuardedCallWrapper(Func<bool> guard, Action callback, float sleepInterval)
        {
            while (guard() == false)
            {
                yield return new WaitForSeconds(sleepInterval);
            }
            callback();
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