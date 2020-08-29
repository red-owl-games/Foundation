using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Core
{
    public interface IRedOwlFile : ISerializationCallbackReceiver {}
    
    #region Settings
    
    public partial class RedOwlSettings
    {
        [BoxGroup("File Controller")]
        [SerializeField]
        private bool useEncryption;
        public static bool UseEncryption => I.useEncryption;
        
        [BoxGroup("File Controller")]
        [SerializeField]
        private bool useCompression;
        public static bool UseCompression => I.useCompression;
        
        [BoxGroup("File Controller")]
        [SerializeField]
        private string encryptionKey;
        public static string EncryptionKey => I.encryptionKey;
        
        [BoxGroup("File Controller")]
        [SerializeField]
        private string encryptionIV;
        public static string EncryptionIV => I.encryptionIV;
    }
    
    #endregion

    public static class FileController
    {
        private static string Filepath(string filename) => $"{Application.persistentDataPath}/{filename}";

        private static AesCryptoServiceProvider _provider;

        private static AesCryptoServiceProvider Provider =>
            _provider ?? (_provider = new AesCryptoServiceProvider
            {
                Key = Encoding.ASCII.GetBytes(RedOwlSettings.EncryptionKey.PadRight(16, 'X').Substring(0, 16)),
                IV = Encoding.ASCII.GetBytes(RedOwlSettings.EncryptionIV.PadRight(16, 'X').Substring(0, 16))
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
            if (RedOwlSettings.UseEncryption) stream = new CryptoStream(stream, Provider.CreateDecryptor(), CryptoStreamMode.Read);
            if (RedOwlSettings.UseCompression) stream = new DeflateStream(stream, CompressionMode.Decompress);
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
            if (RedOwlSettings.UseEncryption) stream = new CryptoStream(stream, Provider.CreateEncryptor(), CryptoStreamMode.Write);
            if (RedOwlSettings.UseCompression) stream = new DeflateStream(stream, CompressionMode.Compress);
            stream.Write(data, 0, data.Length);
            stream.Dispose();
        }
        
        #endregion
    }
}