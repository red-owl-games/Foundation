using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    #region Settings
    
    [Serializable, InlineProperty, HideLabel]
    public class DrawSettings
    {
        [ToggleLeft, HorizontalGroup("Draw", 0.3f), LabelWidth(200)]
        public bool ShowDebugDraw = true;
        
        [HorizontalGroup("Draw"), ShowIf("ShowDebugDraw"), HideLabel]
        public Color DrawColor = Color.magenta;
    }
    
    public partial class GameSettings
    {
        [FoldoutGroup("Debug Draw"), SerializeField]
        private DrawSettings drawSettings = new DrawSettings();
        public static DrawSettings DrawSettings => Instance.drawSettings;
    }
    
    #endregion
    
    public static partial class Game
    {
        public static class Tools
        {
            // TODO: use Shapes to draw - https://acegikmo.com/shapes/docs/#immediate-mode
            
            public static bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hit, float distance, LayerMask mask, bool includeTriggers = false)
            {
#if UNITY_EDITOR
                if (GameSettings.DrawSettings.ShowDebugDraw)
                    Debug.DrawRay(origin, direction * distance, GameSettings.DrawSettings.DrawColor);
#endif
                return Physics.Raycast(origin, direction, out hit, distance, mask, includeTriggers ? QueryTriggerInteraction.Collide : QueryTriggerInteraction.Ignore);
            }

            public static int Raycast(Vector3 origin, Vector3 direction, RaycastHit[] results, float distance, LayerMask mask, bool includeTriggers = false)
            {
#if UNITY_EDITOR
                if (GameSettings.DrawSettings.ShowDebugDraw)
                    Debug.DrawRay(origin, direction * distance, GameSettings.DrawSettings.DrawColor);
#endif
                return Physics.RaycastNonAlloc(origin, direction, results, distance, mask, includeTriggers ? QueryTriggerInteraction.Collide : QueryTriggerInteraction.Ignore);
            }
            
            // TODO: add other Casting - IE OverlapBox & BoxCast - OverlapSphere & SphereCast
        }
    }
}