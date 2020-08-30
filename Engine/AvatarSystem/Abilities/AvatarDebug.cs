using UnityEngine;

namespace RedOwl.Core
{
    public class AvatarDebug : AvatarAbility, ISaveData
    {
        public override int Priority { get; } = 0;

        public int Value1;
        public Vector3 Value2;

        public override void OnStart()
        {
            SaveGame.Register(this);
        }

        public override void OnCleanup()
        {
            SaveGame.Unregister(this);
        }

        public override void HandleInput(ref AvatarInput input)
        {
            // TODO: For Debug Only
            if (input.Get(AvatarInputButtons.ShoulderLeft) == ButtonStates.Pressed) SaveGame.Save(true);
            if (input.Get(AvatarInputButtons.ShoulderRight) == ButtonStates.Pressed) SaveGame.Load(true);
            if (input.Get(AvatarInputButtons.South) == ButtonStates.Pressed) SaveGame.Load();
        }

        public string SaveDataId => $"{name}.{GetType()}";

        public int SaveDataLength => 16;

        public void SaveData(SaveWriter writer)
        {
            writer.Write(Value1);
            writer.Write(Value2);
        }

        public void LoadData(SaveReader reader)
        {
            Value1 = reader.ReadInt32();
            Value2 = reader.ReadVector3();
        }
    }
}