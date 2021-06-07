using System;
using Sirenix.OdinInspector;

namespace RedOwl.Engine
{
    // public Counter usages
    // usages.Reset()
    // if (usages.IsReady) usages.Use()
    [Serializable, InlineProperty]
    public class Counter
    {
        [HorizontalGroup("Counter")]
        [HideLabel, DisableInPlayMode]
        public int uses;

        public event Action<int> OnChanged;

        [HorizontalGroup("Counter", 0.2f)]
        [ShowInInspector, HideLabel, DisableInPlayMode, DisableInEditorMode]
        private int _count;

        public Counter(int uses = 1)
        {
            this.uses = uses;
        }

        public bool IsReady => _count > 0;

        public void Use(int count = 1)
        {
            _count -= count;
            OnChanged?.Invoke(_count);
        }

        [HorizontalGroup("Counter", 0.2f)]
        [Button, DisableInEditorMode]
        public void Reset()
        {
            _count = uses;
            OnChanged?.Invoke(_count);
        }
        
        public static implicit operator Counter(int uses) => new Counter(uses);
    }
}