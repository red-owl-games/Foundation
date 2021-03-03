namespace RedOwl.Engine
{
    public abstract class Builder<T> where T : new()
    {
        protected T Data = new T();
        public T Build() => Data;
        public static implicit operator T(Builder<T> builder) => builder.Build();
    }
}