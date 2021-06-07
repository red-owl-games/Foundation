using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    [Serializable, InlineProperty]
    public class InputReference
    {
        [HideLabel]
        public int index;

        // TODO: Cache? Inject?
        public IInputState State => Game.InputService[index];
    }
}