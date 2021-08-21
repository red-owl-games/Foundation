using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

namespace RedOwl.Engine
{
    public interface ISaveFile : IRedOwlFile
    {
        void WithKey(string key);
    }
    
    public static partial class Game
    {
        public static class Data
        {
            [ClearOnReload]
            public static ISaveFile Metafile;
            [ClearOnReload(true)]
            public static List<ISaveFile> Datafiles = new List<ISaveFile>();

            public static void Save(string key)
            {
                if (Metafile != null)
                {
                    Metafile.WithKey(key);
                    FileController.Write(Metafile);
                }
                foreach (var datafile in Datafiles)
                {
                    datafile.WithKey(key);
                    FileController.Write(datafile);
                }
            }

            public static void Load(string key)
            {
                if (Metafile != null)
                {
                    Metafile.WithKey(key);
                    FileController.Read(Metafile);
                }
                foreach (var datafile in Datafiles)
                {
                    datafile.WithKey(key);
                    FileController.Read(datafile);
                }
            }
            
            private static readonly JsonSerializerSettings defaultSettings = new JsonSerializerSettings(){
                Converters = new List<JsonConverter>
                {
                    new ColorConverter(),
                    new Color32Converter(),
                    new Vector2IntConverter(),
                    new Vector3IntConverter(),
                    new Vector2Converter(),
                    new Vector3Converter(),
                    new Vector4Converter(),
                    new QuaternionConverter(),
                    new PlaneConverter(),
                    new BoundsConverter(),
                    new BoundsIntConverter(),
                    new RectConverter(),
                    new RectIntConverter(),
                    new RectOffsetConverter(),
                    new StringEnumConverter(),
                    new VersionConverter(),
                }
            };

            [RuntimeInitializeOnLoadMethod]
            #if UNITY_EDITOR
            [UnityEditor.InitializeOnLoadMethod]
            #endif
            public static void InitializeJsonSettings()
            {
                if(JsonConvert.DefaultSettings == null) JsonConvert.DefaultSettings = () => defaultSettings;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static T FromJson<T>(string json) => JsonConvert.DeserializeObject<T>(json); //JsonUtility.FromJson<T>(json);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static T FromBytes<T>(byte[] bytes) => FromJson<T>(Encoding.ASCII.GetString(bytes));

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static string ToJson<T>(T item) => JsonConvert.SerializeObject(item); //JsonUtility.ToJson(item);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static byte[] ToBytes<T>(T item) => Encoding.ASCII.GetBytes(ToJson(item));
        }
    }
}