using UnityEngine;

namespace RedOwl.Core
{
    public abstract class RedOwlBehaviour : MonoBehaviour
    {
        protected virtual void Start()
        {
            Game.Inject(this);
        }
    }
}