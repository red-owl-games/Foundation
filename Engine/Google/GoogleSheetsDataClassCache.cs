using System;
using System.Collections.Generic;
using UnityEngine;

namespace RedOwl.Core
{
    public static class GoogleSheetsDataClassCache
    {
        private static Dictionary<string, Type> _perRowCache;
        private static Dictionary<string, Type> _perSheetCache;
        
        public static List<string> Get(SheetTypes sheetType)
        {
            ShouldBuildCache();
            switch (sheetType)
            {
                case SheetTypes.AssetPerRow:
                    return new List<string>(_perRowCache.Keys);
                case SheetTypes.AssetPerSheet:
                    return new List<string>(_perSheetCache.Keys);
                default:
                    return default;
            }
        }

        public static Type Get(SheetTypes sheetTypes, string name)
        {
            switch (sheetTypes)
            {
                case SheetTypes.AssetPerRow:
                    return _perRowCache[name];
                case SheetTypes.AssetPerSheet:
                    return _perSheetCache[name];
                default:
                    return null;
            }
        }

        private static void ShouldBuildCache()
        {
            if (_perRowCache == null) BuildCache();
        }

        private static void BuildCache()
        {
            _perRowCache = new Dictionary<string, Type>();
            _perSheetCache = new Dictionary<string, Type>();
            
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (!type.IsClass || !typeof(ScriptableObject).IsAssignableFrom(type)) continue;
                    if (typeof(IAssetPerRow).IsAssignableFrom(type)) {
                        _perRowCache.Add(type.Name, type);
                    }
                    if (typeof(IAssetPerSheet).IsAssignableFrom(type)) {
                        _perSheetCache.Add(type.Name, type);
                    }
                }
            }
        }
    }
}