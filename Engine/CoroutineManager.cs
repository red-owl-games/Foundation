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
        private Coroutine _routine;
        
        public CoroutineWrapper(IEnumerator target)
        {
            _target = target;
        }
        
        public CoroutineWrapper Start()
        {
            _routine = CoroutineManager.StartRoutine(Wrapper(_target));
            return this;
        }

        public CoroutineWrapper Stop()
        {
            CoroutineManager.StopRoutine(_routine);
            return this;
        }

        private IEnumerator Wrapper(IEnumerator target)
        {
            bool didComplete = false;
            try
            {
                while (target.MoveNext())
                {
                    if (target.Current is Cancelled)
                    {
                        OnCancelled?.Invoke();
                        yield break;
                    }

                    yield return target.Current;
                }

                didComplete = true;
            }
            finally
            {
                if (didComplete) OnSuccessful?.Invoke();
            }
            OnDone?.Invoke();
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

    public class NullCoroutineWrapper : CoroutineWrapper
    {
        public NullCoroutineWrapper() : base(NullWrapper())
        {
        }

        private static IEnumerator NullWrapper()
        {
            yield return null;
        }
    }

    public class CoroutineManager : MonoBehaviour
    {
        private static bool _initialized;
        private static CoroutineManager _instance;

        public static CoroutineManager Instance {
            get {
                if (!_initialized)
                {
                    var obj = new GameObject($"{nameof(CoroutineManager)}");
                    DontDestroyOnLoad(obj);
                    _instance = obj.AddComponent<CoroutineManager>();
                    _initialized = true;
                }
                return _instance;
            }
        }
        
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
            output.Start();
            return output;
        }

        public static Coroutine StartRoutine(IEnumerator wrapper)
        {
            return Instance.StartCoroutine(wrapper);
        }

        public static void StopRoutine(Coroutine routine)
        {
            if (routine != null) Instance.StopCoroutine(routine);
        }
        
        public static void StopAllRoutines()
        {
            Instance.StopAllCoroutines();
        }
    }
}

