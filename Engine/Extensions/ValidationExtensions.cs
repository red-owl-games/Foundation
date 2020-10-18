using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace RedOwl.Core
{
    public static class Validation
    {
        [Conditional("UNITY_EDITOR")]
        public static void Requires<TSource, TComponent>(this TSource obj, TComponent component) where TSource : Object where TComponent : Object
        {
            if (component != null) return;
            
            Debug.LogFormat(LogType.Error, LogOption.NoStacktrace, obj,
                $"<color=yellow>MISSING</color> - <color=blue>{typeof(TComponent)}</color> - on {obj}");

            var mb = obj as MonoBehaviour;
            if (mb != null) mb.enabled = false;
        }
    }
}