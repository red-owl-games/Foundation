namespace RedOwl.Engine
{
    public static partial class Game
    {
        public static StateMachine StateMachine { get; private set; }

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
            var runningState = StateMachine.Add(new CallbackState{Name = "Running", WhenEnter = Events.OnResumeGame.Raise});
            var pauseState = StateMachine.Add(new CallbackState{Name = "Pause", WhenEnter = Events.OnPauseGame.Raise});
            
            initialState.Permit(runningState, Events.StartGame);
            runningState.Permit(pauseState, Events.PauseGame);
            pauseState.Permit(runningState, Events.ResumeGame);
            
            StateMachine.SetInitialState(initialState);

            Services.Add(StateMachine, "GameStateMachine");
        }
        
        private static void BuildLoadingScreenStateMachine()
        {
            var loading = new StateMachine("Loading Screen");
            var show = loading.Add(new CallbackState {Name = "Show", WhenEnter = Events.OnShowLoadingScreen.Raise});
            var hide = loading.Add(new CallbackState {Name = "Hide", WhenEnter = Events.OnHideLoadingScreen.Raise});
            
            show.Permit(hide, Events.HideLoadingScreen);
            hide.Permit(show, Events.ShowLoadingScreen);
            
            loading.SetInitialState(hide);

            Services.Add(loading, "LoadingScreenStateMachine");
        }

        private static void BuildFaderStateMachine()
        {
            var fader = new StateMachine("Fader");
            var show = fader.Add(new CallbackState {Name = "Fade Out", WhenEnter = Events.OnFadeOut.Raise});
            var hide = fader.Add(new CallbackState {Name = "Fade In", WhenEnter = Events.OnFadeIn.Raise});
            
            show.Permit(hide, Events.FadeIn);
            hide.Permit(show, Events.FadeOut);
            
            fader.SetInitialState(hide);

            Services.Add(fader, "FaderStateMachine");
        }
    }
}