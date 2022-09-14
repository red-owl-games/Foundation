using System;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace RedOwl.Engine
{
    [Serializable]
    public partial class Game : SystemBase<Game>
    {
        public static bool IsRunning => Application.isPlaying;

        public static Random RNG => new Random((uint)DateTimeOffset.Now.ToUnixTimeSeconds());

        public Ticker EveryHalfSecond = 0.5f;
        public Ticker EverySecond = 1f;
        public Ticker EveryOtherSecond = 2f;
        
        protected override void OnCreate()
        {
            base.OnCreate();
            Load();
        }

        protected override void OnUpdate(float dt)
        {
            base.OnUpdate(dt);
            EveryHalfSecond.Tick(dt);
            EverySecond.Tick(dt);
            EveryOtherSecond.Tick(dt);
        }

        internal const string DataLocation = "Game/Resources/Config/Game.yaml";
        public void Load()
        {
            FileController.InstanceInternal.Read(DataLocation, this);
        }
    }
}