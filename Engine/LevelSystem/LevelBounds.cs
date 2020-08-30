using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Core
{
    public interface ILevelBounds
    {
        Bounds Bounds { get;  }
    }
    
    [AddComponentMenu("Project/Level Bounds")] 
    public class LevelBounds : MonoBehaviour, ILevelBounds
    {
        [OnValueChanged("CalculateBounds")]
        public Vector3 extents;

        public Bounds Bounds { get; private set; }

        private void OnValidate()
        {
            CalculateBounds();
        }

        private void Awake()
        {
            CalculateBounds();
            Game.BindAs<ILevelBounds>(this);
        }

        private void OnDrawGizmos()
        {
            CalculateBounds();
            var v3FrontTopLeft = new Vector3(Bounds.center.x - Bounds.extents.x, Bounds.center.y + Bounds.extents.y + 1, Bounds.center.z - Bounds.extents.z);
            Draw.Label(v3FrontTopLeft, "Level Bounds", Draw.Colors.Yellow);
            Draw.Bounds(Bounds, Draw.Colors.Yellow);
        }

        protected virtual void CalculateBounds()
        {
            Bounds = new Bounds(transform.position, extents);
        }
    }
}