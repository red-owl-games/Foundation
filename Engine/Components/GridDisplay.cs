using System;
using Shapes;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

namespace RedOwl.Engine
{
    [Serializable]
    public class GridDrawer
    {
        public int2 x = new(0,10);
        public int2 y = new(0, 10);
        public int2 z = new(0, 10);
        public bool useGlobalGrid = true;
        [HideIf("useGlobalGrid"), HideLabel, InlineProperty]
        public GridSettings settings;
        public Color mainColor = Color.white;
        public Color edgeColor = Color.yellow;

        private GridSettings Grid => Game.IsRunning && useGlobalGrid ? Game.Instance.GridSettings.Value : settings;

        public GridDrawer(GridSettings settings)
        {
            this.settings = settings;
        }
        
        public GridDrawer() : this(new GridSettings()) {}
        
        public void OnValidate()
        {
            if (x.x > x.y) x.y = x.x;
            else if (x.y < x.x) x.x = x.y;
            
            if (y.x > y.y) y.y = y.x;
            else if (y.y < y.x) y.x = y.y;
            
            if (z.x > z.y) z.y = z.x;
            else if (z.y < z.x) z.x = z.y;
        }

        public void Render(Camera cam)
        {
            var width = math.abs(x.x - x.y);
            var height = math.abs(y.x - y.y);
            var depth = math.abs(z.x - z.y);
            var hasWidth = width > 1;
            var hasHeight = height > 1;
            var hasDepth = depth > 1;

            if (hasWidth)
            {
                for (var i = x.x; i <= x.y; i++)
                {
                    if (hasDepth)
                        Draw.Line(Grid.GetWorldPositionFrom(i, 0, z.x),
                            Grid.GetWorldPositionFrom(i, 0, z.y),
                            i == x.x || i == x.y ? edgeColor : mainColor);
                    if (hasHeight)
                        Draw.Line(Grid.GetWorldPositionFrom(i, y.x, z.y),
                            Grid.GetWorldPositionFrom(i, y.y, z.y),
                            i == x.x || i == x.y ? edgeColor : mainColor);
                }
            }

            if (hasHeight)
            {
                for (var i = y.x; i <= y.y; i++)
                {
                    if (hasWidth)
                        Draw.Line(Grid.GetWorldPositionFrom(x.x, i, z.y),
                            Grid.GetWorldPositionFrom(x.y, i, z.y),
                            i == y.x || i == y.y ? edgeColor : mainColor);
                    if (hasDepth)
                        Draw.Line(Grid.GetWorldPositionFrom(x.y, i, z.x),
                            Grid.GetWorldPositionFrom(x.y, i, z.y),
                            i == y.x || i == y.y ? edgeColor : mainColor);
                }
            }

            if (hasDepth)
            {
                for (var i = z.x; i <= z.y; i++)
                {
                    if (hasHeight)
                        Draw.Line(Grid.GetWorldPositionFrom(x.y, y.x, i),
                            Grid.GetWorldPositionFrom(x.y, y.y, i),
                            i == z.x || i == z.y ? edgeColor : mainColor);
                    if (hasWidth)
                        Draw.Line(Grid.GetWorldPositionFrom(x.x, 0, i),
                            Grid.GetWorldPositionFrom(x.y, 0, i),
                            i == z.x || i == z.y ? edgeColor : mainColor);
                }
            }
        }
    }
    
    [ExecuteAlways] 
    public class GridDisplay : ImmediateModeShapeDrawer
    {
        [HideLabel, InlineProperty]
        public GridDrawer drawer;

        private void OnValidate()
        {
            drawer.OnValidate();
        }

        public override void DrawShapes(Camera cam)
        {
            using (Draw.Command(cam))
            {
                drawer.Render(cam);
            }
        }
    }
}