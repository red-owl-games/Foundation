namespace RedOwl.Engine
{
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
}