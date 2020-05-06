using System;
using System.Collections;
using UnityEngine;

namespace RedOwl.Core
{
    public class Cancelled : CustomYieldInstruction
    {
        public override bool keepWaiting => false;
    }
    
    public class CoroutineWrapper
    {
        public event Action OnDone;
        public event Action OnSuccessful;
        public event Action OnCancelled;
        
        private readonly IEnumerator _target;
        
        public CoroutineWrapper(IEnumerator target)
        {
            _target = target;
        }

        internal IEnumerator Routine()
        {
            bool didComplete = false;
            try
            {
                while (_target.MoveNext())
                {
                    if (_target.Current is Cancelled)
                    {
                        OnCancelled?.Invoke();
                        yield break;
                    }

                    yield return _target.Current;
                }

                didComplete = true;
            }
            finally
            {
                if (didComplete) OnSuccessful?.Invoke(); 
            }
            OnDone?.Invoke();
        }

        public void Stop()
        {
            CoroutineManager.StopRoutine(_target);
        }
        
        public CoroutineWrapper WhenDone(Action callback)
        {
            OnDone += callback;
            return this;
        }

        public CoroutineWrapper WhenSuccessful(Action callback)
        {
            OnSuccessful += callback;
            return this;
        }

        public CoroutineWrapper WhenCancelled(Action callback)
        {
            OnCancelled += callback;
            return this;
        }
    }

    public class CoroutineManager : Manager<CoroutineManager>
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init() => Initialize();

        private static IEnumerator CallbackWrapper(Func<bool> callback)
        {
            while (callback() == false)
            {
                yield return null;
            }
        }

        public static CoroutineWrapper StartRoutine(Func<bool> callback)
        {
            var output = new CoroutineWrapper(CallbackWrapper(callback));
            Instance.StartCoroutine(output.Routine());
            return output;
        }
        
        public static CoroutineWrapper StartRoutine(IEnumerator routine)
        {
            var output = new CoroutineWrapper(routine);
            Instance.StartCoroutine(output.Routine());
            return output;
        }

        public static void StopRoutine(CoroutineWrapper wrapper)
        {
            wrapper?.Stop();
        }
        
        public static void StopRoutine(Coroutine routine)
        {
            if (routine != null) Instance.StopCoroutine(routine);
        }
        
        public static void StopRoutine(IEnumerator routine) {
            Instance.StopCoroutine(routine);
        }
        
        public static void StopAllRoutines()
        {
            Instance.StopAllCoroutines();
        }
    }
}

