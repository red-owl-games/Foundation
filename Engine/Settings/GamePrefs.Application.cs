using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    public partial class GamePrefs
    {
        [FoldoutGroup("Application")]
        public Parameter<int> MaxFPS = new(60,
            () => Application.targetFrameRate,
            (v) => Application.targetFrameRate = v);
    }
}