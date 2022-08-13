using System;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace RedOwl.Engine
{
    public static partial class Game
    {
        public static bool IsRunning => Application.isPlaying;

        public static Random RNG => new Random((uint)DateTimeOffset.Now.ToUnixTimeSeconds());
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnBeforeSceneLoad()
        {
            Application.targetFrameRate = 60;
        }

        public static FileController FileController { get; set; } = new();

        public static FileController FileControllerEnc { get; set; } = FileController.Encrypted;

        internal static FileController FileControllerInternal { get; set; } = FileController.Internal;

        public static void Save(string filepath, string data, bool encrypted = false) => 
            (encrypted ? FileControllerEnc : FileController).Write(filepath, data);
        
        public static void Save<T>(string filepath, T data, bool encrypted = false) => 
            (encrypted ? FileControllerEnc : FileController).Write(filepath, data);

        public static T Load<T>(string filepath, bool encrypted = false) => 
            (encrypted ? FileControllerEnc : FileController).Read<T>(filepath);
    }
}