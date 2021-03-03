using System.Collections;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

namespace RedOwl.Engine
{
    public class AvatarRespawnable : AvatarAbility//, IPersistData
    {
        public override int Priority { get; } = 10000;

        public bool killAllPlayers;
        
        public Vector3 LastCheckpointPosition { get; private set; } = Vector3.zero;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<LevelCheckpoint>() == null) return;
            if (killAllPlayers)
            {
                foreach (var player in Avatar.Players)
                {
                    player.Abilities.Find<AvatarRespawnable>()?.SetCheckpointPosition(other.transform);
                }
            }
            else
            {
                SetCheckpointPosition(other.transform);
            }
        }

        public override void OnStart()
        {
            //Game.Register(this);
            foreach (var checkpoint in FindObjectsOfType<LevelCheckpoint>())
            {
                if (checkpoint.isLevelStart) LastCheckpointPosition = checkpoint.transform.position;
            }
        }

        public override void OnReset()
        {
            Motor.SetPosition(LastCheckpointPosition);
            gameObject.SetActive(true);
        }

        public override void OnCleanup()
        {
            //Game.Unregister(this);
        }
        
        private void SetCheckpointPosition(Transform target)
        {
            LastCheckpointPosition = target.position;
            //Game.Push(this);
        }

        [Button]
        public void Respawn()
        {
            if (killAllPlayers)
            {
                foreach (var player in Avatar.Players)
                {
                    player.Respawn();
                }
            }
            else
            {
                Avatar.Respawn();
            }
        }

        [Button]
        public void Kill()
        {
            if (killAllPlayers)
            {
                foreach (var player in Avatar.Players)
                {
                    var respawnable = player.Abilities.Find<AvatarRespawnable>();
                    if (respawnable != null) CoroutineManager.StartRoutine(respawnable.InternalKill());
                }

                Delayed.Run(Respawn, 2f);
            }
            else
            {
                CoroutineManager.StartRoutine(InternalKill());
                Delayed.Run(Respawn, 2f);
            }
        }

        private IEnumerator InternalKill()
        {
            Motor.SetPosition(LastCheckpointPosition);
            Motor.BaseVelocity = Vector3.zero;
            yield return null;
            Motor.BaseVelocity = Vector3.zero;
            gameObject.SetActive(false);
        }
        
        /*
        public PersistenceTypes SaveDataPersistenceType => PersistenceTypes.SaveFile;
        public string SaveDataId => $"{name}.{GetType()}";
        public int SaveDataLength => 16;

        public void SaveData(PersistenceWriter writer)
        {
            writer.Write((float3)LastCheckpointPosition);
        }

        public void LoadData(PersistenceReader reader)
        {
            LastCheckpointPosition = reader.ReadVector3();
            Respawn();
        }
        */
    }
}