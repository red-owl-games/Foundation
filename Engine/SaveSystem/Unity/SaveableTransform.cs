using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    [HideMonoScript]
    [RequireComponent(typeof(Identifiable))]
    public class SaveableTransform : MonoBehaviour, ISaveable
    {
        public string Key => this.Identifier("Transform");
        public SaveableTypes Type { get; } = SaveableTypes.Data;

        private ulong _saveId;

        private void OnEnable()
        {
            _saveId = Game.SaveSystem.Subscribe(this);
        }

        private void OnDisable()
        {
            Game.SaveSystem.Unsubscribe(_saveId);
        }

        public void Write(SaveWriter writer)
        {
            var t = transform;
            writer.Write(t.position);
            writer.Write(t.rotation);
            writer.Write(t.localScale);
        }

        public void Read(SaveReader reader)
        {
            var t = transform;
            t.position = reader.Read<Vector3>();
            t.rotation = reader.Read<Quaternion>();
            t.localScale = reader.Read<Vector3>();
        }
    }
}