using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Core
{
    public enum SheetTypes
    {
        AssetPerRow,
        AssetPerSheet,
    }
    
    public interface IAssetPerRow
    {
        uint GetId();
        void Parse(SheetRowData row);
    }

    public interface IAssetPerSheet
    {
        void Parse(SheetData data);
    }

    [Serializable]
    public class GoogleSheetsSettings : Settings<GoogleSheetsSettings>
    {
        [Multiline(15), FoldoutGroup("Credentials"), HideLabel]
        public string Credentials;

        internal static string Creds => Instance.Credentials;

        private bool HasCredentials => !string.IsNullOrEmpty(Credentials);
        internal static bool HasCreds => Instance.HasCredentials;

        [ShowIf("HasCredentials")]
        public List<GoogleSheetsRegistration> registrations;
    }
}