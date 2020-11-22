using System;
using System.Collections.Generic;
using DG.Tweening;
using FMOD;
using FMOD.Studio;

namespace RedOwl.Engine
{
    public readonly struct FmodEvent
    {
        public readonly string path;
        public readonly FmodParam[] parameters;
        public readonly STOP_MODE stopMode;
        public FmodEvent(string path, STOP_MODE stopMode = STOP_MODE.ALLOWFADEOUT, params FmodParam[] parameters)
        {
            this.path = path;
            this.parameters = parameters;
            this.stopMode = stopMode;
        }
    }

    public readonly struct FmodParam
    {
        public readonly string name;
        public readonly float value;
        public readonly float duration;

        public FmodParam(string name, float value, float duration = 0f)
        {
            this.name = name;
            this.value = value;
            this.duration = duration;
        }
    }
    
    public class FmodController : IDisposable
    {
        private static Dictionary<string, FmodController> _all = new Dictionary<string, FmodController>();
        
        private bool _isPlaying;
        private EventInstance _event;
        private STOP_MODE _stopMode;

        public FmodController(FmodEvent fmodEvent)
        {
            _event = FMODUnity.RuntimeManager.CreateInstance($"event:{fmodEvent.path}");
            if (!_event.isValid()) return;
            _all.Add(fmodEvent.path, this);
            foreach (var fmodParam in fmodEvent.parameters)
            {
                if (fmodParam.duration > 0)
                {
                    if (_event.getParameterByName(fmodParam.name, out float from) == RESULT.OK)
                        DOVirtual.Float(from, fmodParam.value, fmodParam.duration, v => _event.setParameterByName(fmodParam.name, v));
                }
                else
                {
                    _event.setParameterByName(fmodParam.name, fmodParam.value);
                }
            }

            _stopMode = fmodEvent.stopMode;
            _event.start();
            _isPlaying = true;
        }

        public void SetParameter(FmodParam parameter)
        {
            if (_event.getParameterByName(parameter.name, out float from) == RESULT.OK)
                DOVirtual.Float(from, parameter.value, parameter.duration, v => _event.setParameterByName(parameter.name, v));
        }

        public void Start()
        {
            if (_isPlaying) return;
            _event.start();
            _isPlaying = true;
        }

        public void Stop()
        {
            if (!_isPlaying) return;
            _event.stop(_stopMode);
            _isPlaying = false;
        }

        public void Dispose()
        {
            Stop();
            _event.release();
        }
        
        public static void Set(List<FmodEvent> events)
        {
            foreach (var fmodEvent in events)
            {
                if (_all.TryGetValue(fmodEvent.path, out var instance))
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
                    var fmodController = new FmodController(fmodEvent);
                }
            }
            
            foreach (string path in _all.Keys)
            {
                bool found = false;
                foreach (var fmodEvent in events)
                {
                    if (fmodEvent.path == path) found = true;
                }

                if (found == false)
                {
                    Log.Always($"Stopping Fmod Event '{path}'");
                    _all[path].Stop();
                }
            }
        }

        public static void Apply(string path, FmodParam parameter)
        {
            if (_all.TryGetValue(path, out var instance))
            {
                instance.SetParameter(parameter);
            }
        }
    }
}