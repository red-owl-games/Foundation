using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace RedOwl.Core
{
    public interface IRedOwlFile : ISerializationCallbackReceiver {}
    
    #region Settings
    
    [Serializable]
    public class FileControllerSettings : Settings<FileControllerSettings>
    {
        [SerializeField]
        private bool useEncryption;
        public static bool UseEncryption => Instance.useEncryption;
        
        [SerializeField]
        private bool useCompression;
        public static bool UseCompression => Instance.useCompression;
        
        [SerializeField]
        private string encryptionKey;
        public static string EncryptionKey => Instance.encryptionKey;
        
        [SerializeField]
        private string encryptionIV;
        public static string EncryptionIV => Instance.encryptionIV;
    }
    
    #endregion

    public static class FileController
    {
        private static string Filepath(string filename) => $"{Application.persistentDataPath}/{filename}";
        
        private static AesCryptoServiceProvider _provider;

        private static AesCryptoServiceProvider Provider =>
            _provider ?? (_provider = new AesCryptoServiceProvider
            {
                Key = Encoding.ASCII.GetBytes(FileControllerSettings.EncryptionKey.PadRight(16, 'X').Substring(0, 16)),
                IV = Encoding.ASCII.GetBytes(FileControllerSettings.EncryptionIV.PadRight(16, 'X').Substring(0, 16))
            });
        
        #region Read
        
        public static void Read<T>(string relativePath, out T file) where T : IRedOwlFile
        {
            file = JsonUtility.FromJson<T>(Encoding.ASCII.GetString(Read(relativePath)));
            file.OnAfterDeserialize();
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
            if (FileControllerSettings.UseEncryption) stream = new CryptoStream(stream, Provider.CreateDecryptor(), CryptoStreamMode.Read);
            if (FileControllerSettings.UseCompression) stream = new DeflateStream(stream, CompressionMode.Decompress);
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
            file.OnBeforeSerialize();
            Write(relativePath, Encoding.UTF8.GetBytes(JsonUtility.ToJson(file)));
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
            if (FileControllerSettings.UseEncryption) stream = new CryptoStream(stream, Provider.CreateEncryptor(), CryptoStreamMode.Write);
            if (FileControllerSettings.UseCompression) stream = new DeflateStream(stream, CompressionMode.Compress);
            stream.Write(data, 0, data.Length);
            stream.Dispose();
        }
        
        #endregion
    }
}