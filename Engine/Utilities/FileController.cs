using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Ionic.Zlib;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    public interface IRedOwlFile
    {
        string Filepath { get; }
    }

    public abstract class Datafile : IRedOwlFile
    {
        public abstract string Filepath { get; }
        
        public void Save() => 
            Game.FileController.Write(this);

        public void Load() => 
            Game.FileController.Read(this);
    }
    
    public abstract class DatafileEnc : IRedOwlFile
    {
        public abstract string Filepath { get; }
        
        public void Save() => 
            Game.FileControllerEnc.Write(this);

        public void Load() => 
            Game.FileControllerEnc.Read(this);
    }
    
    public abstract class DatafileInternal : IRedOwlFile
    {
        public abstract string Filepath { get; }
        
        public void Save() => 
            Game.FileControllerInternal.Write(this);

        public void Load() => 
            Game.FileControllerInternal.Read(this);
    }
    
    [Serializable, InlineProperty, HideLabel]
    public class FileControllerSettings
    {
        public string basePath = Application.persistentDataPath;
        public bool useBackup = true;
        public bool useBuffer = false;
        [ShowIf("UseBuffer")] 
        public int bufferSize = 1024;

        public RedOwlSerializer.Formats format = RedOwlSerializer.Formats.Binary;

        public bool useCompression = false;

        public bool useEncryption = false;

        [ShowIf("UseEncryption")]
        public string encryptionKey = "";

        [ShowIf("UseEncryption")]
        public string encryptionIv = "";
    }
     
    public class FileController
    {
        public static FileController Encrypted => new(new FileControllerSettings()
        {
            useEncryption = true,
            encryptionKey = SystemInfo.deviceUniqueIdentifier,
            encryptionIv = SystemInfo.deviceUniqueIdentifier,
        });
        
        public static FileController Internal => new(new FileControllerSettings()
        {
            basePath = Application.dataPath,
            useBackup = false,
        });
        
        private readonly FileControllerSettings _settings;
        private readonly AesCryptoServiceProvider _provider;
         
        public FileController(FileControllerSettings settings)
        {
            _settings = settings;
            _provider = new AesCryptoServiceProvider
            {
                Key = Encoding.ASCII.GetBytes(settings.encryptionKey.PadRight(16, 'X')[..16]),
                IV = Encoding.ASCII.GetBytes(settings.encryptionIv.PadRight(16, 'X')[..16])
            };
        }

        public FileController() : this(new FileControllerSettings()) {}

        public string Filepath(string filepath) =>
            $"{_settings.basePath}/{filepath.TrimStart('/')}";

        public void Erase(string filepath)
        {
            var path = Filepath(filepath);
            
            if (File.Exists(path))
            {
                Log.Debug($"Deleting File - {path}");
                File.Delete(path);
            }

            if (!_settings.useBackup) return;
            var pathBackup = $"{path}.bak";
            if (File.Exists(pathBackup))
            {
                Log.Debug($"Deleting File - {pathBackup}");
                File.Delete(pathBackup);
            }
        }

        public void Erase<T>(T file) where T : IRedOwlFile => 
         Erase(file.Filepath);

        public string Read(string filepath)
        {
            var path = Filepath(filepath);
            var pathBackup = $"{path}.bak";
            if (!File.Exists(path))
            {
                if (!File.Exists(pathBackup) || !_settings.useBackup) return "{}";
                File.Copy(pathBackup, path);
            }
            //Log.Info($"Reading File - {path}");
            Stream stream = new FileStream(path, FileMode.Open);
            if (_settings.useEncryption) stream = new CryptoStream(stream, _provider.CreateDecryptor(), CryptoStreamMode.Read);
            if (_settings.useCompression) stream = new DeflateStream(stream, CompressionMode.Decompress);
            if (_settings.useBuffer) stream = new BufferedStream(stream, _settings.bufferSize);
            var reader = new StreamReader(stream, Encoding.ASCII);
            var data = reader.ReadToEnd();
            reader.Dispose();
            return data;
        }
        public T Read<T>(string filepath) => 
            JsonUtility.FromJson<T>(Read(filepath));

        public void Read<T>(T file) where T : IRedOwlFile => 
            JsonUtility.FromJsonOverwrite(Read(file.Filepath), file);

        public void Write(string filepath, string data)
        {
            var path = Filepath(filepath);
            var pathBackup = $"{path}.bak";
            //Log.Info($"Writing File - {path}");
            Directory.CreateDirectory(Path.GetDirectoryName(path) ?? string.Empty);
            if (File.Exists(path) && _settings.useBackup)
            {
                if (File.Exists(pathBackup)) File.Delete(pathBackup);
                File.Move(path, pathBackup);
            }
            Stream stream = new FileStream(path, FileMode.Create);
            if (_settings.useEncryption) stream = new CryptoStream(stream, _provider.CreateEncryptor(), CryptoStreamMode.Write);
            if (_settings.useCompression) stream = new DeflateStream(stream, CompressionMode.Compress);
            if (_settings.useBuffer) stream = new BufferedStream(stream, _settings.bufferSize);
            var writer = new StreamWriter(stream, Encoding.ASCII);
            try
            {
                writer.Write(data);
                writer.Flush();
            }
            catch (Exception)
            {
                if (File.Exists(path)) File.Delete(path);
                if (File.Exists(pathBackup)) File.Move(pathBackup, path);
            }
            finally
            {
                writer.Dispose();
            }
        }

        public void Write<T>(string filepath, T data) => 
            Write(filepath, JsonUtility.ToJson(data, true));

        public void Write<T>(T file) where T : IRedOwlFile => 
            Write(file.Filepath, file);
    }
}

