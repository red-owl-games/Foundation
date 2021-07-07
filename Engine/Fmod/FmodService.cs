using System;
using System.Collections;
using System.Collections.Generic;

namespace RedOwl.Engine
{
    public class FmodService : IServiceAsync
    {
        [NonSerialized]
        private Dictionary<string, FmodController> _controllers;

        public FmodService()
        {
            _controllers = new Dictionary<string, FmodController>();
        }
        
        public IEnumerator AsyncInit()
        {
            while (FMODUnity.RuntimeManager.HasBankLoaded("Master Bank"))
            {
                yield return null;
            }
            Log.Info("Master Bank Loaded");
        }

        public void Ensure(params FmodEvent[] events)
        {
            foreach (var fmodEvent in events)
            {
                if (!fmodEvent.IsValid()) continue;
                if (_controllers.TryGetValue(fmodEvent.path, out var instance) == false)
                {
                    // Create Instance
                    Log.Info($"Creating Fmod Event for '{fmodEvent.path}'");
                    _controllers[fmodEvent.path] = instance = new FmodController(fmodEvent);
                }
                instance.Start();
                // Tween Parameters
                foreach (var fmodParam in fmodEvent.parameters)
                {
                    instance.SetParameter(fmodParam);
                }
            }
            
            foreach (string path in new List<string>(_controllers.Keys))
            {
                bool found = false;
                foreach (var fmodEvent in events)
                {
                    if (!fmodEvent.IsValid()) continue;
                    if (fmodEvent.path == path) found = true;
                }

                if (found == false)
                {
                    _controllers[path].StopAndDispose();
                    _controllers.Remove(path);
                }
            }
        }
    }

    public partial class Game
    {
        public static FmodService FmodService => Find<FmodService>();

        // TODO: Should Bind IAudioService
        public static void BindFmodService() => Bind(new FmodService());
    }
}