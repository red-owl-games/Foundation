using Sirenix.OdinInspector;

namespace RedOwl.Engine
{
    public abstract class Builder<T> : Asset where T : new()
    {
        [ShowInInspector]
        public T Data = new T();

        public abstract T Build(string name);
        
        public static implicit operator T(Builder<T> builder) => builder.Build(builder.name);
    }
}