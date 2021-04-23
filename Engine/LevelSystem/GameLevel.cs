using System.Collections.Generic;

namespace RedOwl.Engine
{
    public enum LevelTypes
    {
        SinglePlayer,
        MultiPlayer
    }

    public class GameLevel
    {
        public LevelTypes type { get; private set; }
        public LevelStates state { get; private set; }

        public string sceneName { get; private set; }

        public bool isMainMenu { get; private set; }
        
        //TODO: Localization Key
        public string title { get; private set; }
        public string subTitle { get; private set; }

        public List<FmodEvent> fmodEvents { get; private set; }

        public bool IsSkippable { get; private set; }

        public GameLevel(string name) : base()
        {
            sceneName = name;
            isMainMenu = false;
            title = "";
            subTitle = "";

            type = LevelTypes.MultiPlayer;
            state = LevelStates.None;

            fmodEvents = new List<FmodEvent>();

            IsSkippable = false;
        }
        
        public GameLevel Suffix(string value)
        {
            sceneName = $"{sceneName}{value}";
            return this;
        }

        public GameLevel Skippable()
        {
            IsSkippable = true;
            return this;
        }

        public GameLevel MainMenu()
        {
            isMainMenu = true;
            return this;
        }

        public GameLevel Title(string _title = "", string _subTitle = "")
        {
            title = _title;
            subTitle = _subTitle;
            return this;
        }

        public GameLevel State(LevelStates value)
        {
            state = value;
            return this;
        }

        public GameLevel FMOD(string path, params FmodParam[] parameters)
        {
            fmodEvents.Add(new FmodEvent(path, parameters: parameters));
            return this;
        }
    }
}