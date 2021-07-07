using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    [Serializable]
    public class LocationRef : AssetRef<Location> {}
    
    [CreateAssetMenu(menuName = "Red Owl/Locations/Location", fileName = "Location")]
    public class Location : RedOwlIdentityObject
    {
        public SceneRef mainScene;
        public SceneRef[] otherScenes;

        // TODO: Localization Key
        public string title;
        public string subtitle;

        [HorizontalGroup("loading screen"), ToggleLeft]
        public bool useLoadingScreen = true;
        [HorizontalGroup("loading screen"), EnableIf("useLoadingScreen"), LabelText("Animation Delay"), LabelWidth(110)]
        public float loadingScreenAnimationDelay = 0.75f;
        [HorizontalGroup("fader"), ToggleLeft]
        public bool useFader = false;
        [HorizontalGroup("fader"), EnableIf("useFader"), LabelText("Animation Delay"), LabelWidth(110)]
        public float faderAnimationDelay = 0.75f;

        public float warmupDelay = 0.2f;
        

        [Title("Location State"), EnumToggleButtons, HideLabel]
        public LocationStates state;

        [Title("Music"), HideLabel, InlineProperty]
        public FmodEvent Music;
    }
}