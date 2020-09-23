using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor.Experimental.AssetImporters;
using UnityEngine;

namespace RedOwl.Core.Editor
{
    [HideMonoScript]
    public abstract class RedOwlAssetImporter<T> : ScriptedImporter where T : RedOwlAsset
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            Log.Info($"Processing Asset: '{ctx.assetPath}'");
            var asset = ScriptableObject.CreateInstance<T>();
            asset.ProcessData(File.ReadAllLines(ctx.assetPath).Select(line => new List<string>(line.Split(','))).ToList());
            ctx.AddObjectToAsset("main obj", asset);
            ctx.SetMainObject(asset);
        }
    }
}