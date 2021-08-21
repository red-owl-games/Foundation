
namespace RedOwl.Engine
{
    public static partial class Game
    {
        [Telegraph]
        public static class Events
        {
            [Telegram]
            public static Message StartGame => Telegraph.Get<Message>(nameof(StartGame));
            [Telegram]
            public static Message OnStartGame => Telegraph.Get<Message>(nameof(OnStartGame));
            [Telegram]
            public static Message PauseGame => Telegraph.Get<Message>(nameof(PauseGame));
            [Telegram]
            public static Message OnPauseGame => Telegraph.Get<Message>(nameof(OnPauseGame));
            [Telegram]
            public static Message ResumeGame => Telegraph.Get<Message>(nameof(ResumeGame));
            [Telegram]
            public static Message OnResumeGame => Telegraph.Get<Message>(nameof(OnResumeGame));
            [Telegram]
            public static Message QuitGame => Telegraph.Get<Message>(nameof(QuitGame));
            
            [Telegram]
            public static Message SaveGame => Telegraph.Get<Message>(nameof(SaveGame));
            
            [Telegram]
            public static Message OnBeforeSaveGame => Telegraph.Get<Message>(nameof(OnBeforeSaveGame));
            
            [Telegram]
            public static StringMessage LoadGame => Telegraph.Get<StringMessage>(nameof(LoadGame));
            
            [Telegram]
            public static Message OnAfterSaveGame => Telegraph.Get<Message>(nameof(OnAfterSaveGame));

            // Fade To Black
            [Telegram]
            public static Message FadeOut => Telegraph.Get<Message>(nameof(FadeOut));
            [Telegram]
            public static Message OnFadeOut => Telegraph.Get<Message>(nameof(OnFadeOut));
            
            // Fade Away from Black
            [Telegram]
            public static Message FadeIn => Telegraph.Get<Message>(nameof(FadeIn));
            [Telegram]
            public static Message OnFadeIn => Telegraph.Get<Message>(nameof(OnFadeIn));

            [Telegram]
            public static Message ShowLoadingScreen => Telegraph.Get<Message>(nameof(ShowLoadingScreen));
            [Telegram]
            public static Message OnShowLoadingScreen => Telegraph.Get<Message>(nameof(OnShowLoadingScreen));
            [Telegram]
            public static Message HideLoadingScreen => Telegraph.Get<Message>(nameof(HideLoadingScreen));
            [Telegram]
            public static Message OnHideLoadingScreen => Telegraph.Get<Message>(nameof(OnHideLoadingScreen));
            
        }
    }
}