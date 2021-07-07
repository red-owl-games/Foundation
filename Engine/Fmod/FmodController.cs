using System;
using DG.Tweening;
using FMOD;
using FMOD.Studio;

namespace RedOwl.Engine
{
    public class FmodController : IDisposable
    {
        private bool _isPlaying;
        private string _path;
        private EventInstance _event;
        private STOP_MODE _stopMode;

        public FmodController(FmodEvent fmodEvent)
        {
            _event = FMODUnity.RuntimeManager.CreateInstance(fmodEvent.path);
            if (!_event.isValid()) return;
            _path = fmodEvent.path;
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
            Start();
        }

        public void SetParameter(FmodParam parameter)
        {
            if (_event.getParameterByName(parameter.name, out float from) == RESULT.OK)
            {
                Log.Info($"Tween Parameter '{parameter.name}' from '{from}' to '{parameter.value}' for '{_path}'");
                DOVirtual.Float(from, parameter.value, parameter.duration, v => _event.setParameterByName(parameter.name, v));
            }
        }

        public void Start()
        {
            if (_isPlaying) return;
            Log.Debug($"Starting FMOD Event {_path}");
            _event.start();
            _isPlaying = true;
        }

        public void StopAndDispose()
        {
            Stop();
            Dispose();
        }

        public void Stop()
        {
            if (!_isPlaying) return;
            Log.Debug($"Stopping FMOD Event {_path}");
            _event.stop(_stopMode);
            _isPlaying = false;
        }

        public void Dispose()
        {
            Stop();
            _event.release();
        }
    }
}