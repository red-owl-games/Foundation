using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RedOwl.Engine
{
    public enum CameraSplitOptions
    {
        Rows,
        Columns,
        Special,
    }
    
    [RequireComponent(typeof(Camera))]
    public class CameraSplit : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<Camera>().rect = new Rect(0.5f, 0.5f, 0, 0);
        }

        private void OnEnable()
        {
            World.Default.Get<CameraSplitSystem>().Recalculate();
        }

        private void OnDisable()
        {
            World.Default.Get<CameraSplitSystem>().Recalculate();
        }
    }

    public class CameraSplitSystem : SystemBase
    {
        public CameraSplitOptions SplitOption = CameraSplitOptions.Special;
        public float LerpDuration = 0.3f;

        private List<Tweener> _tweeners = new();
        
        protected override void OnStartRunning()
        {
            Recalculate();
        }

        public void Recalculate()
        {
            var all = Object.FindObjectsOfType<CameraSplit>();
            Array.Sort(all, (x, y) => string.Compare(x.gameObject.name, y.gameObject.name, StringComparison.Ordinal));
            int count = all.Length;
            if (count < 1 || count > 4) return;
            foreach (var tweener in _tweeners)
            {
                tweener.Kill();
            }
            _tweeners.Clear();
            var data = _table[count];
            for (int i = 0; i < count; i++)
            {
                _tweeners.Add(all[i].GetComponent<Camera>().DORect(data[SplitOption][i], LerpDuration));
            }
        }
        
        #region StaticData
        private class TableEntry : Dictionary<CameraSplitOptions, List<Rect>> {}
        private class TableData : Dictionary<int, TableEntry> {}
        private static readonly List<Rect> One = new List<Rect> {new Rect(0, 0, 1f, 1f)};
        private static readonly List<Rect> Four = new List<Rect>
        {
            new Rect(0, 0.501f, 0.499f, 0.499f), new Rect(0.501f, 0.501f, 0.499f, 0.499f),
            new Rect(0, 0, 0.499f, 0.499f), new Rect(0.501f, 0, 0.499f, 0.499f)
        };
        private static TableData _table = new TableData
            {
                {
                    1,
                    new TableEntry
                    {
                        {
                            CameraSplitOptions.Rows,
                            One
                        },
                        {
                            CameraSplitOptions.Columns,
                            One
                        },
                        {
                            CameraSplitOptions.Special,
                            One
                        }
                    }
                },
                {
                    2,
                    new TableEntry
                    {
                        {
                            CameraSplitOptions.Rows,
                            new List<Rect> {new Rect(0, 0.501f, 1, 0.499f), new Rect(0, 0f, 1, 0.499f)}
                        },
                        {
                            CameraSplitOptions.Columns,
                            new List<Rect> {new Rect(0, 0, 0.499f, 1), new Rect(0.501f, 0f, 0.499f, 1)}
                        },
                        {
                            CameraSplitOptions.Special,
                            new List<Rect> {new Rect(0, 0.501f, 1, 0.499f), new Rect(0, 0f, 1, 0.499f)}
                        }
                    }
                },
                {
                    3,
                    new TableEntry
                    {
                        {
                            CameraSplitOptions.Rows,
                            new List<Rect> {new Rect(0, 0.668f, 1, 0.331f), new Rect(0, 0.334f, 1, 0.332f),  new Rect(0, 0, 1, 0.332f)}
                        },
                        {
                            CameraSplitOptions.Columns,
                            new List<Rect> {new Rect(0, 0, 0.332f, 1), new Rect(0.334f, 0, 0.332f, 1), new Rect(0.668f, 0, 0.331f, 1)}
                        },
                        {
                            CameraSplitOptions.Special,
                            new List<Rect> {new Rect(0, 0.501f, 0.499f, 0.499f), new Rect(0.501f, 0.501f, 0.499f, 0.499f), new Rect(0.25f, 0, 0.499f, 0.499f)}
                        }
                    }
                },
                {
                    4,
                    new TableEntry
                    {
                        {
                            CameraSplitOptions.Rows,
                            Four
                        },
                        {
                            CameraSplitOptions.Columns,
                            Four
                        },
                        {
                            CameraSplitOptions.Special,
                            Four
                        }
                    }
                }
            };
        
        #endregion
    }
}