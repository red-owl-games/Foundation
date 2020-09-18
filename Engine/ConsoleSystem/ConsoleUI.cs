using System;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

namespace RedOwl.Core
{
    public class ConsoleUI : MonoBehaviour
    {
        private const string FocusControlName = "CommandLine";
        [ShowInInspector]
        private bool _show;
        private Vector2 _scrollPosition;
        private string _command;
        private Texture2D _texture;
        private int historyIndex = -1;
        private RingBuffer<string> history;

        private void OnEnable()
        {
            ConsoleSettings.ShowConsoleAction.Enable();
        }

        private void Start()
        {
            history = new RingBuffer<string>(ConsoleSettings.HistoryBufferLength);
            history.PushFront("help");
            history.PushFront("clear");
            
            _texture = new Texture2D(1,1);
            _texture.SetPixel(0, 0, new Color(0, 0, 0, .9f));
            _texture.Apply();

            ConsoleSettings.ShowConsoleAction.performed += ctx => ToggleShow();
        }

        private void OnDisable()
        {
            ConsoleSettings.ShowConsoleAction.Disable();
        }
        
        private void OnApplicationQuit()
        {
            _show = false;
        }

        private void ToggleShow() => _show = !_show;

        private void OnGUI()
        {
            if (!_show || Console.Logs == null)
            {
                _command = "";
                return;
            }

            // TODO: Pause Game Time

            int screenWidth = Screen.width;
            int screenHeight = Screen.height;
            float border = 10f;
            float areaWidth = screenWidth - 20f;

            var screen = new Rect(0, 0, screenWidth, screenHeight);
            var logs = new Rect(border, border, areaWidth, screenHeight - 85f);
            var scroll = new Rect(0, 0, areaWidth - 20f, math.max(screenHeight - 85f, (Console.Logs.Size * 15f) + 10f));
            var commandline = new Rect(border, screenHeight - 70f, areaWidth, 60f);
            
            string logLines = "";
            if (Console.Logs.Size > 0)
            {
                logLines = string.Join(Environment.NewLine, Console.Logs);
            }
            
            Event e = Event.current;
            switch (e.type)
            {
                case EventType.KeyDown when e.keyCode == KeyCode.Return:
                {
                    if (string.IsNullOrEmpty(_command) || _command.Split(new []{' '}, StringSplitOptions.RemoveEmptyEntries).Length < 1) break;
                    try
                    {
                        historyIndex = -1;
                        if (history.Front() != _command)
                            history.PushFront(_command);
                        Console.Run(_command);
                    }
                    catch (Exception exception)
                    {
                        Log.Error(exception.ToString());
                    }
                    _command = "";
                    break;
                }
                // case EventType.KeyUp when e.keyCode == KeyCode.UpArrow:
                // {
                //     historyIndex = math.min(historyIndex + 1, history.Size - 1);
                //     if (historyIndex > -1)
                //     {
                //         _command = history[historyIndex];
                //     }
                //     return;
                // }
                // case EventType.KeyUp when e.keyCode == KeyCode.DownArrow:
                // {
                //     historyIndex = math.max(historyIndex - 1, -1);
                //     if (historyIndex > -1)
                //     {
                //         _command = history[historyIndex];
                //     }
                //     if (historyIndex == -1) _command = "";
                //     return;
                // }
            }

            // Draw Transparent Rect
            GUI.Box(screen, "", new GUIStyle(GUI.skin.box) {normal = {background = _texture}} );
            
            // Draw Log Lines
            _scrollPosition = GUI.BeginScrollView(logs, _scrollPosition, scroll);
            GUI.TextArea(scroll, logLines, new GUIStyle(GUI.skin.textArea) { richText = true, fontSize = ConsoleSettings.FontSize});
            GUI.EndScrollView();

            GUI.SetNextControlName(FocusControlName);
            // Draw Commandline
            _command = GUI.TextField(commandline, _command, new GUIStyle(GUI.skin.textField) { fontSize = ConsoleSettings.FontSize});

            if (_show)
            {
                GUI.FocusControl(FocusControlName);
            }
        }
    }
}