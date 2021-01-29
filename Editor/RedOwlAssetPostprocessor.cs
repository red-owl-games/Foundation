using System;
using System.Collections.Generic;
using System.IO;
using RedOwl.Engine;
using UnityEditor;
using UnityEditor.AssetImporters;


namespace RedOwl.Editor
{
    public interface IAssetPostprocessor
    {
        void Init();
        void HandleAdd(string filename, string filepath);
        void HandleRemove(string filename, string filepath);
        void Cleanup();
    }
    
    public class RedOwlInternalAssetPostprocessor : AssetPostprocessor
    {
        private static Dictionary<string, IAssetPostprocessor> _processors;

        private static void EnsureCache()
        {
            if (_processors != null) return;
            Log.Debug("Rebuilding AssetPostprocessor Cache");
            _processors = new Dictionary<string, IAssetPostprocessor>();
            var processors = new HashSet<Type>(TypeCache.GetTypesDerivedFrom<IAssetPostprocessor>());
            processors.IntersectWith(new HashSet<Type>(TypeCache.GetTypesWithAttribute<ScriptedImporterAttribute>()));
            foreach (var processorType in processors)
            {
                var attr = (ScriptedImporterAttribute)processorType.GetCustomAttributes(typeof(ScriptedImporterAttribute), false)[0];
                Log.Debug($"Registering Asset Postprocessor: {processorType}");
                var processor = (IAssetPostprocessor) Activator.CreateInstance(processorType);
                foreach (string fileExtension in attr.fileExtensions)
                {
                    _processors.Add(fileExtension, processor);
                }
            }
        }
        
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            EnsureCache();
            foreach (var processor in _processors.Values)
            {
                processor.Init();
            }
            foreach (string filepath in importedAssets)
            {
                Log.Debug($"Imported Asset: '{filepath}'");
                if (string.IsNullOrEmpty(filepath)) continue;
                if (_processors.TryGetValue(Path.GetExtension(filepath).Replace(".", ""), out IAssetPostprocessor processor))
                {
                    Log.Debug($"Postprocessor handling add for: '{filepath}'");
                    processor.HandleAdd(Path.GetFileNameWithoutExtension(filepath), filepath);
                }
            }
            foreach (string filepath in deletedAssets)
            {
                Log.Debug($"Deleted Asset: '{filepath}'");
                if (string.IsNullOrEmpty(filepath)) continue;
                if (_processors.TryGetValue(Path.GetExtension(filepath).Replace(".", ""), out IAssetPostprocessor processor))
                {
                    Log.Debug($"Postprocessor handling remove for: '{filepath}'");
                    processor.HandleRemove(Path.GetFileNameWithoutExtension(filepath), filepath);
                }
            }

            for (int i = 0; i < movedAssets.Length; i++)
            {
                string filepath = movedAssets[i];
                string oldpath = movedFromAssetPaths[i];
                Log.Debug($"Moved Asset: '{filepath}' from: '{oldpath}'");
                if (string.IsNullOrEmpty(filepath) || string.IsNullOrEmpty(oldpath)) continue;
                IAssetPostprocessor processor;
                if (_processors.TryGetValue(Path.GetExtension(oldpath).Replace(".", ""), out processor))
                {
                    Log.Debug($"Postprocessor handling remove for: '{oldpath}'");
                    processor.HandleRemove(Path.GetFileNameWithoutExtension(oldpath), oldpath);
                }
                if (_processors.TryGetValue(Path.GetExtension(filepath).Replace(".", ""), out processor))
                {
                    Log.Debug($"Postprocessor handling add for: '{filepath}'");
                    processor.HandleAdd(Path.GetFileNameWithoutExtension(filepath), filepath);
                }
            }

            foreach (var processor in _processors.Values)
            {
                processor.Cleanup();
            }
        }
    }
}