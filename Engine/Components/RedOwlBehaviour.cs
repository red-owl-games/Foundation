using UnityEngine;

namespace RedOwl.Engine
{
    public abstract class RedOwlBehaviour : MonoBehaviour
    {
        protected virtual void Start()
        {
            Game.Inject(this);
        }
    }
}