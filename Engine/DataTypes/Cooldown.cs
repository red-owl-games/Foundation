using System;
using Sirenix.OdinInspector;
using Unity.Mathematics;

namespace RedOwl.Engine
{
    // public Cooldown duration;
    // duration.Use();
    // duration.Tick(Time.deltaTime)
    // if (duration.IsReady) ...allow a thing
    // if (duration.IsActive) ...still waiting
    [Serializable, InlineProperty]
    public class Cooldown
    {
        [HorizontalGroup("Cooldown")]
        [HideLabel]
        public float threshold;

        public event Action OnReady;
        public event Action<float> OnChanged;
        public event Action<float> OnChangedPercent;

        [HorizontalGroup("Cooldown", 0.2f)]
        [ShowInInspector, HideLabel, DisableInPlayMode, DisableInEditorMode]
        private float _cooldown;

        public Cooldown(float threshold = 1f)
        {
            this.threshold = threshold;
        }

        public bool IsReady => _cooldown < 0.000000001f;
        public bool IsActive => _cooldown > 0.000000001f;

        public void Tick(float deltaTime = 1f)
        {
            if (IsReady) return;
            _cooldown = math.max(0, _cooldown - deltaTime);
            OnChanged?.Invoke(_cooldown);
            OnChangedPercent?.Invoke(_cooldown / threshold);
            if (IsReady) OnReady?.Invoke();
        }

        public void Use()
        {
            _cooldown = threshold;
            OnChanged?.Invoke(_cooldown);
            OnChangedPercent?.Invoke(_cooldown / threshold);
        }

        public void Reset()
        {
            _cooldown = 0;
            OnChanged?.Invoke(_cooldown);
            OnChangedPercent?.Invoke(_cooldown / threshold);
        }

        public static implicit operator Cooldown(float threshold) => new Cooldown(threshold);

        public static implicit operator Cooldown(int threshold) => new Cooldown(threshold);

        public static implicit operator float(Cooldown self) => self.threshold;
    }
}