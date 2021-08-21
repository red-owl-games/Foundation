namespace RedOwl.Engine
{
    public static partial class Game
    {
        public static StateMachine StateMachine;
        internal static StateMachine LoadingScreenStateMachine;
        internal static StateMachine FaderStateMachine;

        public static class States
        {
            public static void Initialize()
            {
                Running = new CallbackState {Name = "Running", WhenEnter = Events.OnResumeGame.Raise};
                Paused = new CallbackState {Name = "Pause", WhenEnter = Events.OnPauseGame.Raise};
                
                StateMachine = BuildMainStateMachine();
                LoadingScreenStateMachine = BuildLoadingScreenStateMachine();
                FaderStateMachine = BuildFaderStateMachine();
            }

            public static CallbackState Running;
            public static CallbackState Paused;
        }

        private static StateMachine BuildMainStateMachine()
        {
            Log.Always("Building Game StateMachine");
            var machine = new StateMachine("Game");

            var initialState = machine.Add(new CallbackState{Name = "Initial", WhenExit = Events.OnStartGame.Raise});
            machine.Add(States.Running);
            machine.Add(States.Paused);
            
            initialState.Permit(States.Running);
            States.Running.Permit(States.Paused, Events.PauseGame);
            States.Paused.Permit(States.Running, Events.ResumeGame);
            
            machine.SetInitialState(initialState);

            Bind(machine, "GameStateMachine");

            return machine;
        }
        
        private static StateMachine BuildLoadingScreenStateMachine()
        {
            Log.Always("Building LoadingScreen StateMachine");
            var loading = new StateMachine("Loading Screen");
            var initialState = loading.Add(new CallbackState{Name = "Initial"});
            var show = loading.Add(new CallbackState {Name = "Show Loading Screen", WhenEnter = Events.OnShowLoadingScreen.Raise});
            var hide = loading.Add(new CallbackState {Name = "Hide Loading Screen", WhenEnter = Events.OnHideLoadingScreen.Raise});
            
            initialState.Permit(hide, Events.HideLoadingScreen);
            initialState.Permit(show, Events.ShowLoadingScreen);
            show.Permit(hide, Events.HideLoadingScreen);
            hide.Permit(show, Events.ShowLoadingScreen);
            
            loading.SetInitialState(initialState);

            Bind(loading, "LoadingScreenStateMachine");
            return loading;
        }

        private static StateMachine BuildFaderStateMachine()
        {
            Log.Always("Building Fader StateMachine");
            var fader = new StateMachine("Fader");
            var initialState = fader.Add(new CallbackState{Name = "Initial"});
            var show = fader.Add(new CallbackState {Name = "Fade Out", WhenEnter = Events.OnFadeOut.Raise});
            var hide = fader.Add(new CallbackState {Name = "Fade In", WhenEnter = Events.OnFadeIn.Raise});
            
            initialState.Permit(hide, Events.FadeIn);
            initialState.Permit(show, Events.FadeOut);
            show.Permit(hide, Events.FadeIn);
            hide.Permit(show, Events.FadeOut);
            
            fader.SetInitialState(initialState);

            Bind(fader, "FaderStateMachine");
            return fader;
        }
    }
}