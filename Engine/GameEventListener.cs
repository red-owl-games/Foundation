using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace RedOwl.Core
{
    [HideMonoScript]
    public class GameEventListener : MonoBehaviour
    {
        public GameEvent Event;
        public UnityEvent Response;

        private void OnEnable()
        {
            Event.On += OnEventRaised;
        }

        private void OnDisable()
        {
            Event.On -= OnEventRaised;
        }

        public void OnEventRaised()
        {
            Response.Invoke();
        }
    }

    [HideMonoScript]
    public abstract class GameEventListener<T1> : MonoBehaviour
    {
        public GameEvent<T1> Event;
        public UnityEvent<T1> Response;

        private void OnEnable()
        {
            Event.On += OnEventRaised;
        }

        private void OnDisable()
        {
            Event.On -= OnEventRaised;
        }

        public void OnEventRaised(T1 t1)
        {
            Response.Invoke(t1);
        }
    }
    
    [HideMonoScript]
    public abstract class GameEventListener<T1, T2> : MonoBehaviour
    {
        public GameEvent<T1, T2> Event;
        public UnityEvent<T1, T2> Response;

        private void OnEnable()
        {
            Event.On += OnEventRaised;
        }

        private void OnDisable()
        {
            Event.On -= OnEventRaised;
        }

        public void OnEventRaised(T1 t1, T2 t2)
        {
            Response.Invoke(t1, t2);
        }
    }
    
    [HideMonoScript]
    public abstract class GameEventListener<T1, T2, T3> : MonoBehaviour
    {
        public GameEvent<T1, T2, T3> Event;
        public UnityEvent<T1, T2, T3> Response;

        private void OnEnable()
        {
            Event.On += OnEventRaised;
        }

        private void OnDisable()
        {
            Event.On -= OnEventRaised;
        }

        public void OnEventRaised(T1 t1, T2 t2, T3 t3)
        {
            Response.Invoke(t1, t2, t3);
        }
    }
    
    [HideMonoScript]
    public abstract class GameEventListener<T1, T2, T3, T4> : MonoBehaviour
    {
        public GameEvent<T1, T2, T3, T4> Event;
        public UnityEvent<T1, T2, T3, T4> Response;

        private void OnEnable()
        {
            Event.On += OnEventRaised;
        }

        private void OnDisable()
        {
            Event.On -= OnEventRaised;
        }

        public void OnEventRaised(T1 t1, T2 t2, T3 t3, T4 t4)
        {
            Response.Invoke(t1, t2, t3, t4);
        }
    }
}
