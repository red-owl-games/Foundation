using Sirenix.OdinInspector;

namespace RedOwl.Engine
{
    public partial class GamePrefs
    {
        [FoldoutGroup("Red Owl"), BoxGroup("Red Owl/Grid"), HideLabel, InlineProperty]
        public Parameter<GridSettings> GridSettings = new(new GridSettings());
    }
}