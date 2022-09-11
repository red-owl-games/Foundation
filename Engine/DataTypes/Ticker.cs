using System;

namespace RedOwl.Engine
{
    public class Ticker
    {
        private float _seconds;
        private float _current;
        
        public bool IsReady => _current > _seconds;
        
        public event Action On;
        
        public Ticker(float seconds = 1f, Action callback = null)
        {
            _seconds = seconds;
            if (callback != null)
            {
                On += callback;
            }
        }

        public bool Tick(float dt)
        {
            _current += dt;
            if (!IsReady) return false;
            Reset();
            On?.Invoke();
            return true;
        }

        public void Reset() => _current = 0f;
        
        public static implicit operator Ticker(float seconds) => new (seconds);

        public static implicit operator Ticker(int seconds) => new (seconds);
    }
}