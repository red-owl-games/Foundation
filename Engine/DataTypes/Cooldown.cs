using System;
using Sirenix.OdinInspector;
using Unity.Mathematics;

namespace RedOwl.Core
{
    [Serializable, InlineProperty]
    public class Cooldown
    {
        [HorizontalGroup("Cooldown")]
        [HideLabel]
        public float threshold;

        public event Action OnReady;
        public event Action<float> OnChanged;

        [HorizontalGroup("Cooldown", 0.2f)]
        [ShowInInspector, HideLabel, DisableInPlayMode, DisableInEditorMode]
        private float _cooldown;

        public Cooldown(float threshold = 1f)
        {
            this.threshold = threshold;
        }

        public bool IsReady => _cooldown < 0.000000001f;

        public void Tick(float deltaTime = 1f)
        {
            if (IsReady) return;
            _cooldown = math.max(0, _cooldown - deltaTime);
            OnChanged?.Invoke(_cooldown);
            if (IsReady) OnReady?.Invoke();
        }

        public void Use()
        {
            _cooldown = threshold;
            OnChanged?.Invoke(_cooldown);
        }

        public void Reset()
        {
            _cooldown = 0;
            OnChanged?.Invoke(_cooldown);
        }
    }
}