using System;
using System.Collections;
using System.Collections.Generic;

namespace RedOwl.Engine
{
    public class FmodService : IServiceAsync
    {
        [NonSerialized]
        private Dictionary<FMOD.GUID, FmodController> _controllers;

        public FmodService()
        {
            _controllers = new Dictionary<FMOD.GUID, FmodController>();
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
                if (fmodEvent.reference.IsNull) continue;
                if (_controllers.TryGetValue(fmodEvent.reference.Guid, out var instance) == false)
                {
                    // Create Instance
                    Log.Debug($"Creating Fmod Event for '{fmodEvent.reference}'");
                    _controllers[fmodEvent.reference.Guid] = instance = new FmodController(fmodEvent);
                }
                instance.Start();
                // Tween Parameters
                foreach (var fmodParam in fmodEvent.parameters)
                {
                    instance.SetParameter(fmodParam);
                }
            }
            
            foreach (FMOD.GUID guid in new List<FMOD.GUID>(_controllers.Keys))
            {
                bool found = false;
                foreach (var fmodEvent in events)
                {
                    if (fmodEvent.reference.IsNull) continue;
                    if (fmodEvent.reference.Guid == guid) found = true;
                }

                if (found == false)
                {
                    _controllers[guid].StopAndDispose();
                    _controllers.Remove(guid);
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