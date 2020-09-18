using System;
using System.Collections.Generic;
using UnityEngine;

namespace RedOwl.Core
{
    // TODO: Listing Save Files
    // TODO: Picking a Save File
    // TODO: Events
    // TODO: More Types in Reader/Writer
    // TODO: System Endian-ness https://www.codeproject.com/Articles/1130187/A-More-Powerful-BinaryReader-Writer
    
    public interface ISaveData
    {
        string SaveDataId { get; }
        int SaveDataLength { get;  }
        void SaveData(SaveWriter writer);
        void LoadData(SaveReader reader);
    }

    [Serializable]
    public class SaveGameSettings : Settings<SaveGameSettings>
    {
        [SerializeField]
        private bool enabled;
        public static bool Enabled => Instance.enabled;
    }

    public static class SaveGame
    {
        public struct SaveSignal : ISignal
        {
            public bool Force;
        }
        public struct BeforeSave : ISignal { }
        public struct AfterSave : ISignal { }

        public struct LoadSignal : ISignal
        {
            public bool Force;
        }
        public struct BeforeLoad : ISignal { }
        public struct AfterLoad : ISignal { }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            Application.quitting += () =>
            {
                Telegraph.Unsubscribe<SaveSignal>(OnSave);
                Telegraph.Unsubscribe<LoadSignal>(OnLoad);
            };
            Telegraph.Subscribe<SaveSignal>(OnSave);
            Telegraph.Subscribe<LoadSignal>(OnLoad);
        }

        private static SaveGameMetafile _metafile = new SaveGameMetafile();
        private static SaveGameDatafile _datafile = new SaveGameDatafile();

        private static readonly Dictionary<string, ISaveData> Registry = new Dictionary<string, ISaveData>();
        
        private static void PullAll()
        {
            foreach (var value in Registry.Values)
            {
                _datafile.Push(value);
            }
        }

        private static void PushAll()
        {
            foreach (var value in Registry.Values)
            {
                _datafile.Pull(value);
            }
        }

        #region API
        
        public static void Register(ISaveData saveData, bool load = true)
        {
            if (!SaveGameSettings.Enabled) return;
            //Log.Always($"Registering {saveData.SaveDataId}");
            Registry.Add(saveData.SaveDataId, saveData);
            if (load) Pull(saveData);
        }

        public static void Unregister(ISaveData saveData, bool save = false)
        {
            if (!SaveGameSettings.Enabled) return;
            Registry.Remove(saveData.SaveDataId);
            if (save) Push(saveData);
        }
        
        public static void Pull(ISaveData value) => _datafile.Pull(value);
        
        public static void Push(ISaveData value) => _datafile.Push(value);

        private static void OnSave(SaveSignal signal) => Save(signal.Force);
        public static void Save(bool forceUpdate = false)
        {
            if (!SaveGameSettings.Enabled) return;
            Telegraph.Send<BeforeSave>();
            if (forceUpdate) PullAll();
            FileController.Write("saves/save.meta", _metafile);
            FileController.Write("saves/save.data", _datafile);
            Telegraph.Send<AfterSave>();
        }

        private static void OnLoad(LoadSignal signal) => Load(signal.Force);
        public static void Load(bool forceUpdate = false)
        {
            if (!SaveGameSettings.Enabled) return;
            Telegraph.Send<BeforeLoad>();
            if (forceUpdate)
            {
                FileController.Read("saves/save.data", out _datafile);
            }
            PushAll();
            Telegraph.Send<AfterLoad>();
        }
        
        #endregion
    }
}