// using System;
// using System.IO;
// using System.IO.Compression;
// using System.Security.Cryptography;
// using System.Text;
// using Sirenix.OdinInspector;
// using UnityEngine;
//
// namespace RedOwl.Engine
// {
//     public interface IRedOwlFile
//     {
//         string Directory { get; }
//         string Filename { get; }
//         string Extension { get; }
//         int LatestVersion { get; }
//         void BeginSerialize(RedOwlSerializer serializer);
//     }
//     
//     #region Settings
//     
//     [Serializable, InlineProperty, HideLabel]
//     public class FileControllerSettings
//     {
//         [SerializeField]
//         public bool UseBuffer = false;
//         [SerializeField, ShowIf("UseBuffer")] 
//         public int BufferSize = 1024;
//
//         [SerializeField]
//         public RedOwlSerializer.Formats Format = RedOwlSerializer.Formats.Binary;
//         
//         [SerializeField]
//         public bool UseCompression;
//         
//         [SerializeField]
//         public bool UseEncryption;
//
//         [SerializeField, ShowIf("UseEncryption")]
//         public string EncryptionKey;
//
//         [SerializeField, ShowIf("UseEncryption")]
//         public string EncryptionIV;
//     }
//
//     #endregion
//
//     public class FileController
//     {
//         private FileControllerSettings _settings;
//         private AesCryptoServiceProvider _provider;
//         
//         public FileController(FileControllerSettings settings)
//         {
//             _provider = new AesCryptoServiceProvider
//             {
//                 Key = Encoding.ASCII.GetBytes(settings.EncryptionKey.PadRight(16, 'X')[..16]),
//                 IV = Encoding.ASCII.GetBytes(settings.EncryptionIV.PadRight(16, 'X')[..16])
//             };
//         }
//         
//         public string Filepath<T>(T file) where T : IRedOwlFile =>
//             $"{Application.persistentDataPath}/{file.Directory}/{file.Filename}.{file.Extension}";
//
//         
//         #region Read
//
//         public void Read<T>(T file) where T : IRedOwlFile
//         {
//             string filepath = Filepath(file);
//             string filepathBackup = $"{filepath}.bak";
//             if (!File.Exists(filepath))
//             {
//                 if (!File.Exists(filepathBackup)) return;
//                 File.Move(filepathBackup, filepath);
//             }
//             Log.Debug($"Reading File - {filepath}");
//             Stream stream = new FileStream(filepath, FileMode.Open);
//             if (_settings.UseEncryption) stream = new CryptoStream(stream, _provider.CreateDecryptor(), CryptoStreamMode.Read);
//             if (_settings.UseCompression) stream = new DeflateStream(stream, CompressionMode.Decompress);
//             if (_settings.UseBuffer) stream = new BufferedStream(stream, _settings.BufferSize);
//             var serializer = new RedOwlSerializer(stream, file.LatestVersion, _settings.Format);
//             try
//             {
//                 file.BeginSerialize(serializer);
//             }
//             finally
//             {
//                 serializer.Dispose();
//             }
//         }
//
//         #endregion
//         
//         #region Write
//
//         public void Write<T>(T file) where T : IRedOwlFile
//         {
//             string filepath = Filepath(file);
//             string filepathBackup = $"{filepath}.bak";
//             Log.Debug($"Writing File - {filepath}");
//             Directory.CreateDirectory(Path.GetDirectoryName(filepath) ?? string.Empty);
//             if (File.Exists(filepath))
//             {
//                 if (File.Exists(filepathBackup)) File.Delete(filepathBackup);
//                 File.Move(filepath, filepathBackup);
//             }
//             Stream stream = new FileStream(filepath, FileMode.Create);
//             if (_settings.UseEncryption) stream = new CryptoStream(stream, _provider.CreateEncryptor(), CryptoStreamMode.Write);
//             if (_settings.UseCompression) stream = new DeflateStream(stream, CompressionMode.Compress);
//             if (_settings.UseBuffer) stream = new BufferedStream(stream, _settings.BufferSize);
//             var serializer = new RedOwlSerializer(stream, file.LatestVersion, _settings.Format, true);
//             try
//             {
//                 file.BeginSerialize(serializer);
//                 serializer.Dispose();
//             }
//             catch (Exception)
//             {
//                 serializer.Dispose();
//                 if (File.Exists(filepath)) File.Delete(filepath);
//                 if (File.Exists(filepathBackup)) File.Move(filepathBackup, filepath);
//             }
//         }
//
//         #endregion
//
//         public T List<T>(T file) where T : IRedOwlFile
//         {
//             // TODO: not sure how to do this given our customization of Directory/Filename
//             // If we nest save meta files in a directory we need to do a double listing
//             throw new NotImplementedException();
//         }
//
//         public void Erase<T>(T file) where T : IRedOwlFile
//         {
//             var filepath = Filepath(file);
//             var filepathBackup = $"{filepath}.bak";
//             if (File.Exists(filepath))
//             {
//                 Log.Debug($"Deleting File - {filepath}");
//                 File.Delete(filepath);
//             }
//             if (File.Exists(filepathBackup))
//             {
//                 Log.Debug($"Deleting File - {filepathBackup}");
//                 File.Delete(filepathBackup);
//             }
//         }
//     }
// }