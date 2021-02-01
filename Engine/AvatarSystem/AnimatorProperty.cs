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
        protected AnimatorController Controller;
    }

    [Serializable]
    public class AnimFloatProperty : AnimatorProperty
    {
        public void Register(AnimatorController controller)
        {
            Controller = controller;
            controller.RegisterFloat(Name, out Parameter);
        }

        public float Get() => Controller.GetFloat(Parameter);
        public void Set(float value) => Controller.SetFloat(Parameter, value);
        
        public static implicit operator AnimFloatProperty(string name) => new AnimFloatProperty {Name = name};
    }

    [Serializable]
    public class AnimIntProperty : AnimatorProperty
    {
        public void Register(AnimatorController controller)
        {
            Controller = controller;
            controller.RegisterInt(Name, out Parameter);
        }
        
        public int Get() => Controller.GetInt(Parameter);
        public void Set(int value) => Controller.SetInt(Parameter, value);
        
        public static implicit operator AnimIntProperty(string name) => new AnimIntProperty {Name = name};
    }

    [Serializable]
    public class AnimBoolProperty : AnimatorProperty
    {
        public void Register(AnimatorController controller)
        {
            Controller = controller;
            controller.RegisterBool(Name, out Parameter);
        }
        
        public bool Get() => Controller.GetBool(Parameter);
        public void Set(bool value) => Controller.SetBool(Parameter, value);

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
        public void Register(AnimatorController controller)
        {
            Controller = controller;
            controller.RegisterTrigger(Name, out Parameter);
        }
        
        public void Set() => Controller.SetTrigger(Parameter);
        
        public static implicit operator AnimTriggerProperty(string name) => new AnimTriggerProperty {Name = name};
    }
}