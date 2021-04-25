namespace RedOwl.Engine
{
    public enum GameTypes
    {
        SinglePlayer,
        Coop,
        OnlineMultiPlayer
    }
    
    public partial class Game
    {
        [ClearOnReload(GameTypes.SinglePlayer)]
        public static GameTypes GameType;

        public static bool IsGameSinglePlayer => GameType == GameTypes.SinglePlayer;
        public static bool IsGameCoop => GameType == GameTypes.Coop;
        public static bool IsGameOnlineMultiPlayer => GameType == GameTypes.OnlineMultiPlayer;
    }
}