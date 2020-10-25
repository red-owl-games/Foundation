using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    [Serializable, InlineProperty, FoldoutGroup("Animator Properties")]
    public abstract class AnimatorProperty
    {
        [SerializeField, HideLabel]
        protected string Name;
        [NonSerialized]
        protected int Parameter;
        [NonSerialized]
        protected AnimatorManager _manager;
    }

    [Serializable]
    public class AnimFloatProperty : AnimatorProperty
    {
        public void Register(AnimatorManager manager)
        {
            _manager = manager;
            manager.RegisterFloat(Name, out Parameter);
        }

        public float Get() => _manager.GetFloat(Parameter);
        public void Set(float value) => _manager.SetFloat(Parameter, value);
        
        public static implicit operator AnimFloatProperty(string name) => new AnimFloatProperty {Name = name};
    }

    [Serializable]
    public class AnimIntProperty : AnimatorProperty
    {
        public void Register(AnimatorManager manager)
        {
            _manager = manager;
            manager.RegisterInt(Name, out Parameter);
        }
        
        public int Get() => _manager.GetInt(Parameter);
        public void Set(int value) => _manager.SetInt(Parameter, value);
        
        public static implicit operator AnimIntProperty(string name) => new AnimIntProperty {Name = name};
    }

    [Serializable]
    public class AnimBoolProperty : AnimatorProperty
    {
        public void Register(AnimatorManager manager)
        {
            _manager = manager;
            manager.RegisterBool(Name, out Parameter);
        }
        
        public bool Get() => _manager.GetBool(Parameter);
        public void Set(bool value) => _manager.SetBool(Parameter, value);

        public void Trigger(float delay = 0.2f)
        {
            On();
            Delayed.Run(Off, delay);
        }

        public void On() => Set(true);
        public bool IsOn() => Get() == true;
        public void Off() => Set(false);
        public bool IsOff() => Get() == false;
        
        public static implicit operator AnimBoolProperty(string name) => new AnimBoolProperty {Name = name};
    }

    // TODO: Don't use this - use the bool with the Trigger function
    [Serializable]
    public class AnimTriggerProperty : AnimatorProperty
    {
        public void Register(AnimatorManager manager)
        {
            _manager = manager;
            manager.RegisterTrigger(Name, out Parameter);
        }
        
        public void Set() => _manager.SetTrigger(Parameter);
        
        public static implicit operator AnimTriggerProperty(string name) => new AnimTriggerProperty {Name = name};
    }
}