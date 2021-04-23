namespace RedOwl.Engine
{
    public class History<T>
    {
        private T _last;
        private T _current;

        public T Previous => _last;
        public T Current => _current;

        public void Set(T next)
        {
            _last = _current;
            _current = next;
        }
    }
}