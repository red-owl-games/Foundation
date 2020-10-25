using System;
using System.Collections.Generic;
using UnityEngine;

namespace RedOwl.Engine
{
    // TODO: Listing Save Files
    // TODO: Picking a Save File
    // TODO: Events
    // TODO: More Types in Reader/Writer
    // TODO: System Endian-ness https://www.codeproject.com/Articles/1130187/A-More-Powerful-BinaryReader-Writer
    
    [Command("Game.Save", "Triggers a Save of the Game - [True]")]
    public class SaveCommand : ICommand
    {
        public void Invoke(string[] args) => Game.Save(); //Force Update - args.Length <= 0 || bool.Parse(args[0]));
    }
    
    [Command("Game.Load", "Triggers a Load of the Game - [True]")]
    public class LoadCommand : ICommand
    {
        public void Invoke(string[] args) => Game.Load(); //Force update - args.Length <= 0 || bool.Parse(args[0]));
    }

    [Serializable]
    public class DataStore
    {
        private Dictionary<string, string> cache;

        public DataStore()
        {
            cache = new Dictionary<string, string>();
        }

        public T Get<T>(string key, T defaultValue)
        {
            return cache.TryGetValue(key, out string output) ? JsonUtility.FromJson<T>(output) : defaultValue;
        }

        public void Set<T>(string key, T value)
        {
            cache[key] = JsonUtility.ToJson(value);
        }
    }
    
    public partial class Game
    {
        private static ulong _saveableIds;
        public static readonly DataStore SaveMeta = new DataStore();
        public static readonly DataStore SaveData = new DataStore();
        private static readonly Dictionary<ulong, ISaveable> Saveables = new Dictionary<ulong, ISaveable>();

        public static ulong Subscribe<T>(Saveable<T> pref)
        {
            _saveableIds += 1;
            Saveables.Add(_saveableIds, pref);
            return _saveableIds;
        }

        public static void Unsubscribe(ulong id)
        {
            Saveables.Remove(id);
        }

        public static void Save()
        {
            foreach (var pref in Saveables.Values)
            {
                pref.Save();
            }
        }

        public static void Load()
        {
            foreach (var prefs in Saveables.Values)
            {    
                prefs.Load();
            }
        }
    }


}