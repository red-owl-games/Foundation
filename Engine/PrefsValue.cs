using System;
using UnityEngine;

namespace RedOwl.Core
{
    public class PrefsValue<T>
    {
        private readonly string _key;
        private readonly T _defaultValue;
        private readonly Func<string, T, T> _getter;
        private readonly Action<string, T> _setter;

        private T _value;
        public T Value
        {
            get
            {
                if (_value == null) _value = _getter(_key, _defaultValue);
                return _value;
            }
            set
            {
                _value = value;
                _setter(_key, value);
            }
        }

        protected PrefsValue(string key, T defaultValue, Func<string, T, T> getter, Action<string, T> setter)
        {
            _key = key;
            _defaultValue = defaultValue;
            _getter = getter;
            _setter = setter;
        }
        
        public static implicit operator T(PrefsValue<T> self) => self.Value;
    }

    public class IntPref : PrefsValue<int>
    {
        public IntPref(string key, int defaultValue = default) : base(key, defaultValue, PlayerPrefs.GetInt, PlayerPrefs.SetInt) {}
    }
    
    public class FloatPref : PrefsValue<float>
    {
        public FloatPref(string key, float defaultValue = default) : base(key, defaultValue, PlayerPrefs.GetFloat, PlayerPrefs.SetFloat) {}
    }
    
    public class StringPref : PrefsValue<string>
    {
        public StringPref(string key, string defaultValue = default) : base(key, defaultValue, PlayerPrefs.GetString, PlayerPrefs.SetString) {}
    }
}