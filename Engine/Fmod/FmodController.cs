using System;
using DG.Tweening;
using FMOD;
using FMOD.Studio;

namespace RedOwl.Engine
{
    public class FmodController : IDisposable
    {
        private bool _isPlaying;
        private GUID _guid;
        private EventInstance _event;
        private STOP_MODE _stopMode;

        public FmodController(FmodEvent fmodEvent)
        {
            _event = FMODUnity.RuntimeManager.CreateInstance(fmodEvent.reference);
            if (!_event.isValid()) return;
            _guid = fmodEvent.reference.Guid;
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
                Log.Debug($"Tween Parameter '{parameter.name}' from '{from}' to '{parameter.value}' for '{_guid}'");
                DOVirtual.Float(from, parameter.value, parameter.duration, v => _event.setParameterByName(parameter.name, v));
            }
        }

        public void Start()
        {
            if (_isPlaying) return;
            Log.Debug($"Starting FMOD Event {_guid}");
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
            Log.Debug($"Stopping FMOD Event {_guid}");
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