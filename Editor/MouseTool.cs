using Unity.Mathematics;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

namespace RedOwl.Editor
{
    public abstract class MouseTool : EditorTool
    {
        protected Event Event { get; private set; }
        protected int3 Start { get; private set; }
        protected int3 End { get; private set; }
        protected bool IsDragging { get; private set; }
        
        protected bool IsShiftPressed => (Event.modifiers & EventModifiers.Shift) == EventModifiers.Shift;
        protected bool IsCtrlPressed => (Event.modifiers & EventModifiers.Control) == EventModifiers.Control;
        protected bool IsAltPressed => (Event.modifiers & EventModifiers.Alt) == EventModifiers.Alt;
        
        protected Bounds GetBounds()
        {
            var s = new Vector3(Start.x, Start.y, Start.z);
            var e = new Vector3(End.x, End.y, End.z);
            var min = Vector3.Min(s, e);
            var max = Vector3.Max(s, e) + Vector3.one;
            var bounds = new Bounds((min + max) / 2f, max - min);
            return bounds;
        }
        
        public override void OnToolGUI(EditorWindow window)
        {
            Event = Event.current;
            var view = window as SceneView;
            if (view == null) return;
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

            int3 point = GetCursorPoint(HandleUtility.GUIPointToWorldRay(Event.mousePosition));
            switch (Event.type)
            {
                case EventType.MouseUp when Event.button == 1:
                    if (OnRightClick()) Event.Use();
                    break;
                case EventType.MouseLeaveWindow:
                    IsDragging = false;
                    Start = default;
                    End = default;
                    break;
                case EventType.MouseDown when Event.button == 0 && !IsAltPressed:
                    IsDragging = true;
                    Event.Use();
                    break;
                case EventType.MouseDrag when IsDragging:
                    End = point;
                    Event.Use();
                    break;
                case EventType.MouseMove when !IsDragging:
                    Start = End = point;
                    break;
                case EventType.MouseUp when IsDragging:
                    IsDragging = false;
                    if (OnLeftClick(out var newStart, out var newEnd))
                    {
                        Start = newStart;
                        End = newEnd;
                        Event.Use();
                    }
                    break;
            }
            DrawPreview();
            view.Repaint();
        }

        protected abstract int3 GetCursorPoint(Ray ray);
        protected abstract void DrawPreview();
        protected abstract bool OnLeftClick(out int3 start, out int3 end);
        protected abstract bool OnRightClick();
    }
}