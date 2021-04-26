using System.Collections;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

namespace RedOwl.Engine
{
    public class AvatarRespawnable : AvatarAbility
    {
        public override int Priority { get; } = 10000;

        public bool killAllPlayers;
        
        public Vector3 LastCheckpointPosition { get; private set; } = Vector3.zero;
        public Quaternion LastCheckpointRotation { get; private set; } = Quaternion.identity;
        
        private void OnTriggerEnter(Collider other)
        {
            var checkpoint = other.GetComponent<LevelCheckpoint>();
            if (checkpoint == null) return;
            if (killAllPlayers)
            {
                foreach (var player in Avatar.Players)
                {
                    player.Abilities.Get<AvatarRespawnable>()?.SetCheckpointPosition(checkpoint.SpawnLocator.transform);
                }
            }
            else
            {
                SetCheckpointPosition(checkpoint.SpawnLocator.transform);
            }
        }

        public override void OnStart()
        {
            //Game.Register(this);
            foreach (var checkpoint in FindObjectsOfType<LevelCheckpoint>())
            {
                if (checkpoint.isLevelStart)
                {
                    var checkPointTransform = checkpoint.SpawnLocator.transform;
                    LastCheckpointPosition = checkPointTransform.position;
                    LastCheckpointRotation = checkPointTransform.rotation;
                }
            }
        }

        public override void OnReset()
        {
            Motor.SetPosition(LastCheckpointPosition);
            Motor.SetRotation(LastCheckpointRotation);
            gameObject.SetActive(true);
        }

        private void SetCheckpointPosition(Transform target)
        {
            LastCheckpointPosition = target.position;
            LastCheckpointRotation = target.rotation;
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
                    var respawnable = player.Abilities.Get<AvatarRespawnable>();
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
    }
}