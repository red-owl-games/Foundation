using System.Collections.Generic;

namespace RedOwl.Engine
{
    public class FmodService
    {
        private readonly Dictionary<string, FmodController> _controllers;

        public FmodService()
        {
            _controllers = new Dictionary<string, FmodController>();
        }

        public FmodController NewController(FmodEvent fmodEvent)
        {
            var controller = new FmodController(fmodEvent);
            _controllers.Add(fmodEvent.path, controller);
            return controller;
        }
        
        public void Set(List<FmodEvent> events)
        {
            foreach (var fmodEvent in events)
            {
                if (_controllers.TryGetValue(fmodEvent.path, out var instance))
                {
                    instance.Start();
                    // Tween Parameters
                    foreach (var fmodParam in fmodEvent.parameters)
                    {
                        Log.Info($"Tween Parameter '{fmodParam.name}' to '{fmodParam.value}' for Fmod Event '{fmodEvent.path}'");
                        instance.SetParameter(fmodParam);
                    }
                }
                else
                {
                    // Create Instance
                    Log.Info($"Creating Fmod Event for '{fmodEvent.path}'");
                    var fmodController = NewController(fmodEvent);
                }
            }
            
            foreach (string path in _controllers.Keys)
            {
                bool found = false;
                foreach (var fmodEvent in events)
                {
                    if (fmodEvent.path == path) found = true;
                }

                if (found == false)
                {
                    _controllers[path].Stop();
                }
            }
        }

        public void Apply(string path, FmodParam parameter)
        {
            if (_controllers.TryGetValue(path, out var instance))
            {
                instance.SetParameter(parameter);
            }
        }
    }

    public partial class Game
    {
        public static FmodService FmodService => Find<FmodService>();

        public static void BindFmodService() => Bind(new FmodService());
    }
}