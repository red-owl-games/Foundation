using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace RedOwl.Engine
{
    [Serializable, InlineProperty, HideLabel]
    public abstract class Settings
    {
        
    }
    
    [Singleton]
    public partial class Game : Asset<Game>
    {
        [FoldoutGroup("Log"), SerializeField]
        private LogSettings logSettings = new LogSettings();
        public static LogSettings LogSettings => Instance.logSettings;

        [FoldoutGroup("Console"), SerializeField]
        private ConsoleSettings consoleSettings = new ConsoleSettings();
        public static ConsoleSettings ConsoleSettings => Instance.consoleSettings;

        [FoldoutGroup("Debug Draw"), SerializeField]
        private DrawSettings drawSettings = new DrawSettings();
        public static DrawSettings DrawSettings => Instance.drawSettings;

        [FoldoutGroup("File Controller"), SerializeField]
        private FileControllerSettings fileControllerSettings = new FileControllerSettings();
        public static FileControllerSettings FileControllerSettings => Instance.fileControllerSettings;

        [FoldoutGroup("Level Manager"), SerializeField]
        private LevelManagerSettings levelManagerSettings = new LevelManagerSettings();
        public static LevelManagerSettings LevelManagerSettings => Instance.levelManagerSettings;

        [FoldoutGroup("Google Sheets"), SerializeField]
        private GoogleSheetsSettings googleSheetsSettings = new GoogleSheetsSettings();
        public static GoogleSheetsSettings GoogleSheetsSettings => Instance.googleSheetsSettings;

        [FoldoutGroup("Avatar System"), SerializeField]
        private AvatarSettings avatarSettings = new AvatarSettings();
        public static AvatarSettings AvatarSettings => Instance.avatarSettings;


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        internal static void Initialize()
        {
            Log.Always("Initialize RedOwl Game!");
            AvatarSettings.Initialize();
        }

        #region Random

        public static Random Random => new Random((uint)Environment.TickCount);

        #endregion
        
        #region Services

        private static ServiceCache _services;
        public static ServiceCache Services => _services ?? (_services = new ServiceCache());

        public static void Bind<T>(T instance) => Services.Bind(instance);
        public static void BindAs<T>(T instance) => Services.BindAs(instance);
        public static T Find<T>() => Services.Find<T>();
        public static void Inject(object obj) => Services.Inject(obj);
        
        #endregion

        
    }
}