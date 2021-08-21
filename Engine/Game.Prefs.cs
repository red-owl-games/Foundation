using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace RedOwl.Engine
{
    public partial class GameSettings
    {
        [Button]
        public static void ClearPrefs() => FileController.Erase(new GamePrefs());
    }
    
    public class GamePrefException : Exception
    {
        public GamePrefException(string msg) : base(msg) {}
    }

    public class GamePref<T>
    {
        private readonly string _key;

        public T Value
        {
            get => Game.Prefs.Get<T>(_key);
            set => Game.Prefs.Set(_key, value);
        }

        public event Action<T> On;
        
        public GamePref(string key)
        {
            Log.Always($"Register Pref '{key}'");
            _key = key;
            Game.Prefs.Register(key, OnChanged);
        }
        
        public GamePref(string key, Action<T> callback) : this(key)
        {
            On += callback;
        }

        private void OnChanged() => On?.Invoke(Value);

        public override string ToString() => $"{Value}";

        public static implicit operator T(GamePref<T> self) => self.Value;
        
        public static implicit operator GamePref<T>(string key) => new GamePref<T>(key);
    }

    public class GamePrefs : IRedOwlFile
    {
        private Dictionary<string, byte[]> _data;
        public string Directory => ""; 
        public string Filename => "preferences";
        public string Extension => ".dat";
        public int LatestVersion => 0;
        public IEnumerable<string> Keys => _data.Keys;

        public GamePrefs()
        {
            _data = new Dictionary<string, byte[]>();
        }

        public void BeginSerialize(RedOwlSerializer s)
        {
            if (s.IsWriting)
            {
                SerializationUtility.SerializeValue(_data, s.Writer);
            }
            else
            {
                _data = SerializationUtility.DeserializeValue<Dictionary<string, byte[]>>(s.Reader);
            }
        }

        public T Get<T>(string key) => 
            !_data.TryGetValue(key, out byte[] data) ? throw new GamePrefException($"Unable to find Key '{key}'") : SerializationUtility.DeserializeValue<T>(data, DataFormat.Binary);

        public void Set<T>(string key, T value) => 
            _data[key] = SerializationUtility.SerializeValue(value, DataFormat.Binary);
    }
    
    public static partial class Game
    {
        public static class Prefs
        {
            #region Storage
            
            public static IEnumerable<string> All => GamePrefs.Keys;

            private static Dictionary<string, List<Action>> _events;

            private static GamePrefs GamePrefs;
            
            public static void Register(string key, Action callback)
            {
                if (_events == null) _events = new Dictionary<string, List<Action>>();
                if (!_events.TryGetValue(key, out var handlers))
                {
                    handlers = new List<Action>();
                    _events[key] = handlers;
                }
                handlers.Add(callback);
            }
            
            public static T Get<T>(string key, bool useDefault = true)
            {
                try
                {
                    return GamePrefs.Get<T>(key);
                }
                catch (GamePrefException)
                {
                    if (useDefault) return default;
                    throw;
                }
            }

            public static void Set<T>(string key, T value, bool save = true)
            {
                Log.Debug($"Set GamePref<{typeof(T)}>('{key}') = '{value}'");
                GamePrefs.Set(key, value);
                if (_events.TryGetValue(key, out var handlers))
                {
                    foreach (var handler in handlers)
                    {
                        handler();
                    }
                }
                if (save) FileController.Write(GamePrefs);
            }
            
            #endregion

            #region Prefs
            
            public enum AntiAliasingOptions
            {
                Disabled = 0,
                _2xMultiSampling = 2,
                _4xMultiSampling = 4,
                _8xMultiSampling = 8,
            }
            
            public enum VSyncOptions
            {
                Off = 0,
                On = 1,
            }

            // TODO: Localization
            public static string GetNiceName(Resolution opt) => $"{opt.width} x {opt.height}";
            public static string GetNiceName(AnisotropicFiltering opt) =>
                opt switch
                {
                    UnityEngine.AnisotropicFiltering.Disable => "Disable",
                    UnityEngine.AnisotropicFiltering.Enable => "Enable",
                    UnityEngine.AnisotropicFiltering.ForceEnable => "Force",
                    _ => throw new ArgumentOutOfRangeException(nameof(opt), opt, null)
                };
            
            public static string GetNiceName(AntiAliasingOptions opt) =>
                opt switch
                {
                    AntiAliasingOptions.Disabled => "Disabled",
                    AntiAliasingOptions._2xMultiSampling => "2x Multi",
                    AntiAliasingOptions._4xMultiSampling => "4x Multi",
                    AntiAliasingOptions._8xMultiSampling => "8x Multi",
                    _ => throw new ArgumentOutOfRangeException(nameof(opt), opt, null)
                };

            public static string GetNiceName(ShadowQuality opt) =>
                opt switch
                {
                    UnityEngine.ShadowQuality.Disable => "Disabled",
                    UnityEngine.ShadowQuality.HardOnly => "Hard",
                    UnityEngine.ShadowQuality.All => "Hard & Soft",
                    _ => throw new ArgumentOutOfRangeException(nameof(opt), opt, null)
                };

            public static string GetNiceName(ShadowResolution opt) =>
                opt switch
                {
                    UnityEngine.ShadowResolution.Low => "Low",
                    UnityEngine.ShadowResolution.Medium => "Medium",
                    UnityEngine.ShadowResolution.High => "High",
                    UnityEngine.ShadowResolution.VeryHigh => "Very High",
                    _ => throw new ArgumentOutOfRangeException(nameof(opt), opt, null)
                };

            public static string GetNiceName(VSyncOptions opt) =>
                opt switch
                {
                    VSyncOptions.Off => "Off",
                    VSyncOptions.On => "On",
                    _ => throw new ArgumentOutOfRangeException(nameof(opt), opt, null)
                };


            public static void Initialize()
            {
                GamePrefs = new GamePrefs();
                FileController.Read(GamePrefs);
                
                FullScreen = new GamePref<bool>(nameof(FullScreen), (v) => Screen.fullScreen = v);
                Resolution = new GamePref<Resolution>(nameof(Resolution), (v) => Screen.SetResolution(v.width, v.height, Screen.fullScreenMode, v.refreshRate));
                AnisotropicFiltering = new GamePref<AnisotropicFiltering>(nameof(AnisotropicFiltering), (v) => QualitySettings.anisotropicFiltering = v);
                AntiAliasing = new GamePref<AntiAliasingOptions>(nameof(AntiAliasing), (v) => QualitySettings.antiAliasing = (int)v);
                ShadowDistance = new GamePref<float>(nameof(ShadowDistance), (v) => QualitySettings.shadowDistance = v);
                ShadowQuality = new GamePref<ShadowQuality>(nameof(ShadowQuality), (v) => QualitySettings.shadows = v);
                ShadowResolution = new GamePref<ShadowResolution>(nameof(ShadowResolution), (v) => QualitySettings.shadowResolution = v);
                SoftParticles = new GamePref<bool>(nameof(SoftParticles), (v) => QualitySettings.softParticles = v);
                SoftVegetation = new GamePref<bool>(nameof(SoftVegetation), (v) => QualitySettings.softVegetation = v);
                VSync = new GamePref<VSyncOptions>(nameof(VSync), (v) => QualitySettings.vSyncCount = (int)v);
            }

            public static GamePref<bool> FullScreen;
            public static GamePref<Resolution> Resolution;
            public static GamePref<AntiAliasingOptions> AntiAliasing;
            public static GamePref<AnisotropicFiltering> AnisotropicFiltering;
            public static GamePref<float> ShadowDistance;
            public static GamePref<ShadowQuality> ShadowQuality;
            public static GamePref<ShadowResolution> ShadowResolution;
            public static GamePref<bool> SoftParticles;
            public static GamePref<bool> SoftVegetation;
            public static GamePref<VSyncOptions> VSync;

            #endregion
        }
    }
}