using System.Diagnostics;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace RedOwl.Core
{
    public partial class CoreSettings
    {
        [SerializeField]
        [ToggleLeft, HorizontalGroup("Draw", 0.3f), LabelWidth(120)]
        private bool showDebugDraw = true;

        public static bool ShowDebugDraw => Instance.showDebugDraw;

        [SerializeField]
        [HorizontalGroup("Draw"), ShowIf("showDebugDraw"), HideLabel]
        private Color debugDrawColor = Color.magenta;

        public static Color DebugDrawColor => Instance.debugDrawColor;
    }
    
    public static class Draw
    {
        [Conditional("UNITY_EDITOR")]
        public static void Line(Vector3 start, Vector3 end, Color? color = null)
        {
            if (!CoreSettings.ShowDebugDraw) return;
            UnityEngine.Debug.DrawLine(start, end, color ?? CoreSettings.DebugDrawColor, 0f, true);
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

                Line(legStart, legEnd);
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
