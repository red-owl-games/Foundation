namespace RedOwl.Core
{
    public static class SceneController
    {
        public static void Load(int i, bool addative = false)
        {
            Log.Always($"Loading Scene: {i} - {addative}");
        }
        
        public static void Unload(int i)
        {
            Log.Always($"Unloading Scene: {i}");
        }
    }
}