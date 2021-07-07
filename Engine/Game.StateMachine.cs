namespace RedOwl.Engine
{
    public static partial class Game
    {
        public static StateMachine StateMachine { get; private set; }

        public static class States
        {
            public static CallbackState Running { get; internal set; }
            public static CallbackState Paused { get; internal set; }
        }

        private static void SetupStateMachines()
        {
            BuildMainStateMachine();
            BuildLoadingScreenStateMachine();
            BuildFaderStateMachine();
        }

        private static void BuildMainStateMachine()
        {
            StateMachine = new StateMachine("Game");

            var initialState = StateMachine.Add(new CallbackState{Name = "Initial", WhenExit = Events.OnStartGame.Raise});
            States.Running = StateMachine.Add(new CallbackState{Name = "Running", WhenEnter = Events.OnResumeGame.Raise});
            States.Paused = StateMachine.Add(new CallbackState{Name = "Pause", WhenEnter = Events.OnPauseGame.Raise});
            
            initialState.Permit(States.Running);
            States.Running.Permit(States.Paused, Events.PauseGame);
            States.Paused.Permit(States.Running, Events.ResumeGame);
            
            StateMachine.SetInitialState(initialState);

            Bind(StateMachine, "GameStateMachine");
        }
        
        private static void BuildLoadingScreenStateMachine()
        {
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
        }

        private static void BuildFaderStateMachine()
        {
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
        }
    }
}