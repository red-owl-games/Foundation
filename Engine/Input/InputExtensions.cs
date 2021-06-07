using UnityEngine;
using UnityEngine.InputSystem;

namespace RedOwl.Engine
{
    public static class InputExtensions
    {
        public static void InvertY(this InputAction action, bool state)
        {
            // TODO: do we need to check if action is vector2?
            action.ApplyBindingOverride(new InputBinding {overrideProcessors = $"invertVector2(invertX=false,invertY={state})"});
        }
    }
}