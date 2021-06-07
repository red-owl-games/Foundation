using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    [Serializable, InlineProperty, FoldoutGroup("Animator Properties"), PropertyOrder(10000)]
    public abstract class AnimatorProperty
    {
        [SerializeField, HideLabel]
        protected string name;
        [NonSerialized]
        protected int Parameter;
        [NonSerialized]
        protected AnimatorController Controller;
    }

    [Serializable]
    public class AnimFloatProperty : AnimatorProperty
    {
        public void Register(AnimatorController controller)
        {
            Controller = controller;
            controller.RegisterFloat(name, out Parameter);
        }

        public float Get() => Controller?.GetFloat(Parameter) ?? 0f;
        public void Set(float value) => Controller?.SetFloat(Parameter, value);
        
        public static implicit operator AnimFloatProperty(string name) => new AnimFloatProperty {name = name};
    }

    [Serializable]
    public class AnimIntProperty : AnimatorProperty
    {
        public void Register(AnimatorController controller)
        {
            Controller = controller;
            controller.RegisterInt(name, out Parameter);
        }
        
        public int Get() => Controller?.GetInt(Parameter) ?? 0;
        public void Set(int value) => Controller?.SetInt(Parameter, value);
        
        public static implicit operator AnimIntProperty(string name) => new AnimIntProperty {name = name};
    }

    [Serializable]
    public class AnimBoolProperty : AnimatorProperty
    {
        public void Register(AnimatorController controller)
        {
            Controller = controller;
            controller.RegisterBool(name, out Parameter);
        }
        
        public bool Get() => Controller?.GetBool(Parameter) ?? false;
        public void Set(bool value) => Controller?.SetBool(Parameter, value);

        public void On() => Set(true);
        public bool IsOn() => Get() == true;
        public void Off() => Set(false);
        public bool IsOff() => Get() == false;
        
        // Use this instead of Trigger Properties
        public void Trigger(float delay = 0.2f)
        {
            On();
            Game.DelayedCall(Off, delay);
        }
        
        public static implicit operator AnimBoolProperty(string name) => new AnimBoolProperty {name = name};
    }
    
    // Do Not Make a Trigger Property
    
    // Wraps calls to an Animator by caching parameter access
    // as well as provides Property classes for utility functionality and easy inspector configuration
    public class AnimatorController
    {
        private readonly Animator _animator;
        private readonly AnimatorControllerParameter[] _parameters;
        private readonly HashSet<int> _availableParameters;
        
        public AnimatorController(Animator animator)
        {
            _animator = animator;
            _parameters = _animator.parameters;
            _availableParameters = new HashSet<int>();
        }
        
        private bool HasParameterOfType(string name, AnimatorControllerParameterType type)
        {
            if (string.IsNullOrEmpty(name)) { return false; }
            foreach (AnimatorControllerParameter currParam in _parameters)
            {
                if (currParam.type == type && currParam.name == name)
                {
                    return true;
                }
            }
            return false;
        }
        
        public void RegisterParameter(string name, out int parameter, AnimatorControllerParameterType type)
        {
            parameter = Animator.StringToHash(name);
            if (HasParameterOfType(name, type))
            {
                _availableParameters.Add(parameter);
            }
        }

        public void RegisterFloat(string name, out int parameter) =>
            RegisterParameter(name, out parameter, AnimatorControllerParameterType.Float);

        public void RegisterInt(string name, out int parameter) =>
            RegisterParameter(name, out parameter, AnimatorControllerParameterType.Int);
        
        public void RegisterBool(string name, out int parameter) =>
            RegisterParameter(name, out parameter, AnimatorControllerParameterType.Bool);
        
        // Do Not Use Triggers

        public float GetFloat(int parameter)
        {
            return _availableParameters.Contains(parameter) ? _animator.GetFloat(parameter) : 0f;
        }
        
        public int GetInt(int parameter)
        {
            return _availableParameters.Contains(parameter) ? _animator.GetInteger(parameter) : 0;
        }
        
        public bool GetBool(int parameter)
        {
            return _availableParameters.Contains(parameter) && _animator.GetBool(parameter);
        }

        public void SetFloat(int parameter, float value)
        {
            if (_availableParameters.Contains(parameter)) _animator.SetFloat(parameter, value);
        }
        
        public void SetInt(int parameter, int value)
        {
            if (_availableParameters.Contains(parameter)) _animator.SetInteger(parameter, value);
        }
        
        public void SetBool(int parameter, bool value)
        {
            if (_availableParameters.Contains(parameter)) _animator.SetBool(parameter, value);
        }
    }
}