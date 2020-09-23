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

    [Command("Game.Save", "Triggers a Save of the Game - [True]")]
    public class SaveCommand : ICommand
    {
        public void Invoke(string[] args) => Game.Save(args.Length <= 0 || bool.Parse(args[0]));
    }
    
    [Command("Game.Load", "Triggers a Load of the Game - [True]")]
    public class LoadCommand : ICommand
    {
        public void Invoke(string[] args) => Game.Load(args.Length <= 0 || bool.Parse(args[0]));
    }
    
    public enum PersistenceTypes
    {
        Preferences,
        SaveFile
    }
    
    public interface IPersistData
    {
        PersistenceTypes SaveDataPersistenceType { get; }
        string SaveDataId { get; }
        int SaveDataLength { get;  }
        void SaveData(PersistenceWriter writer);
        void LoadData(PersistenceReader reader);
    }

    [Serializable]
    public class SaveGameSettings : Settings<SaveGameSettings>
    {
        [SerializeField]
        private bool enabled;
        public static bool Enabled => Instance.enabled;
    }

    public static partial class Game
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
        private static void InitializeSaveSystem()
        {
            Application.quitting += () =>
            {
                Telegraph.Unsubscribe<SaveSignal>(OnSave);
                Telegraph.Unsubscribe<LoadSignal>(OnLoad);
            };
            Telegraph.Subscribe<SaveSignal>(OnSave);
            Telegraph.Subscribe<LoadSignal>(OnLoad);
        }
        private static void OnSave(SaveSignal signal) => Save(signal.Force);
        private static void OnLoad(LoadSignal signal) => Load(signal.Force);

        private static GameDatafile _prefsFile = new GameDatafile();
        private static GameMetafile _metafile = new GameMetafile();
        private static GameDatafile _datafile = new GameDatafile();

        private static readonly Dictionary<string, IPersistData> ForPreferences = new Dictionary<string, IPersistData>();
        private static readonly Dictionary<string, IPersistData> ForSaveGame = new Dictionary<string, IPersistData>();

        #region API
        
        public static void Register(IPersistData value, bool load = true)
        {
            if (!SaveGameSettings.Enabled) return;
            //Log.Always($"Registering {saveData.SaveDataId}");
            switch (value.SaveDataPersistenceType)
            {
                case PersistenceTypes.Preferences:
                    ForPreferences.Add(value.SaveDataId, value);
                    break;
                case PersistenceTypes.SaveFile:
                    ForSaveGame.Add(value.SaveDataId, value);
                    break;
            }
            if (load) Pull(value);
        }

        public static void Unregister(IPersistData value, bool save = false)
        {
            if (!SaveGameSettings.Enabled) return;
            switch (value.SaveDataPersistenceType)
            {
                case PersistenceTypes.Preferences:
                    ForPreferences.Remove(value.SaveDataId);
                    break;
                case PersistenceTypes.SaveFile:
                    ForSaveGame.Remove(value.SaveDataId);
                    break;
            }
            if (save) Push(value);
        }
        
        public static void Pull(IPersistData value)
        {
            switch (value.SaveDataPersistenceType)
            {
                case PersistenceTypes.Preferences:
                    _prefsFile.Pull(value);
                    break;
                case PersistenceTypes.SaveFile:
                    _datafile.Pull(value);
                    break;
            }
        }

        public static void Push(IPersistData value)
        {
            switch (value.SaveDataPersistenceType)
            {
                case PersistenceTypes.Preferences:
                    _prefsFile.Push(value);
                    break;
                case PersistenceTypes.SaveFile:
                    _datafile.Push(value);
                    break;
            }
        }
        
        public static void Save(bool forceUpdate = false)
        {
            if (!SaveGameSettings.Enabled) return;
            Telegraph.Send<BeforeSave>();
            if (forceUpdate)
            {
                foreach (var value in ForPreferences.Values) _prefsFile.Push(value);
                foreach (var value in ForSaveGame.Values) _datafile.Push(value);
            }
            FileController.Write("prefs.data", _prefsFile);
            FileController.Write("saves/save.meta", _metafile);
            FileController.Write("saves/save.data", _datafile);
            Telegraph.Send<AfterSave>();
        }

        public static void Load(bool forceUpdate = false)
        {
            if (!SaveGameSettings.Enabled) return;
            Telegraph.Send<BeforeLoad>();
            if (forceUpdate)
            {
                FileController.Read("prefs.data", out _prefsFile);
                FileController.Read("saves/save.data", out _datafile);
            }
            foreach (var value in ForPreferences.Values) _prefsFile.Pull(value);
            foreach (var value in ForSaveGame.Values) _datafile.Pull(value);
            Telegraph.Send<AfterLoad>();
        }
        
        #endregion
    }
}