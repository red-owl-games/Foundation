using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    /// <summary>
    /// Parameter is a wrapper class for game parameters and easy serialization to file
    /// </summary>
    /// <typeparam name="T">The data type this parameter represents</typeparam>
    [Serializable, InlineProperty]
    public class Parameter<T> : ISerializationCallbackReceiver
    {
        [SerializeField, HideInInspector] private T value;

        private Func<T> _get;
        private Action<T> _set;

        [ShowInInspector, HideLabel, HideReferenceObjectPicker]
        public T Value
        {
            get => _get();
            set => _set(this.value = value);
        }

        private T _getter() => value;

        private void _setter(T v) { }

        public Parameter(T dv)
        {
            value = dv;
            _get = _getter;
            _set = _setter;
        }
        
        public Parameter(Func<T> get, Action<T> set)
        {
            value = get();
            _get = get;
            _set = set;
        }
        
        public Parameter(T dv, Func<T> get, Action<T> set)
        {
            value = dv;
            _get = get;
            _set = set;
            _set(value);
        }

        public void OnBeforeSerialize()
        {
            if (_get != null)
                value = _get();
        }

        public void OnAfterDeserialize()
        {
            _set?.Invoke(value);
        }

        public override string ToString() => value.ToString();

        public static implicit operator T(Parameter<T> p) => p.value;
        public static implicit operator Parameter<T>(T v) => new(v);
    }

    /// <summary>
    /// ParameterEnum is a wrapper class for Enum game parameters and serializes to file the Enum name
    /// </summary>
    /// <typeparam name="T">The Enum type this parameter represents</typeparam>
    [Serializable, InlineProperty]
    public class ParameterEnum<T> : ISerializationCallbackReceiver where T : Enum
    {
        [SerializeField, HideInInspector] private string value;

        private T _proxy;

        private Func<T> _get;
        private Action<T> _set;

        [ShowInInspector, HideLabel, HideReferenceObjectPicker]
        public T Value
        {
            get => _get();
            set => _set(_proxy = value);
        }

        private T _getter() => _proxy;

        private void _setter(T v) { }

        public ParameterEnum(T dv)
        {
            _proxy = dv;
            _get = _getter;
            _set = _setter;
        }
        
        public ParameterEnum(Func<T> get, Action<T> set)
        {
            _proxy = get();
            _get = get;
            _set = set;
        }
        
        public ParameterEnum(T dv, Func<T> get, Action<T> set)
        {
            _proxy = dv;
            _get = get;
            _set = set;
            _set(_proxy);
        }

        public void OnBeforeSerialize()
        {
            if (_get != null)
            {
                value = Enum.GetName(typeof(T), _proxy);
            }
        }

        public void OnAfterDeserialize()
        {
            _proxy = (T)Enum.Parse(typeof(T), value);
            _set?.Invoke(_proxy);
        }

        public override string ToString() => value.ToString();

        public static implicit operator T(ParameterEnum<T> p) => p._proxy;
        public static implicit operator ParameterEnum<T>(T v) => new(v);
    }
}