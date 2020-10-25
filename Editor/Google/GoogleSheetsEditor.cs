using System.Collections.Generic;
using System.Linq;
using RedOwl.Engine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace RedOwl.Editor
{
    public class GoogleSheetsRegistrationProcessor : OdinPropertyProcessor<GoogleSheetsRegistration>
    {
        public override void ProcessMemberProperties(List<InspectorPropertyInfo> propertyInfos)
        {
            propertyInfos.AddDelegate(
                "Generate",
                () => Generate((GoogleSheetsRegistration)Property.ValueEntry.WeakSmartValue));
        }

        internal static void Generate(GoogleSheetsRegistration value)
        {
            if (!value.HasId()) return;
            value.Poll();
            switch (value.SheetType)
            {
                //case SheetTypes.AssetPerRow:
                //BuildPerRow(value);
                //break;
                case SheetTypes.AssetPerSheet:
                BuildPerSheet(value);
                break;
            }
        }

        private static GoogleSheetsDatabase GetDb(GoogleSheetsRegistration value)
        {
            string id = $"{value.DataClass}.{value.Id}";
            string dbFolder = $"Assets/{value.Folder}";
            foreach (string guid in AssetDatabase.FindAssets($"t:GoogleSheetsDatabase", new[] {dbFolder}))
            {
                var asset = AssetDatabase.LoadAssetAtPath<GoogleSheetsDatabase>(AssetDatabase.GUIDToAssetPath(guid));
                if (!(asset is GoogleSheetsDatabase db)) continue;
                if (db.Id == id)
                    return db;
            }
            //Then we never found one so we should make one
            var database = ScriptableObject.CreateInstance<GoogleSheetsDatabase>();
            database.Id = id;
            AssetDatabase.CreateAsset(database, $"{dbFolder}/{value.DataClass}_SheetsDB.asset");
            return database;
        }

        /*
        private static void BuildPerRow(GoogleSheetsRegistration value)
        {
            Log.Info($"Building Asset Data for '{value.Manager.Title} - {value.Manager[value.Sheet].Title}' using data class '{value.DataClass}' per row");
            var db = GetDb(value);
            var sheet = value.Manager[value.Sheet];

            var current = sheet.ToDictionary(row => uint.Parse(row[value.IdColumn].ToString()));
            var old = db.Objects.ToDictionary(obj => ((IAssetPerRow)obj).GetId());
            var keepers = new List<ScriptableObject>(current.Count);

            foreach (var asset in db.Objects)
            {
                var item = (IAssetPerRow) asset;
                var key = item.GetId();
                if (current.TryGetValue(key, out SheetRowData data))
                {
                    // Update
                    item.Parse(data);
                    EditorUtility.SetDirty(asset);
                    current.Remove(key);
                    old.Remove(key);
                    keepers.Add(asset);
                }
            }

            foreach (var row in current.Values)
            {
                // Add
                var asset = ScriptableObject.CreateInstance(value.DataClass);
                ((IAssetPerRow)asset).Parse(row);
                AssetDatabase.CreateAsset(asset, $"Assets/{value.Folder}/{row[value.NameColumn]}.asset");
                db.Objects.Add(asset);
                keepers.Add(asset);
            }
            
            foreach (var asset in old.Values)
            {
                // Remove
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(asset));
            }

            db.Objects = keepers;
            EditorUtility.SetDirty(db);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        */

        private static void BuildPerSheet(GoogleSheetsRegistration value)
        {
            Log.Info($"Building Asset Data for '{value.Manager.Title}' using data class '{value.DataClass}' per sheet");
            var db = GetDb(value);
            var current = value.Manager.ToDictionary(sheet => sheet.Title);
            var old = db.Objects.ToDictionary(obj => obj.name);
            var keepers = new List<ScriptableObject>(current.Count);
            
            foreach (var asset in db.Objects)
            {
                var item = (IAssetPerSheet) asset;
                var key = asset.name;
                if (current.TryGetValue(key, out SheetData data))
                {
                    // Update
                    item.Parse(data);
                    EditorUtility.SetDirty(asset);
                    current.Remove(key);
                    old.Remove(key);
                    keepers.Add(asset);
                }
            }

            foreach (var sheet in current.Values)
            {
                // Add
                var asset = ScriptableObject.CreateInstance(value.DataClass);
                ((IAssetPerSheet)asset).Parse(sheet);
                AssetDatabase.CreateAsset(asset, $"Assets/{value.Folder}/{sheet.Title}.asset");
                db.Objects.Add(asset);
                keepers.Add(asset);
            }
            
            foreach (var asset in old.Values)
            {
                // Remove
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(asset));
            }
            
            db.Objects = keepers;
            EditorUtility.SetDirty(db);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

    public class GoogleSheetsConfigProcessor : OdinPropertyProcessor<GoogleSheetsSettings>
    {
        public override void ProcessMemberProperties(List<InspectorPropertyInfo> propertyInfos)
        {
            propertyInfos.AddDelegate(
                "Generate All",
                () => Generate((GoogleSheetsSettings)Property.ValueEntry.WeakSmartValue),
                new ButtonAttribute(ButtonSizes.Large), new PropertyOrderAttribute(-10));
        }

        private static void Generate(GoogleSheetsSettings value)
        {
            foreach (var registration in value.registrations)
            {
                GoogleSheetsRegistrationProcessor.Generate(registration);
            }
        }
    }

    public static class GoogleSheetsEditor
    {
        public static void Refresh(string id)
        {
            var info = id.Split('.');
            foreach (var registration in Game.GoogleSheetsSettings.registrations)
            {
                if (registration.Id == info[1] && registration.DataClass == info[0])
                    GoogleSheetsRegistrationProcessor.Generate(registration);
            }
        }

        public static void Refresh(ScriptableObject obj)
        {
            string assetPath = AssetDatabase.GetAssetPath(obj);
            string folderPath = assetPath.Replace($"/{obj.name}.asset", "");
            foreach (string guid in AssetDatabase.FindAssets($"t:GoogleSheetsDatabase", new[] {folderPath}))
            {
                var asset = AssetDatabase.LoadAssetAtPath<GoogleSheetsDatabase>(AssetDatabase.GUIDToAssetPath(guid));
                if (!(asset is GoogleSheetsDatabase db)) continue;
                if (db.Objects.All(dbObject => dbObject != obj)) continue;
                Refresh(db.Id);
                break;
            }
        }
    }
}