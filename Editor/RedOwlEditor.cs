using System;
using UnityEditor;

namespace RedOwl.Editor
{
    public static partial class RedOwlEditor
    {
        public static void Progress(string title, string body, params Action[] callbacks)
        {
            float steps = callbacks.Length;
            EditorUtility.DisplayProgressBar(title, body, 0);
            try
            {
                for (int i = 0; i < callbacks.Length; i++)
                {
                    callbacks[i]();
                    EditorUtility.DisplayProgressBar($"{title} ({i} / {steps})", body, i/steps);
                }
            }
            catch (Exception)
            {
                // ignored
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }
        
        public static void Progress<TItem>(string title, string body, TItem[] items, Action<TItem> callback)
        {
            float steps = items.Length;
            EditorUtility.DisplayProgressBar(title, body, 0);
            try
            {
                for (int i = 0; i < steps; i++)
                {
                    callback(items[i]);
                    EditorUtility.DisplayProgressBar($"{title} ({i} / {steps})", body, i/steps);
                }
            }
            catch (Exception)
            {
                // ignored
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }
    }
}