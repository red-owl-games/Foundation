using System;
using System.Collections;
using FMODUnity;
using Sirenix.OdinInspector;

namespace RedOwl.Engine
{
    [Serializable]
    public struct FmodEvent
    {
        public EventReference reference;
        public FmodParam[] parameters;
        public FMOD.Studio.STOP_MODE stopMode;
    }
}