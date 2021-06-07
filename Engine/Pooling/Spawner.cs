using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Random = Unity.Mathematics.Random;

namespace RedOwl.Engine
{
    //yield return new WaitForSeconds(_rng.NextFloat(config.spawnInterval.x, config.spawnInterval.y));
    
    // private bool CanAutoSpawn => config.spawnForever || _currentSpawnCycle < config.spawnCycles;
    // private bool CanManualSpawn => (config.spawnForever || _currentSpawnCycle < config.spawnCycles) && _canManualSpawn;
    
    // private void Spawn()
    // {
    //     _canManualSpawn = false;
    //     _currentSpawnCycle++;
    //     SpawnEntry entry = config.GetNextSpawnEntry();
    //     var spawnable = entry.pool.Request().transform;
    //     spawnable.SetPositionAndRotation(
    //         config.matchScale ? _trans.localScale : Vector3.one,
    //         config.matchRotation ? _trans.rotation : Quaternion.identity
    //         );
    //     if (config.parentToSpawner) spawnable.SetParent(_trans, true);
    //     if (entry.data != null) spawnable.BroadcastMessage("OnConfigureSpawned", entry.data, SendMessageOptions.DontRequireReceiver);
    // }
    
    
    /*
    [Serializable]
    public struct SpawnEntry
    {
        [Title("Prefab"), HorizontalGroup("data"), HideLabel, Required]
        public PrefabPool pool;
        [Title("Data"), HorizontalGroup("data"), HideLabel]
        public ScriptableObject data;
    }

    [Serializable]
    public class SpawnerConfiguration
    {
        [HorizontalGroup("config", 0.3f), ToggleLeft, LabelWidth(80)]
        public bool spawnForever;
        [HorizontalGroup("config"), LabelWidth(90)]
        [HideIf("spawnForever")]
        public int spawnCycles;

        [HorizontalGroup("match"), ToggleLeft, LabelWidth(100)]
        public bool matchRotation;
        [HorizontalGroup("match"), ToggleLeft, LabelWidth(95)]
        public bool matchScale;
        [HorizontalGroup("match"), ToggleLeft, LabelWidth(110)]
        public bool parentToSpawner;
    
        [Title("Spawn Interval (seconds)"), MinMaxSlider(0f, 100f, true), HideLabel]
        public Vector2 spawnInterval = Vector2.one;

        public List<SpawnEntry> spawnEntries;

        private int _entryIndex = -1;

        public void ResetIndex()
        {
            _entryIndex = -1;
        }

        public SpawnEntry GetNextSpawnEntry()
        {
            _entryIndex++;
            if (_entryIndex >= spawnEntries.Count) _entryIndex = 0;
            return spawnEntries[_entryIndex];
        }
    }
    */
    
    public class Spawner : Spawnable, IOnSpawned, IOnDespawned
    {
        public enum AutoStartOn
        {
            Start,
            Spawned,
            Never
        }
        
        [Serializable]
        public class SpawnEvent : UnityEvent {}
        
        [FoldoutGroup("Spawn Configuration"), BoxGroup("Spawn Configuration/Start Settings"), LabelWidth(90)]
        public AutoStartOn autoStartOn;
        [HorizontalGroup("Spawn Configuration/Start Settings/extra"), ToggleLeft, LabelWidth(125)]
        public bool despawnOnComplete;
        [HorizontalGroup("Spawn Configuration/Start Settings/extra"), LabelWidth(80)]
        [HideIf("autoStartOn", AutoStartOn.Never)]
        public float startDelay;
        [HorizontalGroup("Spawn Configuration/Start Settings/extra"), ToggleLeft, LabelWidth(125)]
        [HideIf("autoStartOn", AutoStartOn.Never)]
        public bool destroyOnComplete;

        [SerializeReference]
        [FoldoutGroup("Spawn Configuration"), HideLabel]
        public ISpawnConfiguration config;

