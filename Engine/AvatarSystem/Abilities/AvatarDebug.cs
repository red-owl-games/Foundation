using UnityEngine;

namespace RedOwl.Core
{
    public class AvatarDebug : AvatarAbility, IPersistData
    {
        public override int Priority { get; } = 0;

        public int Value1;
        public Vector3 Value2;

        public override void OnStart()
        {
            Game.Register(this);
        }

        public override void OnCleanup()
        {
            Game.Unregister(this);
        }

        public override void HandleInput(ref AvatarInput input)
        {
            // TODO: For Debug Only
            if (input.Get(AvatarInputButtons.ShoulderLeft) == ButtonStates.Pressed) Game.Save(true);
            if (input.Get(AvatarInputButtons.ShoulderRight) == ButtonStates.Pressed) Game.Load(true);
            if (input.Get(AvatarInputButtons.South) == ButtonStates.Pressed) Game.Load();
        }

        public PersistenceTypes SaveDataPersistenceType => PersistenceTypes.SaveFile;
        public string SaveDataId => $"{name}.{GetType()}";
        public int SaveDataLength => 16;

        public void SaveData(PersistenceWriter writer)
        {
            writer.Write(Value1);
            writer.Write(Value2);
        }

        public void LoadData(PersistenceReader reader)
        {
            Value1 = reader.ReadInt32();
            Value2 = reader.ReadVector3();
        }
    }
}