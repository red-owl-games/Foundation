using UnityEngine;

namespace RedOwl.Core
{
    public partial class RedOwlSettings
    {
        [SerializeField]
        private StateMachineConfig gameStateMachine;

        public static StateMachineConfig GameStateMachine => Instance.gameStateMachine;
    }
    
    public static class GameManager
    {
        public static ServiceCache Services { get; private set; }
        public static StateMachine StateMachine { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Initialize()
        {
            Log.Always("Initialize RedOwl Game");
            var obj = new GameObject("Game");
            Object.DontDestroyOnLoad(obj);
            
            Services = new ServiceCache();
            StateMachine = new StateMachine(obj, RedOwlSettings.GameStateMachine);

            StateMachine.EnterInitialState();
        }
    }
}