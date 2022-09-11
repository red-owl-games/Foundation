using System;
using System.Dynamic;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Sirenix.OdinInspector;
using UnityEngine;
using YamlDotNet.Serialization;
using DeserializerBuilder = YamlDotNet.Serialization.DeserializerBuilder;
using SerializerBuilder = YamlDotNet.Serialization.SerializerBuilder;

namespace RedOwl.Engine
{
    public abstract class Datafile
    {
        public enum Locations
        {
            Internal,
            Encrypted,
            Normal,
        }

        private string _filepath;
        private Locations _location;

        public Datafile(string filepath, Locations location = Locations.Normal)
        {
            _filepath = filepath;
            _location = location;
        }
        
        public void Save()
        {
            switch (_location)
            {
                case Locations.Internal:
                    FileController.InstanceInternal.Write(_filepath, this);
                    break;
                case Locations.Encrypted:
                    FileController.InstanceEnc.Write(_filepath, this);
                    break;
                case Locations.Normal:
                default:
                    FileController.Instance.Write(_filepath, this);
                    break;
            }
        }

        public void Load()
        {
            switch (_location)
            {
                case Locations.Internal:
                    FileController.InstanceInternal.Read(_filepath, this);
                    break;
                case Locations.Encrypted:
                    FileController.InstanceEnc.Read(_filepath, this);
                    break;
                case Locations.Normal:
                default:
                    FileController.Instance.Read(_filepath, this);
                    break;
            }
            
        }
    }

    [Serializable]
    public class Datafile<T> : Datafile where T : new()
    {
        [SerializeReference]
        public T Data;
        
        public Datafile(string filepath, Locations location = Locations.Normal) : base(filepath, location)
        {
            Data = new T();
            Load();
        }

        public static implicit operator Datafile<T>(string filepath) => new(filepath);
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
    
    /// <summary>
    /// [Serializable]
    /// public class GameSettings
    /// {
    ///     public int Score;
    ///     
    ///     public void Save() => FileController.Instance.Write("settings.dat", this);
    ///     public void Load() => FileController.Instance.Read("settings.dat", this);
    /// }
    /// </summary>
    public class FileController
    {
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

        public string Read(string filepath)
        {
            var path = Filepath(filepath);
            var pathBackup = $"{path}.bak";
            if (!File.Exists(path))
            {
                if (!File.Exists(pathBackup) || !_settings.useBackup) return "{}";
                File.Copy(pathBackup, path);
            }
            Log.Info($"Reading File - {path}");
            var isYaml = Path.GetExtension(path) is ".yaml" or ".yml" or ".YML" or ".YAML";
            Stream stream = new FileStream(path, FileMode.Open);
            if (_settings.useEncryption) stream = new CryptoStream(stream, _provider.CreateDecryptor(), CryptoStreamMode.Read);
            if (_settings.useCompression) stream = new DeflateStream(stream, CompressionMode.Decompress);
            if (_settings.useBuffer) stream = new BufferedStream(stream, _settings.bufferSize);
            var reader = new StreamReader(stream, Encoding.ASCII);
            try
            {
                return isYaml ? Serializer.Serialize(Deserializer.Deserialize(reader) ?? "") : reader.ReadToEnd();
            }
            finally
            {
                reader.Dispose();
            }
        }
        
        public T Read<T>(string filepath) => 
            JsonUtility.FromJson<T>(Read(filepath));
        
        public void Read(string filepath, object data) => 
            JsonUtility.FromJsonOverwrite(Read(filepath), data);
        
        public void Read<T>(string filepath, ref T data) => 
            JsonUtility.FromJsonOverwrite(Read(filepath), data);

        public void Write(string filepath, string data)
        {
            var path = Filepath(filepath);
            var pathBackup = $"{path}.bak";
            Log.Info($"Writing File - {path}");
            Directory.CreateDirectory(Path.GetDirectoryName(path) ?? string.Empty);
            if (File.Exists(path) && _settings.useBackup)
            {
                if (File.Exists(pathBackup)) File.Delete(pathBackup);
                File.Move(path, pathBackup);
            }
            var isYaml = Path.GetExtension(path) is ".yaml" or ".yml" or ".YML" or ".YAML";
            Stream stream = new FileStream(path, FileMode.Create);
            if (_settings.useEncryption) stream = new CryptoStream(stream, _provider.CreateEncryptor(), CryptoStreamMode.Write);
            if (_settings.useCompression) stream = new DeflateStream(stream, CompressionMode.Compress);
            if (_settings.useBuffer) stream = new BufferedStream(stream, _settings.bufferSize);
            var writer = new StreamWriter(stream, Encoding.ASCII);
            try
            {
                writer.Write(isYaml? new SerializerBuilder().Build().Serialize(JsonConvert.DeserializeObject<ExpandoObject>(data, new ExpandoObjectConverter())): data);
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
        
        private static ISerializer _serializer;
        public static ISerializer Serializer => _serializer ??= new SerializerBuilder().JsonCompatible().Build();


        private static IDeserializer _deserializer;
        public static IDeserializer Deserializer => _deserializer ??= new DeserializerBuilder().IgnoreUnmatchedProperties().Build();

        public static FileController Instance { get; } = new();

        public static FileController InstanceEnc { get; } = new(new FileControllerSettings()
        {
            useCompression = true,
            useEncryption = true,
            encryptionKey = SystemInfo.deviceUniqueIdentifier,
            encryptionIv = SystemInfo.deviceUniqueIdentifier,
        });

        internal static FileController InstanceInternal { get; } = new(new FileControllerSettings()
        {
            basePath = Application.dataPath,
            useBackup = false,
        });
    }
}
