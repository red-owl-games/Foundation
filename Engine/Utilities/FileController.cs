using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    public interface IRedOwlFile
    {
        string Directory { get; }
        string Filename { get; }
        string Extension { get; }
        int LatestVersion { get; }
        void BeginSerialize(RedOwlSerializer serializer);
    }
    
    #region Settings
    
    [Serializable, InlineProperty, HideLabel]
    public class FileControllerSettings
    {
        [SerializeField]
        public bool UseBuffer = false;
        [SerializeField, ShowIf("UseBuffer")] 
        public int BufferSize = 1024;

        [SerializeField]
        public RedOwlSerializer.Formats Format = RedOwlSerializer.Formats.Binary;
        
        [SerializeField]
        public bool UseCompression;
        
        [SerializeField]
        public bool UseEncryption;

        [SerializeField, ShowIf("UseEncryption")]
        public string EncryptionKey;

        [SerializeField, ShowIf("UseEncryption")]
        public string EncryptionIV;
    }
    
    public partial class GameSettings
    {
        [FoldoutGroup("File Controller"), SerializeField]
        private FileControllerSettings fileControllerSettings = new FileControllerSettings();
        public static FileControllerSettings FileControllerSettings => Instance.fileControllerSettings;
    }
    
    #endregion

    public static class FileController
    {
        public static string Filepath<T>(T file) where T : IRedOwlFile =>
            $"{Application.persistentDataPath}/{file.Directory}/{file.Filename}.{file.Extension}";
        
        private static AesCryptoServiceProvider _provider;

        private static AesCryptoServiceProvider Provider =>
            _provider ?? (_provider = new AesCryptoServiceProvider
            {
                Key = Encoding.ASCII.GetBytes(GameSettings.FileControllerSettings.EncryptionKey.PadRight(16, 'X').Substring(0, 16)),
                IV = Encoding.ASCII.GetBytes(GameSettings.FileControllerSettings.EncryptionIV.PadRight(16, 'X').Substring(0, 16))
            });
        
        #region Read

        public static void Read<T>(T file) where T : IRedOwlFile
        {
            var settings = GameSettings.FileControllerSettings;
            string filepath = Filepath(file);
            string filepathBackup = $"{filepath}.bak";
            if (!File.Exists(filepath))
            {
                if (!File.Exists(filepathBackup)) return;
                File.Move(filepathBackup, filepath);
            }
            Log.Debug($"Reading File - {filepath}");
            Stream stream = new FileStream(filepath, FileMode.Open);
            if (settings.UseEncryption) stream = new CryptoStream(stream, Provider.CreateDecryptor(), CryptoStreamMode.Read);
            if (settings.UseCompression) stream = new DeflateStream(stream, CompressionMode.Decompress);
            if (settings.UseBuffer) stream = new BufferedStream(stream, settings.BufferSize);
            var serializer = new RedOwlSerializer(stream, file.LatestVersion, settings.Format);
            try
            {
                file.BeginSerialize(serializer);
            }
            finally
            {
                serializer.Dispose();
            }
        }

        #endregion
        
        #region Write

        public static void Write<T>(T file) where T : IRedOwlFile
        {
            var settings = GameSettings.FileControllerSettings;
            string filepath = Filepath(file);
            string filepathBackup = $"{filepath}.bak";
            Log.Debug($"Writing File - {filepath}");
            Directory.CreateDirectory(Path.GetDirectoryName(filepath) ?? string.Empty);
            if (File.Exists(filepath))
            {
                if (File.Exists(filepathBackup)) File.Delete(filepathBackup);
                File.Move(filepath, filepathBackup);
            }
            Stream stream = new FileStream(filepath, FileMode.Create);
            if (settings.UseEncryption) stream = new CryptoStream(stream, Provider.CreateEncryptor(), CryptoStreamMode.Write);
            if (settings.UseCompression) stream = new DeflateStream(stream, CompressionMode.Compress);
            if (settings.UseBuffer) stream = new BufferedStream(stream, settings.BufferSize);
            var serializer = new RedOwlSerializer(stream, file.LatestVersion, settings.Format, true);
            try
            {
                file.BeginSerialize(serializer);
                serializer.Dispose();
            }
            catch (Exception)
            {
                serializer.Dispose();
                if (File.Exists(filepath)) File.Delete(filepath);
                if (File.Exists(filepathBackup)) File.Move(filepathBackup, filepath);
            }
        }

        #endregion

        public static T List<T>(T file) where T : IRedOwlFile
        {
            // TODO: not sure how to do this given our customization of Directory/Filename
            // If we nest save meta files in a directory we need to do a double listing
            throw new NotImplementedException();
        }

        public static void Erase<T>(T file) where T : IRedOwlFile
        {
            string filepath = Filepath(file);
            string filepathBackup = $"{filepath}.bak";
            if (File.Exists(filepath))
            {
                Log.Debug($"Deleting File - {filepath}");
                File.Delete(filepath);
            }
            if (File.Exists(filepathBackup))
            {
                Log.Debug($"Deleting File - {filepathBackup}");
                File.Delete(filepathBackup);
            }
        }
    }
}