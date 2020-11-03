using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    public enum SheetTypes
    {
        AssetPerRow,
        AssetPerSheet,
    }
    
    public interface IAssetPerRow
    {
        uint GetId();
        void Parse(IList<string> row);
    }

    public interface IAssetPerSheet
    {
        void Parse(SheetData data);
    }

    [Serializable]
    public class GoogleSheetsSettings : Settings
    {
        [FoldoutGroup("Credentials"), HideLabel, MultiLineProperty(10)]
        public string Credentials;
        
        public bool HasCredentials => !string.IsNullOrEmpty(Credentials);

        [ShowIf("HasCredentials")]
        public List<GoogleSheetsRegistration> registrations;
    }

    public partial class Game
    {
        [FoldoutGroup("Google Sheets"), SerializeField]
        private GoogleSheetsSettings googleSheetsSettings = new GoogleSheetsSettings();
        public static GoogleSheetsSettings GoogleSheetsSettings => Instance.googleSheetsSettings;
    }
}