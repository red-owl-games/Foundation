using System.Collections.Generic;
using UnityEngine;

namespace RedOwl.Engine
{
    public class AnimatorManager
    {
        private readonly Animator _animator;
        private readonly List<int> _availableParameters;
        
        public AnimatorManager(Animator animator)
        {
            _animator = animator;
            _availableParameters = new List<int>();
        }
        
        private bool HasParameterOfType(string name, AnimatorControllerParameterType type)
        {
            if (string.IsNullOrEmpty(name)) { return false; }
            var parameters = _animator.parameters;
            foreach (AnimatorControllerParameter currParam in parameters)
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
        
        public void RegisterTrigger(string name, out int parameter) =>
            RegisterParameter(name, out parameter, AnimatorControllerParameterType.Trigger);

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
        
        public void SetTrigger(int parameter)
        {
            if (_availableParameters.Contains(parameter)) _animator.SetTrigger(parameter);
        }
    }
}