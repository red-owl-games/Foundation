using UnityEngine;

namespace RedOwl.Engine
{
    public abstract class Builder<T> where T : Builder<T>
    {
        protected readonly T Instance = null;

        protected Builder() => 
            Instance = (T) this;

        public T Clone() => 
            JsonUtility.FromJson<T>(JsonUtility.ToJson(this));
    }
}