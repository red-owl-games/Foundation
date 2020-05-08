using System.Collections;
using UnityEngine;

namespace RedOwl.Core
{
    public abstract class State
    {
        public int Id { get; private set; }
        
        public bool Reenterable { get; private set; }

        private bool ShouldStop;

        internal void Initialize(GameObject owner, bool reenterable)
        {
            Id = StateCache.NextId;
            Reenterable = reenterable;
            OnInitialize(owner);
        }

        private IEnumerator InfiniteUpdate()
        {
            while (true)
            {
                yield return OnUpdate();
                yield return null;
                if (ShouldStop)
                {
                    //Log.Always($"Trying to stop state: {GetType().Name} with id: {Id}");
                    break;
                }
            }

            ShouldStop = false;
        }

        internal CoroutineWrapper Enter() => new CoroutineWrapper(OnEnter());
        internal CoroutineWrapper Update() => new CoroutineWrapper(InfiniteUpdate());
        internal CoroutineWrapper Exit()
        {
            ShouldStop = true;
            return new CoroutineWrapper(OnExit());
        }

        // API

        public virtual void OnInitialize(GameObject owner)
        {
            //Debug.Log($"Initializing State: {GetType().FullName} with Id: {Id}");
        }

        public virtual IEnumerator OnEnter()
        {
            //Debug.Log($"Entering State: {GetType().FullName} with Id: {Id}");
            yield return null;
        }

        public virtual IEnumerator OnUpdate()
        {
            yield return null;
        }

        public virtual IEnumerator OnExit()
        {
            //Debug.Log($"Exiting State: {GetType().FullName} with Id: {Id}");
            yield return null;
        }
    }

    public class NullState : State { }
}