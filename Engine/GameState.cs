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

        public static bool IsSinglePlayer => GameType == GameTypes.SinglePlayer;
        public static bool IsCoop => GameType == GameTypes.Coop;
        public static bool IsOnlineMultiPlayer => GameType == GameTypes.OnlineMultiPlayer;
    }
}