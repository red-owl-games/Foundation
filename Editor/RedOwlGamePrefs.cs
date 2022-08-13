using RedOwl.Engine;
using UnityEditor;

namespace RedOwl.Editor
{
    [InitializeOnLoad]
    public class RedOwlGamePrefs
    {
        static RedOwlGamePrefs()
        {
            var prefs = new GamePrefs();
            prefs.Load();
            prefs.Save();
        }
    }
}