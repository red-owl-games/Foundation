namespace RedOwl.Engine
{
    public readonly struct FmodEvent
    {
        public readonly string path;
        public readonly FmodParam[] parameters;
        public readonly FMOD.Studio.STOP_MODE stopMode;
        public FmodEvent(string path, FMOD.Studio.STOP_MODE stopMode = FMOD.Studio.STOP_MODE.ALLOWFADEOUT, params FmodParam[] parameters)
        {
            this.path = path;
            this.parameters = parameters;
            this.stopMode = stopMode;
        }
    }
}