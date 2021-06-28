using System;
using System.Collections.Generic;
using UnityEngine;

namespace RedOwl.Engine
{
    public class ModelIdentifier
    {
        public string kind;
        public int id;
        public string key;
        public Dictionary<string, object> spec;
    }
    
    public abstract class Database<TModel, TData> where TModel : Model<TData> where TData : struct
    {
        public Dictionary<string, int> Lookup { get; }
        public Dictionary<int, TData> Cache { get; }
        
        public IEnumerable<int> Ids => Cache.Keys;
        public IEnumerable<string> Keys => Lookup.Keys;
        public IEnumerable<TData> All => Cache.Values;
        public TData Get(int id) => Cache[id];
        public TData Get(string key) => Cache[Lookup[key]];

        protected abstract string Tag { get; }

        protected Database()
        {
            Lookup = new Dictionary<string, int>();
            Cache = new Dictionary<int, TData>();
            if (Game.IsRunning) return;
            Preload();
        }

        public void Preload()
        {
            Lookup.Clear();
            Cache.Clear();
            AssetTools.LoadAll<TModel>(Tag, true, HandleLoadScriptableObject);
            AssetTools.LoadAll<TextAsset>(Tag, true, HandleLoadTextAssets);
        }

        private void HandleLoadScriptableObject(IList<TModel> results)
        {
            foreach (var asset in results)
            {
                try
                {
                    Lookup.Add(asset.Key, asset.Id);
                    Cache.Add(asset.Id, asset.data);
                }
                catch (Exception e)
                {
                    Log.Error($"Error adding '{asset.name}' with key '{asset.Key}' and id '{asset.Id}' to database '{GetType().FullName}'\n{e}");
                }
            }
        }

        private void HandleLoadTextAssets(IList<TextAsset> results)
        {
            foreach (var asset in results)
            {
                var identifier = AssetTools.Deserializer.Deserialize<ModelIdentifier>(asset.text);
                if (identifier.kind == typeof(TData).Name)
                {
                    try
                    {
                        Lookup.Add(identifier.key, identifier.id);
                        Cache.Add(identifier.id,
                            AssetTools.Deserializer.Deserialize<TData>(
                                AssetTools.Serializer.Serialize(identifier.spec)));
                    }
                    catch (Exception e)
                    {
                        Log.Error($"Error adding '{asset.name}' of kind '{identifier.kind}' with key '{identifier.key}' and id '{identifier.id}' to database '{GetType().FullName}'\n{e}");
                    }
                }
            }
        }
    }
    
    public abstract class Database<TSelf, TModel, TData> : Database<TModel, TData> where TSelf : Database<TModel, TData>, new() where TModel : Model<TData> where TData : struct
    {
        public static TSelf Instance => Game.FindOrBind<TSelf>();
        
        public new static IEnumerable<int> Ids => Instance.Ids;
        public new static IEnumerable<string> Keys => Instance.Keys;
        public new static IEnumerable<TData> All => Instance.All;
        public new static TData Get(int id) => Instance.Get(id);
        public new static TData Get(string key) => Instance.Get(key);
    }
}