using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace RedOwl.Core
{
    [HideMonoScript]
    public class ConnectionTarget : MonoBehaviour, IConnectionTarget
    {
        [SerializeField] 
        private List<GameObject> sources;

        public bool HasSources => sources.Count > 0;

        public UnityEvent onTrue = new UnityEvent();
        public UnityEvent onFalse = new UnityEvent();
        public BoolEvent onTriggered = new BoolEvent();

        private bool _lastState;
        private List<IConnectionSource> _sources;

        private void Awake()
        {
            _sources = new List<IConnectionSource>();
            foreach (var source in sources)
            {
                _sources.Add(source.GetComponent<IConnectionSource>());
            }
        }

        private void OnEnable()
        {
            foreach (var source in _sources)
            {
                source.ConnectionTriggered += HandleConnectionTriggered;
            }
        }

        private void OnDisable()
        {
            foreach (var source in _sources)
            {
                source.ConnectionTriggered -= HandleConnectionTriggered;
            }
        }

        private void HandleConnectionTriggered()
        {
            bool state = _sources.Any(s => s.ConnectionState);

            if (state != _lastState)
            {
                _lastState = state;
                if (_lastState)
                {
                    onTrue.Invoke();
                }
                else
                {
                    onFalse.Invoke();
                }
            }
            onTriggered.Invoke(state);
        }

        public void RegisterSource(IConnectionSource source)
        {
            if (sources == null) sources = new List<GameObject>();
            sources.Add(source.gameObject);
        }
    }
}