using System;
using System.Collections;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

namespace RedOwl.Engine
{
    // public Cooldown duration;
    // duration.Use();
    // duration.Tick(Time.deltaTime) ... Returns Percent Complete
    // if (duration.IsReady) ...allow a thing
    // if (duration.IsActive) ...still waiting
    
    [Serializable, InlineProperty]
    public class Cooldown
    {
        [HorizontalGroup("Cooldown")]
        [HideLabel]
        public float threshold;

        [HorizontalGroup("Cooldown", 0.2f)]
        [ShowInInspector, HideLabel, DisableInPlayMode, DisableInEditorMode]
        private float _cooldown;

        public Cooldown(float threshold = 1f)
        {
            this.threshold = threshold;
        }

        public bool IsReady => _cooldown < 0.000000001f;
        public bool IsActive => _cooldown > 0.000000001f;
        public float PercentComplete => 1 - (_cooldown / threshold);

        private void InternalTick(float deltaTime)
        {
            _cooldown = math.max(0, _cooldown - deltaTime);
        }
        
        public float Tick(float deltaTime = 1f)
        {
            if (IsReady) return 1;
            InternalTick(deltaTime);
            return PercentComplete;
        }

        public void Use()
        {
            _cooldown = threshold;
        }

        public IEnumerator WaitFor()
        {
            while (IsActive)
            {
                InternalTick(Time.deltaTime);
                yield return null;
            }
        }

        public static implicit operator Cooldown(float threshold) => new Cooldown(threshold);

        public static implicit operator Cooldown(int threshold) => new Cooldown(threshold);

        public static implicit operator float(Cooldown self) => self.threshold;


    }
}