        [FoldoutGroup("Events"), HorizontalGroup("Events/flags"), ToggleLeft, LabelWidth(50)]
        public bool started;
        [HorizontalGroup("Events/flags"), ToggleLeft, LabelWidth(50)]
        public bool stopped;
        [HorizontalGroup("Events/flags"), ToggleLeft, LabelWidth(50)]
        public bool paused;
        [HorizontalGroup("Events/flags"), ToggleLeft, LabelWidth(50)]
        public bool resumed;
        [HorizontalGroup("Events/flags"), ToggleLeft, LabelWidth(50)]
        public bool completed;
        
        [FoldoutGroup("Events"), ShowIf("started")]
        public SpawnEvent onStart = new SpawnEvent();
        [FoldoutGroup("Events"), ShowIf("stopped")]
        public SpawnEvent onStop = new SpawnEvent();
        [FoldoutGroup("Events"), ShowIf("paused")]
        public SpawnEvent onPause = new SpawnEvent();
        [FoldoutGroup("Events"), ShowIf("resumed")]
        public SpawnEvent onResume = new SpawnEvent();
        [FoldoutGroup("Events"), ShowIf("completed")]
        public SpawnEvent onComplete = new SpawnEvent();
        
        private Coroutine _mainloop;
        
        // TODO: StateMachine?
#pragma warning disable 414
        private bool _isRunning;
#pragma warning restore 414
        private bool _isPaused;

        private void Awake()
        {
            config.Init();
        }

        private void Start()
        {
            if (autoStartOn == AutoStartOn.Start) StartSpawning();
        }

        public void OnSpawned()
        {
            if (autoStartOn == AutoStartOn.Spawned) StartSpawning();
        }
        
        public void OnDespawned()
        {
            StopSpawning();
            if (destroyOnComplete) gameObject.Destroy();
        }

        private void OnDestroy() => StopSpawning();

        private IEnumerator MainLoop()
        {
            _isRunning = true;
            if (autoStartOn != AutoStartOn.Never) yield return new WaitForSeconds(startDelay);
            if (started) onStart?.Invoke();
            while (config.CanSpawn())
            {
                while (_isPaused)
                {
                    yield return null;
                }
                config.Spawn();
                yield return Cooldown();
            }
            if (completed) onComplete?.Invoke();
            if (despawnOnComplete) Despawn();
            _isRunning = false;
        }

        private IEnumerator Cooldown()
        {
            yield return config.Cooldown();
        }

        [PublicAPI]
        [FoldoutGroup("Runtime Controls"), ResponsiveButtonGroup("Runtime Controls/Buttons"), Button(ButtonSizes.Medium), EnableIf("canDespawn"), DisableIf("_isRunning"), DisableInEditorMode]
        public void StartSpawning()
        {
            _mainloop = StartCoroutine(MainLoop());
        }

        [PublicAPI]
        [ResponsiveButtonGroup("Runtime Controls/Buttons"), Button(ButtonSizes.Medium), EnableIf("_isRunning"), DisableInEditorMode]
        public void StopSpawning()
        {
            _isPaused = false;
            _isRunning = false;
            if (_mainloop != null) StopCoroutine(_mainloop);
            if (stopped) onStop?.Invoke();
        }

        [PublicAPI]
        [ResponsiveButtonGroup("Runtime Controls/Buttons"), Button(ButtonSizes.Medium), EnableIf("_isRunning"), DisableIf("_isPaused"), DisableInEditorMode]
        public void PauseSpawning()
        {
            _isPaused = true;
            if (paused) onPause?.Invoke();
        }

        [PublicAPI]
        [ResponsiveButtonGroup("Runtime Controls/Buttons"), Button(ButtonSizes.Medium), EnableIf("_isRunning"), EnableIf("_isPaused"), DisableInEditorMode]
        public void ResumeSpawning()
        {
            _isPaused = false;
            if (resumed) onResume?.Invoke();
        }
        
        [PublicAPI]
        [FoldoutGroup("Runtime Controls"), Button("Spawn", ButtonSizes.Medium), EnableIf("canDespawn"), DisableIf("_isRunning"), DisableInEditorMode]
        public void SpawnNext()
        {
            if (!config.CanSpawn()) return;
            config.Spawn();
            StartCoroutine(Cooldown());
        }
    }
}