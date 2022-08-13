namespace RedOwl.Engine
{
    public static partial class Game
    {
        [ClearOnReload] private static GamePrefs _prefs;

        public static GamePrefs Prefs
        {
            get
            {
                if (_prefs == null)
                {
                    _prefs = new GamePrefs();
                    _prefs.Load();
                }

                return _prefs;
            }
        }
    }

    public partial class GamePrefs : DatafileInternal
    {
        public override string Filepath => "prefs.dat";
    }
}