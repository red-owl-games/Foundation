using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Core
{
    public enum CameraSplitOptions
    {
        Rows,
        Columns,
        Special
    }
    
    public partial class RedOwlSettings
    {
        [BoxGroup("Player Camera")]
        [SerializeField, LabelText("Split Options"), Tooltip("How to split the camera screen space for 2 or 3 local players")]
        private CameraSplitOptions playerCameraSplitOption;

        public static CameraSplitOptions PlayerCameraSplitOption => I.playerCameraSplitOption;
        
        [BoxGroup("Player Camera")]
        [SerializeField, LabelText("Lerp Duration"), Tooltip("The amount of time for the cameras to lerp into position")]
        private float playerCameraLerpDuration;

        public static float PlayerCameraLerpDuration => I.playerCameraLerpDuration;
    }
    
    [RequireComponent(typeof(Camera))]
    public class CameraSplitManager : MonoBehaviour
    {
        private static readonly List<CameraSplitManager> All = new List<CameraSplitManager>();

        private Camera cam;
        
        private void Awake()
        {
            cam = GetComponent<Camera>();
            cam.rect = new Rect(0.5f,0.5f,0,0);
        }

        private void OnEnable()
        {
            All.Add(this);
            UpdateCameraRects();
        }

        private void OnDisable()
        {
            All.Remove(this);
            UpdateCameraRects();
        }

        private static void UpdateCameraRects()
        {
            var duration = RedOwlSettings.PlayerCameraLerpDuration;
            switch (All.Count)
            {
                case 1:
                    All[0].cam.DORect(new Rect(0, 0, 1f, 1f), duration);
                    break;
                case 2:
                    switch (RedOwlSettings.PlayerCameraSplitOption)
                    {
                        case CameraSplitOptions.Rows:
                            All[0].cam.DORect(new Rect(0, 0.501f, 1, 0.499f), duration);
                            All[1].cam.DORect(new Rect(0, 0f, 1, 0.499f), duration);
                            break;
                        case CameraSplitOptions.Columns:
                            All[0].cam.DORect(new Rect(0, 0, 0.499f, 1), duration);
                            All[1].cam.DORect(new Rect(0.501f, 0f, 0.499f, 1), duration);
                            break;
                        case CameraSplitOptions.Special:
                            All[0].cam.DORect(new Rect(0, 0.501f, 1, 0.499f), duration);
                            All[1].cam.DORect(new Rect(0, 0f, 1, 0.499f), duration);
                            break;
                    }

                    break;
                case 3:
                    switch (RedOwlSettings.PlayerCameraSplitOption)
                    {
                        case CameraSplitOptions.Rows:
                            All[0].cam.DORect(new Rect(0, 0.668f, 1, 0.331f), duration);
                            All[1].cam.DORect(new Rect(0, 0.334f, 1, 0.332f), duration);
                            All[2].cam.DORect(new Rect(0, 0, 1, 0.332f), duration);
                            break;
                        case CameraSplitOptions.Columns:
                            All[0].cam.DORect(new Rect(0, 0, 0.332f, 1), duration);
                            All[1].cam.DORect(new Rect(0.334f, 0, 0.332f, 1), duration);
                            All[2].cam.DORect(new Rect(0.668f, 0, 0.331f, 1), duration);
                            break;
                        case CameraSplitOptions.Special:
                            All[0].cam.DORect(new Rect(0, 0.501f, 0.499f, 0.499f), duration);
                            All[1].cam.DORect(new Rect(0.501f, 0.501f, 0.499f, 0.499f), duration);
                            All[2].cam.DORect(new Rect(0.25f, 0, 0.499f, 0.499f), duration);
                            break;
                    }

                    break;
                case 4:
                    All[0].cam.DORect(new Rect(0, 0.501f, 0.499f, 0.499f), duration);
                    All[1].cam.DORect(new Rect(0.501f, 0.501f, 0.499f, 0.499f), duration);
                    All[2].cam.DORect(new Rect(0, 0, 0.499f, 0.499f), duration);
                    All[3].cam.DORect(new Rect(0.501f, 0, 0.499f, 0.499f), duration);
                    break;
            }
        }
    }
}