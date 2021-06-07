using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RedOwl.Engine
{
    public interface ISpawnConfiguration
    {
        void Init();
        bool CanSpawn();
        void Spawn();
        IEnumerator Cooldown();
    }

    public interface ISpawnProvider
    {
        Transform GetSpawnable();
    }

    [Serializable]
    public struct SpawnLocation
    {
        [HideLabel]
        public Transform location;

        [ShowIf("location")]
        [HorizontalGroup("settings"), ToggleLeft, LabelWidth(100)]
        public bool parent;

        [HideIf("@location == null || parent")]
        [HorizontalGroup("settings"), ToggleLeft, LabelWidth(75), LabelText("Translation")]
        public bool matchTranslation;
        [HideIf("@location == null || parent")]
        [HorizontalGroup("settings"), ToggleLeft, LabelWidth(65), LabelText("Rotation")]
        public bool matchRotation;
        [HideIf("@location == null || parent")]
        [HorizontalGroup("settings"), ToggleLeft, LabelWidth(45), LabelText("Scale")]
        public bool matchScale;

        public void Apply(Transform spawned)
        {
            if (parent)
            {
                spawned.SetParent(location, false);
            }
            else
            {
                spawned.SetPositionAndRotation(
                    matchTranslation ? location.position : Vector3.one,
                    matchRotation ? location.rotation : Quaternion.identity);
                spawned.localScale = matchScale ? location.localScale : Vector3.one;
            }
        }
    }

    public enum SpawnLocationModes
    {
        RoundRobin,
        ForEach
    }
    
    [Serializable]
    public class SpawnConfiguration : ISpawnConfiguration
    {
        public Cooldown cooldown;
        
        [HorizontalGroup("config", 0.3f), ToggleLeft, LabelWidth(80)]
        public bool spawnForever;

        [HorizontalGroup("config"), LabelWidth(90)] [HideIf("spawnForever")]
        public Counter count;

        public SpawnLocationModes locationMode;
        
        public List<SpawnLocation> locations;

        [SerializeReference, InlineProperty, HideLabel] public ISpawnProvider provider;

        private bool _useLocation;
        private int _lastLocationUsed;
        
        public virtual void Init()
        {
            count.Reset();
            _useLocation = locations.Count > 0;
            _lastLocationUsed = -1;
        }

        public bool CanSpawn()
        {
            return spawnForever || cooldown.IsReady && count.IsReady;
        }

        public void Spawn()
        {

            switch (locationMode)
            {
                case SpawnLocationModes.RoundRobin:
                    RoundRobinSpawn();
                    break;
                case SpawnLocationModes.ForEach:
                    NestedSpawn();
                    break;
            }

            //spawnable.BroadcastMessage("OnSpawned", null, SendMessageOptions.DontRequireReceiver);
            if (!spawnForever) count.Use();
            cooldown.Use();
        }

        private void RoundRobinSpawn()
        {
            var spawnable = provider.GetSpawnable();
            if (_useLocation)
            {
                _lastLocationUsed++;
                if (_lastLocationUsed >= locations.Count) _lastLocationUsed = 0;
                locations[_lastLocationUsed].Apply(spawnable);
            }
            foreach (var onSpawned in spawnable.GetComponentsInChildren<IOnSpawned>())
            {
                onSpawned.OnSpawned();
            }
        }

        private void NestedSpawn()
        {
            foreach (var location in locations)
            {
                var spawnable = provider.GetSpawnable();
                location.Apply(spawnable);
                foreach (var onSpawned in spawnable.GetComponentsInChildren<IOnSpawned>())
                {
                    onSpawned.OnSpawned();
                }
            }
        }

        public IEnumerator Cooldown()
        {
            yield return cooldown.WaitFor();
        }
    }

    public class PrefabSpawn : ISpawnProvider
    {
        [HideLabel]
        public Transform prefab;

        public Transform GetSpawnable() => Object.Instantiate(prefab).transform;
    }

    public class PoolableSpawn : ISpawnProvider
    {
        [HideLabel]
        public PrefabPool pool;
        
        public Transform GetSpawnable() => pool.Request().transform;
    }
}