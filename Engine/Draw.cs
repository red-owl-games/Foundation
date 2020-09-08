using System;
using System.Diagnostics;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Core
{
    [Serializable]
    public class DrawSettings : Settings<DrawSettings>
    {
        [SerializeField]
        [ToggleLeft, HorizontalGroup("Draw", 0.3f), LabelWidth(120)]
        private bool showDebugDraw = true;
        public static bool ShowDebugDraw => Instance.showDebugDraw;
        
        [SerializeField]
        [HorizontalGroup("Draw"), ShowIf("showDebugDraw"), HideLabel]
        private Color drawColor = Color.magenta;
        public static Color DrawColor => Instance.drawColor;
    }
    
    public static class Draw
    {
        public static class Colors
        {
            // via https://gist.github.com/LotteMakesStuff/f7ce43f11e545a151b95b5e87f76304c
            // NOTE: The following color names come from the CSS3 specification, Section 4.3 Extended Color Keywords
            // http://www.w3.org/TR/css3-color/#svg-color

            public static readonly Color ReunoYellow = new Color32(255, 196, 0, 255);
            public static readonly Color BestRed = new Color(255, 24, 0, 255);
            public static readonly Color AliceBlue = new Color32(240, 248, 255, 255);
            public static readonly Color AntiqueWhite = new Color32(250, 235, 215, 255);
            public static readonly Color Aqua = new Color32(0, 255, 255, 255);
            public static readonly Color Aquamarine = new Color32(127, 255, 212, 255);
            public static readonly Color Azure = new Color32(240, 255, 255, 255);
            public static readonly Color Beige = new Color32(245, 245, 220, 255);
            public static readonly Color Bisque = new Color32(255, 228, 196, 255);
            public static readonly Color Black = new Color32(0, 0, 0, 255);
            public static readonly Color BlanchedAlmond = new Color32(255, 235, 205, 255);
            public static readonly Color Blue = new Color32(0, 0, 255, 255);
            public static readonly Color BlueViolet = new Color32(138, 43, 226, 255);
            public static readonly Color Brown = new Color32(165, 42, 42, 255);
            public static readonly Color Burlywood = new Color32(222, 184, 135, 255);
            public static readonly Color CadetBlue = new Color32(95, 158, 160, 255);
            public static readonly Color Chartreuse = new Color32(127, 255, 0, 255);
            public static readonly Color Chocolate = new Color32(210, 105, 30, 255);
            public static readonly Color Coral = new Color32(255, 127, 80, 255);
            public static readonly Color CornflowerBlue = new Color32(100, 149, 237, 255);
            public static readonly Color Cornsilk = new Color32(255, 248, 220, 255);
            public static readonly Color Crimson = new Color32(220, 20, 60, 255);
            public static readonly Color Cyan = new Color32(0, 255, 255, 255);
            public static readonly Color DarkBlue = new Color32(0, 0, 139, 255);
            public static readonly Color DarkCyan = new Color32(0, 139, 139, 255);
            public static readonly Color DarkGoldenrod = new Color32(184, 134, 11, 255);
            public static readonly Color DarkGray = new Color32(169, 169, 169, 255);
            public static readonly Color DarkGreen = new Color32(0, 100, 0, 255);
            public static readonly Color DarkKhaki = new Color32(189, 183, 107, 255);
            public static readonly Color DarkMagenta = new Color32(139, 0, 139, 255);
            public static readonly Color DarkOliveGreen = new Color32(85, 107, 47, 255);
            public static readonly Color DarkOrange = new Color32(255, 140, 0, 255);
            public static readonly Color DarkOrchid = new Color32(153, 50, 204, 255);
            public static readonly Color DarkRed = new Color32(139, 0, 0, 255);
            public static readonly Color DarkSalmon = new Color32(233, 150, 122, 255);
            public static readonly Color DarkSeaGreen = new Color32(143, 188, 143, 255);
            public static readonly Color DarkSlateBlue = new Color32(72, 61, 139, 255);
            public static readonly Color DarkSlateGray = new Color32(47, 79, 79, 255);
            public static readonly Color DarkTurquoise = new Color32(0, 206, 209, 255);
            public static readonly Color DarkViolet = new Color32(148, 0, 211, 255);
            public static readonly Color DeepPink = new Color32(255, 20, 147, 255);
            public static readonly Color DeepSkyBlue = new Color32(0, 191, 255, 255);
            public static readonly Color DimGray = new Color32(105, 105, 105, 255);
            public static readonly Color DodgerBlue = new Color32(30, 144, 255, 255);
            public static readonly Color FireBrick = new Color32(178, 34, 34, 255);
            public static readonly Color FloralWhite = new Color32(255, 250, 240, 255);
            public static readonly Color ForestGreen = new Color32(34, 139, 34, 255);
            public static readonly Color Fuchsia = new Color32(255, 0, 255, 255);
            public static readonly Color Gainsboro = new Color32(220, 220, 220, 255);
            public static readonly Color GhostWhite = new Color32(248, 248, 255, 255);
            public static readonly Color Gold = new Color32(255, 215, 0, 255);
            public static readonly Color Goldenrod = new Color32(218, 165, 32, 255);
            public static readonly Color Gray = new Color32(128, 128, 128, 255);
            public static readonly Color Green = new Color32(0, 128, 0, 255);
            public static readonly Color GreenYellow = new Color32(173, 255, 47, 255);
            public static readonly Color Honeydew = new Color32(240, 255, 240, 255);
            public static readonly Color HotPink = new Color32(255, 105, 180, 255);
            public static readonly Color IndianRed = new Color32(205, 92, 92, 255);
            public static readonly Color Indigo = new Color32(75, 0, 130, 255);
            public static readonly Color Ivory = new Color32(255, 255, 240, 255);
            public static readonly Color Khaki = new Color32(240, 230, 140, 255);
            public static readonly Color Lavender = new Color32(230, 230, 250, 255);
            public static readonly Color Lavenderblush = new Color32(255, 240, 245, 255);
            public static readonly Color LawnGreen = new Color32(124, 252, 0, 255);
            public static readonly Color LemonChiffon = new Color32(255, 250, 205, 255);
            public static readonly Color LightBlue = new Color32(173, 216, 230, 255);
            public static readonly Color LightCoral = new Color32(240, 128, 128, 255);
            public static readonly Color LightCyan = new Color32(224, 255, 255, 255);
            public static readonly Color LightGoldenodYellow = new Color32(250, 250, 210, 255);
            public static readonly Color LightGray = new Color32(211, 211, 211, 255);
            public static readonly Color LightGreen = new Color32(144, 238, 144, 255);
            public static readonly Color LightPink = new Color32(255, 182, 193, 255);
            public static readonly Color LightSalmon = new Color32(255, 160, 122, 255);
            public static readonly Color LightSeaGreen = new Color32(32, 178, 170, 255);
            public static readonly Color LightSkyBlue = new Color32(135, 206, 250, 255);
            public static readonly Color LightSlateGray = new Color32(119, 136, 153, 255);
            public static readonly Color LightSteelBlue = new Color32(176, 196, 222, 255);
            public static readonly Color LightYellow = new Color32(255, 255, 224, 255);
            public static readonly Color Lime = new Color32(0, 255, 0, 255);
            public static readonly Color LimeGreen = new Color32(50, 205, 50, 255);
            public static readonly Color Linen = new Color32(250, 240, 230, 255);
            public static readonly Color Magenta = new Color32(255, 0, 255, 255);
            public static readonly Color Maroon = new Color32(128, 0, 0, 255);
            public static readonly Color MediumAquamarine = new Color32(102, 205, 170, 255);
            public static readonly Color MediumBlue = new Color32(0, 0, 205, 255);
            public static readonly Color MediumOrchid = new Color32(186, 85, 211, 255);
            public static readonly Color MediumPurple = new Color32(147, 112, 219, 255);
            public static readonly Color MediumSeaGreen = new Color32(60, 179, 113, 255);
            public static readonly Color MediumSlateBlue = new Color32(123, 104, 238, 255);
            public static readonly Color MediumSpringGreen = new Color32(0, 250, 154, 255);
            public static readonly Color MediumTurquoise = new Color32(72, 209, 204, 255);
            public static readonly Color MediumVioletRed = new Color32(199, 21, 133, 255);
            public static readonly Color MidnightBlue = new Color32(25, 25, 112, 255);
            public static readonly Color Mintcream = new Color32(245, 255, 250, 255);
            public static readonly Color MistyRose = new Color32(255, 228, 225, 255);
            public static readonly Color Moccasin = new Color32(255, 228, 181, 255);
            public static readonly Color NavajoWhite = new Color32(255, 222, 173, 255);
            public static readonly Color Navy = new Color32(0, 0, 128, 255);
            public static readonly Color OldLace = new Color32(253, 245, 230, 255);
            public static readonly Color Olive = new Color32(128, 128, 0, 255);
            public static readonly Color Olivedrab = new Color32(107, 142, 35, 255);
            public static readonly Color Orange = new Color32(255, 165, 0, 255);
            public static readonly Color Orangered = new Color32(255, 69, 0, 255);
            public static readonly Color Orchid = new Color32(218, 112, 214, 255);
            public static readonly Color PaleGoldenrod = new Color32(238, 232, 170, 255);
            public static readonly Color PaleGreen = new Color32(152, 251, 152, 255);
            public static readonly Color PaleTurquoise = new Color32(175, 238, 238, 255);
            public static readonly Color PaleVioletred = new Color32(219, 112, 147, 255);
            public static readonly Color PapayaWhip = new Color32(255, 239, 213, 255);
            public static readonly Color PeachPuff = new Color32(255, 218, 185, 255);
            public static readonly Color Peru = new Color32(205, 133, 63, 255);
            public static readonly Color Pink = new Color32(255, 192, 203, 255);
            public static readonly Color Plum = new Color32(221, 160, 221, 255);
            public static readonly Color PowderBlue = new Color32(176, 224, 230, 255);
            public static readonly Color Purple = new Color32(128, 0, 128, 255);
            public static readonly Color Red = new Color32(255, 0, 0, 255);
            public static readonly Color RosyBrown = new Color32(188, 143, 143, 255);
            public static readonly Color RoyalBlue = new Color32(65, 105, 225, 255);
            public static readonly Color SaddleBrown = new Color32(139, 69, 19, 255);
            public static readonly Color Salmon = new Color32(250, 128, 114, 255);
            public static readonly Color SandyBrown = new Color32(244, 164, 96, 255);
            public static readonly Color SeaGreen = new Color32(46, 139, 87, 255);
            public static readonly Color Seashell = new Color32(255, 245, 238, 255);
            public static readonly Color Sienna = new Color32(160, 82, 45, 255);
            public static readonly Color Silver = new Color32(192, 192, 192, 255);
            public static readonly Color SkyBlue = new Color32(135, 206, 235, 255);
            public static readonly Color SlateBlue = new Color32(106, 90, 205, 255);
            public static readonly Color SlateGray = new Color32(112, 128, 144, 255);
            public static readonly Color Snow = new Color32(255, 250, 250, 255);
            public static readonly Color SpringGreen = new Color32(0, 255, 127, 255);
            public static readonly Color SteelBlue = new Color32(70, 130, 180, 255);
            public static readonly Color Tan = new Color32(210, 180, 140, 255);
            public static readonly Color Teal = new Color32(0, 128, 128, 255);
            public static readonly Color Thistle = new Color32(216, 191, 216, 255);
            public static readonly Color Tomato = new Color32(255, 99, 71, 255);
            public static readonly Color Turquoise = new Color32(64, 224, 208, 255);
            public static readonly Color Violet = new Color32(238, 130, 238, 255);
            public static readonly Color Wheat = new Color32(245, 222, 179, 255);
            public static readonly Color White = new Color32(255, 255, 255, 255);
            public static readonly Color WhiteSmoke = new Color32(245, 245, 245, 255);
            public static readonly Color Yellow = new Color32(255, 255, 0, 255);
            public static readonly Color YellowGreen = new Color32(154, 205, 50, 255);
        }

        [Conditional("UNITY_EDITOR")]
        public static void Label(Vector3 point, string text, Color? color = null)
        {
            if (!DrawSettings.ShowDebugDraw) return;
            var style = new GUIStyle {normal = {textColor = color ?? DrawSettings.DrawColor}};
            #if UNITY_EDITOR
            UnityEditor.Handles.Label(point, text, style);
            #endif
        }

        [Conditional("UNITY_EDITOR")]
        public static void Line(Vector3 start, Vector3 end, Color? color = null)
        {
            if (!DrawSettings.ShowDebugDraw) return;
            UnityEngine.Debug.DrawLine(start, end, color ?? DrawSettings.DrawColor, 0f, true);
        }
        
        [Conditional("UNITY_EDITOR")]
        public static void Ray(Vector3 start, Vector3 direction, Color? color = null)
        {
            Line(start, start + direction, color);
        }
        
        [Conditional("UNITY_EDITOR")]
        public static void Point(Vector3 position, Color? color = null, float scale = 1f)
        {
            float halfScale = scale * 0.5f;

            Ray(position + Vector3.up * halfScale, -Vector3.up * scale, color);
            Ray(position + Vector3.right * halfScale, -Vector3.right * scale, color);
            Ray(position + Vector3.forward * halfScale, -Vector3.forward * scale, color);
        }
        
        [Conditional("UNITY_EDITOR")]
        public static void Circle(Vector3 center, Vector3 normal, Color? color = null, float radius = 1.0f, int sides = 10)
        {
            float step = 2 * Mathf.PI / sides;
            Quaternion rot = Quaternion.FromToRotation(Vector3.forward, normal);

            for(int i = 0; i < sides; ++i)
            {
                int startIdx = i;
                int endIdx = (i+1) % sides;

                Vector3 legStart = rot * new Vector3(radius * Mathf.Cos(step * startIdx), radius * Mathf.Sin(step * startIdx), 0.0f);
                Vector3 legEnd = rot * new Vector3(radius * Mathf.Cos(step * endIdx),   radius * Mathf.Sin(step * endIdx), 0.0f);

                Line(center + legStart, center + legEnd, color);
            }
        }

        [Conditional("UNITY_EDITOR")]
        public static void Square(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, Color? color = null)
        {
            Line(p0, p1, color);
            Line(p1, p2, color);
            Line(p2, p3, color);
            Line(p3, p0, color);
        }

        [Conditional("UNITY_EDITOR")]
        public static void Square(Rect rect, Color? color = null)
        {
            Vector2 p0 = new Vector2( rect.xMin, rect.yMin );
            Vector2 p1 = new Vector2( rect.xMax, rect.yMin );
            Vector2 p2 = new Vector2( rect.xMax, rect.yMax );
            Vector2 p3 = new Vector2( rect.xMin, rect.yMax );
            Square(p0, p1, p2, p3, color);
        }

        [Conditional("UNITY_EDITOR")]
        public static void Square(Vector3 center, Vector3 normal, Vector2 size, Color? color = null, float degrees = 0f)
        {
            Quaternion rot = Quaternion.FromToRotation(Vector3.forward, normal);

            Vector2 halfExt = size / 2;

            Quaternion localRot = Quaternion.AngleAxis(degrees, Vector3.forward);

            Vector3[] vertices =
            {
                localRot * new Vector3(-halfExt.x,  halfExt.y, 0), // top-left
                localRot * new Vector3( halfExt.x,  halfExt.y, 0), // top-right
                localRot * new Vector3( halfExt.x, -halfExt.y, 0), // bottom-right
                localRot * new Vector3(-halfExt.x, -halfExt.y, 0), // bottom-left
            };

            for(int i = 0; i < vertices.Length; ++i)
            {
                int endIdx = (i+1) % vertices.Length;

                Vector3 legStart = center + rot * vertices[i];
                Vector3 legEnd = center + rot * vertices[endIdx];

                Line(legStart, legEnd, color);
            }
        }

        [Conditional("UNITY_EDITOR")]
        public static void Sphere(Vector3 center, Color? color = null, float radius = 1f)
        {
            Circle(center, Vector3.up, color, radius);
            Circle(center, Vector3.right, color, radius);
            Circle(center, Vector3.forward, color, radius);
        }
        
        [Conditional("UNITY_EDITOR")]
        public static void Cube(Vector3 center, Vector3 size, Color? color = null)
        {
            Square(center + new Vector3(0, 0, size.z * 0.5f), Vector3.forward, new Vector2(size.x, size.y), color);
            Square(center + new Vector3(0, 0, -size.z * 0.5f),Vector3.forward, new Vector2(size.x, size.y), color, 180f);
            Square(center + new Vector3(size.x * 0.5f, 0, 0), Vector3.right, new Vector2(size.z, size.y), color);
            Square(center + new Vector3(-size.x * 0.5f, 0, 0), Vector3.right, new Vector2(size.z, size.y), color, 180f);
            // Might beable to do without 2 side?
            //Square(center + new Vector3(0, size.y, 0), Vector3.up, new Vector2(size.x, size.z), 0f, color);
            //Square(center + new Vector3(0, -size.y, 0), -Vector3.up, new Vector2(size.x, size.z), 0f, color);
        }

        [Conditional("UNITY_EDITOR")]
        public static void Bounds(Bounds bounds, Color? color = null)
        {
            Cube(bounds.center, bounds.size, color);
        }

        [Conditional("UNITY_EDITOR")]
        public static void Cylinder(Vector3 start, Vector3 end, Color? color = null, float radius = 1.0f)
        {
            Vector3 up = (end - start).normalized * radius;
            Vector3 forward = Vector3.Slerp(up, -up, 0.5f);
            Vector3 right = Vector3.Cross(up, forward).normalized * radius;
        
            //Radial circles
            Circle(start, up, color, radius);    
            Circle(end, -up, color, radius);
            Circle((start + end) * 0.5f, up, color, radius);

            //Side lines
            Line(start + right, end + right, color);
            Line(start - right, end - right, color);
        
            Line(start + forward, end + forward, color);
            Line(start - forward, end - forward, color);
        
            //Start endcap
            Line(start - right, start + right, color);
            Line(start - forward, start + forward, color);
        
            //End endcap
            Line(end - right, end + right, color);
            Line(end - forward, end + forward, color);
        }

        [Conditional("UNITY_EDITOR")]
        public static void Cone(Vector3 position, Vector3 direction, Color? color = null, float angle = 45)
        {
            float length = direction.magnitude;
        
            Vector3 forward = direction;
            Vector3 up = Vector3.Slerp(forward, -forward, 0.5f);
            Vector3 right = Vector3.Cross(forward, up).normalized * length;
        
            direction = direction.normalized;
        
            Vector3 slerpedVector = Vector3.Slerp(forward, up, angle / 90.0f);

            Plane farPlane = new Plane(-direction, position + forward);
            Ray distRay = new Ray(position, slerpedVector);
    
            farPlane.Raycast(distRay, out float dist);

            Ray(position, slerpedVector.normalized*dist, color);
            Ray(position, Vector3.Slerp(forward, -up, angle / 90.0f).normalized * dist, color);
            Ray(position, Vector3.Slerp(forward, right, angle / 90.0f).normalized * dist, color);
            Ray(position, Vector3.Slerp(forward, -right, angle / 90.0f).normalized * dist, color);
        
            Circle(position + forward, direction, color, (forward - slerpedVector.normalized * dist).magnitude);
            Circle(position + forward * 0.5f, direction, color, (forward * 0.5f - slerpedVector.normalized * (dist * 0.5f)).magnitude);
        }

        [Conditional("UNITY_EDITOR")]
        public static void Arrow(Vector3 position, Vector3 direction, Color? color = null)
        {
            Ray(position, direction, color);
            Cone(position + direction, -direction * 0.333f, color, 15);
        }

        [Conditional("UNITY_EDITOR")]
        public static void Capsule(Vector3 start, Vector3 end, Color? color = null, float radius = 1)
        {
            Vector3 up = (end - start).normalized * radius;
            Vector3 forward = Vector3.Slerp(up, -up, 0.5f);
            Vector3 right = Vector3.Cross(up, forward).normalized * radius;

            float height = (start - end).magnitude;
            float sideLength = Mathf.Max(0, (height * 0.5f) - radius);
            Vector3 middle = (end + start) * 0.5f;

            start = middle + ((start - middle).normalized * sideLength);
            end = middle + ((end - middle).normalized * sideLength);

            //Radial circles
            Circle(start, up, color, radius);
            Circle(end, -up, color, radius);

            //Side lines
            Line(start + right, end + right, color);
            Line(start - right, end - right, color);

            Line(start + forward, end + forward, color);
            Line(start - forward, end - forward, color);

            for (int i = 1; i < 26; i++)
            {

                //Start endcap
                Line(Vector3.Slerp(right, -up, i / 25.0f) + start, Vector3.Slerp(right, -up, (i - 1) / 25.0f) + start, color);
                Line(Vector3.Slerp(-right, -up, i / 25.0f) + start, Vector3.Slerp(-right, -up, (i - 1) / 25.0f) + start, color);
                Line(Vector3.Slerp(forward, -up, i / 25.0f) + start, Vector3.Slerp(forward, -up, (i - 1) / 25.0f) + start, color);
                Line(Vector3.Slerp(-forward, -up, i / 25.0f) + start, Vector3.Slerp(-forward, -up, (i - 1) / 25.0f) + start, color);

                //End endcap
                Line(Vector3.Slerp(right, up, i / 25.0f) + end, Vector3.Slerp(right, up, (i - 1) / 25.0f) + end, color);
                Line(Vector3.Slerp(-right, up, i / 25.0f) + end, Vector3.Slerp(-right, up, (i - 1) / 25.0f) + end, color);
                Line(Vector3.Slerp(forward, up, i / 25.0f) + end, Vector3.Slerp(forward, up, (i - 1) / 25.0f) + end, color);
                Line(Vector3.Slerp(-forward, up, i / 25.0f) + end, Vector3.Slerp(-forward, up, (i - 1) / 25.0f) + end, color);
            }
        }
    }
}
