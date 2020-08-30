using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

namespace RedOwl.Core
{
    public class AvatarRespawnable : AvatarAbility, ISaveData
    {
        public override int Priority { get; } = 10000;
        
        public Vector3 LastCheckpointPosition { get; private set; } = Vector3.zero;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Checkpoint>() != null)
            {
                SetCheckpointPosition(other.transform);
            }
        }

        public override void OnStart()
        {
            SaveGame.Register(this);
            foreach (var checkpoint in FindObjectsOfType<Checkpoint>())
            {
                if (checkpoint.isLevelStart) LastCheckpointPosition = checkpoint.transform.position;
            }
            Respawn();
        }

        public override void OnCleanup()
        {
            SaveGame.Unregister(this);
        }
        
        private void SetCheckpointPosition(Transform target)
        {
            LastCheckpointPosition = target.position;
            SaveGame.Push(this);
        }

        [Button]
        public void Respawn()
        {
            Motor.SetPosition(LastCheckpointPosition);
            gameObject.SetActive(true);
        }
        
        [Button]
        public void Kill()
        {
            Motor.BaseVelocity = Vector3.zero;
            gameObject.SetActive(false);
        }
        
        public string SaveDataId => $"{name}.{GetType()}";

        public int SaveDataLength => 16;

        public void SaveData(SaveWriter writer)
        {
            writer.Write((float3)LastCheckpointPosition);
        }

        public void LoadData(SaveReader reader)
        {
            LastCheckpointPosition = reader.ReadVector3();
            Respawn();
        }
    }
}