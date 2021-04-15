namespace RedOwl.Engine
{
    public abstract class Builder<T> where T : new()
    {
        public T Data = new T();
        public virtual T Build() => Data;
        public static implicit operator T(Builder<T> builder) => builder.Build();
    }
}