using System;

namespace RedOwl.Engine
{
    [Serializable]
    public struct FmodEvent
    {
        [FMODUnity.EventRef]
        public string path;
        public FmodParam[] parameters;
        public FMOD.Studio.STOP_MODE stopMode;

        public bool IsValid() => !string.IsNullOrEmpty(path);
    }
}