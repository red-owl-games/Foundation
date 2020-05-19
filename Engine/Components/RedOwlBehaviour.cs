using UnityEngine;

namespace RedOwl.Core
{
    public abstract class RedOwlBehaviour : MonoBehaviour
    {
        protected virtual void Awake()
        {
            Game.Inject(this);
        }
    }
}