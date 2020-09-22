using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

namespace RedOwl.Core
{
    public class AvatarRespawnable : AvatarAbility, IPersistData
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
            Game.Register(this);
            foreach (var checkpoint in FindObjectsOfType<Checkpoint>())
            {
                if (checkpoint.isLevelStart) LastCheckpointPosition = checkpoint.transform.position;
            }
            Respawn();
        }

        public override void OnCleanup()
        {
            Game.Unregister(this);
        }
        
        private void SetCheckpointPosition(Transform target)
        {
            LastCheckpointPosition = target.position;
            Game.Push(this);
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
    }
}