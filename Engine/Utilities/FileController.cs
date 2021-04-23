using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    public interface IRedOwlFile : ISerializationCallbackReceiver {}
    
    #region Settings
    
    [Serializable]
    public class FileControllerSettings : Settings
    {
        [SerializeField]
        public bool UseEncryption;

        [SerializeField]
        public bool UseCompression;

        [SerializeField]
        public string EncryptionKey;

        [SerializeField]
        public string EncryptionIV;
    }
    
    public partial class GameSettings
    {
        [FoldoutGroup("File Controller"), SerializeField]
        private FileControllerSettings fileControllerSettings = new FileControllerSettings();
        public static FileControllerSettings FileControllerSettings => Instance.fileControllerSettings;
    }
    
    #endregion
    
    [Serializable]
    public struct PersistenceItem
    {
        public string key;
        public byte[] value;
    }
    
    [Serializable]
    public class DataFile : IRedOwlFile
    {
        [SerializeField] 
        private List<PersistenceItem> data;
        private Dictionary<string, MemoryStream> cache;

        public DataFile(int capacity = 100)
        {
            data = new List<PersistenceItem>(capacity);
            cache = new Dictionary<string, MemoryStream>(capacity);
        }

        public MemoryStream Allocate(string key)
        {
            cache[key] = new MemoryStream();
            return cache[key];
        }

        public bool Get(string key, out MemoryStream stream)
        {
            return cache.TryGetValue(key, out stream);
        }

        public void OnBeforeSerialize()
        {
            if (data == null) data = new List<PersistenceItem>(cache.Count);
            else data.Clear();
            foreach (var kvp in cache)
            {
                data.Add(new PersistenceItem{ key = kvp.Key, value = kvp.Value.ToArray() });
            }
        }

        public void OnAfterDeserialize()
        {
            if (cache == null) cache = new Dictionary<string, MemoryStream>(data.Count);
            else cache.Clear();
            foreach (var item in data)
            {
                cache[item.key] = new MemoryStream(item.value);
            }
        }
    }

    public static class FileController
    {
        private static string Filepath(string filename) => $"{Application.persistentDataPath}/{filename}";
        
        private static AesCryptoServiceProvider _provider;

        private static AesCryptoServiceProvider Provider =>
            _provider ?? (_provider = new AesCryptoServiceProvider
            {
                Key = Encoding.ASCII.GetBytes(GameSettings.FileControllerSettings.EncryptionKey.PadRight(16, 'X').Substring(0, 16)),
                IV = Encoding.ASCII.GetBytes(GameSettings.FileControllerSettings.EncryptionIV.PadRight(16, 'X').Substring(0, 16))
            });
        
        #region Read
        
        public static void Read<T>(string relativePath, out T file) where T : IRedOwlFile
        {
            file = RedOwlTools.FromBytes<T>(Read(relativePath));
        }
        
        public static byte[] Read(string relativePath)
        {
            string filepath = Filepath(relativePath);
            return !File.Exists(filepath) ? new byte[0] : DoRead(filepath);
        }
        
        private static byte[] DoRead(string filepath)
        {
            //Log.Always($"Reading File - {filepath}");
            Stream stream = new FileStream(filepath, FileMode.Open);
            if (GameSettings.FileControllerSettings.UseEncryption) stream = new CryptoStream(stream, Provider.CreateDecryptor(), CryptoStreamMode.Read);
            if (GameSettings.FileControllerSettings.UseCompression) stream = new DeflateStream(stream, CompressionMode.Decompress);
            using (var reader = new MemoryStream())
            {
                stream.CopyTo(reader);
                stream.Dispose();
                return reader.GetBuffer();
            }
        }
        
        #endregion
        
        #region Write
        
        public static void Write<T>(string relativePath, T file) where T : IRedOwlFile
        {
            Write(relativePath, RedOwlTools.ToBytes(file));
        }
        
        public static void Write(string relativePath, byte[] data)
        {
            string filepath = Filepath(relativePath);
            DoWrite(filepath, data);
        }
        
        private static void DoWrite(string filepath, byte[] data)
        {
            //Log.Always($"Writing File - {filepath}");
            Directory.CreateDirectory(Path.GetDirectoryName(filepath) ?? string.Empty);
            Stream stream = new FileStream(filepath, FileMode.Create);
            if (GameSettings.FileControllerSettings.UseEncryption) stream = new CryptoStream(stream, Provider.CreateEncryptor(), CryptoStreamMode.Write);
            if (GameSettings.FileControllerSettings.UseCompression) stream = new DeflateStream(stream, CompressionMode.Compress);
            stream.Write(data, 0, data.Length);
            stream.Dispose();
        }
        
        #endregion
    }
}