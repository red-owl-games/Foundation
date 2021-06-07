using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace RedOwl.Engine
{
    public interface IOnSpawned
    {
        void OnSpawned();
    }

    public interface IOnDespawned
    {
        void OnDespawned();
    }
    
    public interface IDespawnFilter
    {
        bool ShouldDespawn(GameObject other);
    }
    
    [HideMonoScript, DisallowMultipleComponent]
    public class Spawnable : MonoBehaviour
    {
        public enum DespawnAfter
        {
            Manual = 0,
            Time,
            SoundPlayed,
            EffectPlayed,
            Collision,
            Trigger,
            Collision2d,
            Trigger2d
        }
        
        [Serializable]
        public class DespawnEvent : UnityEvent {}

        [BoxGroup("Despawn Configuration"), PropertyOrder(-10), LabelWidth(90)]
        public DespawnAfter despawnAfter = DespawnAfter.Manual;

        [BoxGroup("Despawn Configuration"), ShowIf("despawnAfter", DespawnAfter.Time)]
        public float duration = 5f;

        private AudioSource _audioSource;
        [BoxGroup("Despawn Configuration"), ShowInInspector, ShowIf("despawnAfter", DespawnAfter.SoundPlayed), PropertyOrder(-1)]
        public AudioSource AudioSource => _audioSource != null ? _audioSource : _audioSource = GetComponentInChildren<AudioSource>(false);

        [BoxGroup("Despawn Configuration"), ShowInInspector, ShowIf("despawnAfter", DespawnAfter.SoundPlayed), LabelText("Despawn after duration:"), DisplayAsString]
        public float AudioSourceTotalDuration
        {
            get {
                if(AudioSource == null || AudioSource.clip == null) return 0;
                return AudioSource.clip.length;
                
            }
        }

        private ParticleSystem _particleSystem;

        [BoxGroup("Despawn Configuration"), ShowInInspector, ShowIf("despawnAfter", DespawnAfter.EffectPlayed),
         PropertyOrder(-1)]
        public ParticleSystem ParticleSystem => _particleSystem != null ? _particleSystem : _particleSystem = GetComponentInChildren<ParticleSystem>(false);

        [HorizontalGroup("Despawn Configuration/particleSystem"), ShowIf("despawnAfter", DespawnAfter.EffectPlayed), ToggleLeft, LabelText("Use Duration"), LabelWidth(100)]
        public bool useParticleSystemDuration = true;
        [HorizontalGroup("Despawn Configuration/particleSystem"), ShowIf("despawnAfter", DespawnAfter.EffectPlayed), ToggleLeft, LabelText("Use Start Delay"), LabelWidth(100)]
        public bool useParticleSystemStartDelay = true;
        [HorizontalGroup("Despawn Configuration/particleSystem"), ShowIf("despawnAfter", DespawnAfter.EffectPlayed), ToggleLeft, LabelText("Use Start Lifetime"), LabelWidth(100)]
        public bool useParticleSystemStartLifetime = true;
        [BoxGroup("Despawn Configuration"), ShowIf("despawnAfter", DespawnAfter.EffectPlayed)]
        public float extraTime;
        
        [BoxGroup("Despawn Configuration"), ShowInInspector, ShowIf("despawnAfter", DespawnAfter.EffectPlayed), LabelText("Despawn after duration:"), DisplayAsString]
        public float particleSystemTotalDuration
        {
            get
            {
                if(ParticleSystem == null) { return 0; }
                ParticleSystem.MainModule main = ParticleSystem.main;
                return (useParticleSystemDuration ? main.duration : 0)
                       + (useParticleSystemStartDelay ? main.startDelay.constant : 0)
                       + (useParticleSystemStartLifetime ? main.startLifetime.constant : 0)
                       + extraTime;
            }
        }
        
        [BoxGroup("Despawn Configuration")]
        [HorizontalGroup("Despawn Configuration/event"), ShowIf("NeedsCollisionTriggerProperties"), ToggleLeft, LabelWidth(60)]
        public bool onEnter;
        [HorizontalGroup("Despawn Configuration/event"), ShowIf("NeedsCollisionTriggerProperties"), ToggleLeft, LabelWidth(60)]
        public bool onStay;
        [HorizontalGroup("Despawn Configuration/event"), ShowIf("NeedsCollisionTriggerProperties"), ToggleLeft, LabelWidth(60)]
        public bool onExit;
        
        [HorizontalGroup("Despawn Configuration/filter", 0.2f), ShowIf("NeedsCollisionTriggerProperties"), ToggleLeft, LabelWidth(50)]
        public bool useFilter;

        private IDespawnFilter _filter;
        [HorizontalGroup("Despawn Configuration/filter"), ShowIf("NeedsCollisionTriggerProperties"), HideLabel, ShowInInspector, ShowIf("useFilter")]
        public IDespawnFilter Filter => (MonoBehaviour)_filter != null ? _filter : _filter = GetComponentInChildren<IDespawnFilter>(false);

        [FoldoutGroup("Events"), PropertyOrder(1)]
        public DespawnEvent onDespawn = new DespawnEvent();

        private Coroutine _mainloop;

        private bool NeedsCollisionTriggerProperties()
        {
            return despawnAfter!= DespawnAfter.Manual && despawnAfter != DespawnAfter.Time && despawnAfter != DespawnAfter.SoundPlayed && despawnAfter != DespawnAfter.EffectPlayed;
        }

        private void Awake()
        {
            switch (despawnAfter)
            {
                case DespawnAfter.Collision:
                    if (onEnter) gameObject.AddComponent<SpawnableOnCollisionEnterListener>().spawnable = this;
                    if (onStay) gameObject.AddComponent<SpawnableOnCollisionStayListener>().spawnable = this;
                    if (onExit) gameObject.AddComponent<SpawnableOnCollisionExitListener>().spawnable = this;
                    break;
                case DespawnAfter.Trigger:
                    if (onEnter) gameObject.AddComponent<SpawnableOnTriggerEnterListener>().spawnable = this;
                    if (onStay) gameObject.AddComponent<SpawnableOnTriggerStayListener>().spawnable = this;
                    if (onExit) gameObject.AddComponent<SpawnableOnTriggerExitListener>().spawnable = this;
                    break;
                case DespawnAfter.Collision2d:
                    if (onEnter) gameObject.AddComponent<SpawnableOnCollisionEnter2DListener>().spawnable = this;
                    if (onStay) gameObject.AddComponent<SpawnableOnCollisionStay2DListener>().spawnable = this;
                    if (onExit) gameObject.AddComponent<SpawnableOnCollisionExit2DListener>().spawnable = this;
                    break;
                case DespawnAfter.Trigger2d:
                    if (onEnter) gameObject.AddComponent<SpawnableOnTriggerEnterListener>().spawnable = this;
                    if (onStay) gameObject.AddComponent<SpawnableOnTriggerStayListener>().spawnable = this;
                    if (onExit) gameObject.AddComponent<SpawnableOnTriggerExitListener>().spawnable = this;
                    break;
            }
        }
        
        private void OnEnable()
        {
            if (despawnAfter != DespawnAfter.Manual) _mainloop = StartCoroutine(Despawnloop());
        }

        private void OnDisable()
        {
            if (_mainloop == null) return;
            StopCoroutine(_mainloop);
            _mainloop = null;
        }

        private void OnDestroy()
        {
            if(_mainloop != null) { StopCoroutine(_mainloop); _mainloop = null; }
            onDespawn?.Invoke();
        }
        
        private IEnumerator Despawnloop()
        {
            switch (despawnAfter)
            {
                case DespawnAfter.Time:
                    yield return new WaitForSeconds(duration);
                    break;
                case DespawnAfter.SoundPlayed:
                    PlaySound();
                    yield return new WaitForSeconds(AudioSourceTotalDuration);
                    break;
                case DespawnAfter.EffectPlayed:
                    PlayEffect();
                    yield return new WaitForSeconds(particleSystemTotalDuration);
                    break;
                case DespawnAfter.Collision:
                case DespawnAfter.Trigger:
                case DespawnAfter.Collision2d:
                case DespawnAfter.Trigger2d:
                    yield break;
            }
            Despawn();
        }
        
        private void PlaySound()
        {
            if (AudioSource == null || AudioSource.clip == null) return;
            AudioSource.Play();
        }

        private void PlayEffect()
        {
            if (ParticleSystem == null) return;
            ParticleSystem.Clear(true);
            ParticleSystem.Play();
        }

        private bool canDespawn => gameObject.activeInHierarchy;
        [FoldoutGroup("Runtime Controls"), PropertyOrder(10), Button(ButtonSizes.Medium), EnableIf("canDespawn"), DisableInEditorMode]
        public void Despawn()
        {
            onDespawn?.Invoke();
            foreach (var onDespawned in GetComponentsInChildren<IOnDespawned>())
            {
                onDespawned.OnDespawned();
            }
            //BroadcastMessage("OnDespawn", null, SendMessageOptions.DontRequireReceiver);
        }
    }
}