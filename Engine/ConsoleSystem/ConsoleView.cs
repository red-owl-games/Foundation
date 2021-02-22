using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    public interface IConsoleView
    {
        void Toggle();
    }
    
    [HideMonoScript]
    public class ConsoleView : MonoBehaviour, IConsoleView
    {
        // private const string FocusControlName = "CommandLine";
        // private Vector2 _scrollPosition;
        // private string _command;

        [SerializeField]
        private CanvasGroup canvasGroup;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0;
        }

        public void Toggle() => canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;

        /*
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
                    var parsed = _command.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                    if (string.IsNullOrEmpty(_command) || parsed.Length < 1) break;
                    try
                    {
                        _historyIndex = -1;
                        if (_history.Front() != _command)
                            _history.PushFront(_command);
                        Console.Run(parsed);
                    }
                    catch (Exception exception)
                    {
                        Log.Error(exception.ToString());
                    }
                    _command = "";
                    break;
                }
                case EventType.KeyUp when e.keyCode == KeyCode.UpArrow:
                {
                    _historyIndex = math.min(_historyIndex + 1, _history.Size - 1);
                    if (_historyIndex > -1)
                    {
                        _command = _history[_historyIndex];
                    }
                    return;
                }
                case EventType.KeyUp when e.keyCode == KeyCode.DownArrow:
                {
                    _historyIndex = math.max(_historyIndex - 1, -1);
                    if (_historyIndex > -1)
                    {
                        _command = _history[_historyIndex];
                    }
                    if (_historyIndex == -1) _command = "";
                    return;
                }
            }

            // Draw Transparent Rect
            GUI.Box(screen, "", new GUIStyle(GUI.skin.box) {normal = {background = _texture}} );
            
            // Draw Log Lines
            _scrollPosition = GUI.BeginScrollView(logs, _scrollPosition, scroll);
            GUI.TextArea(scroll, logLines, new GUIStyle(GUI.skin.textArea) { richText = true, fontSize = Game.ConsoleSettings.FontSize});
            GUI.EndScrollView();

            GUI.SetNextControlName(FocusControlName);
            // Draw Commandline
            _command = GUI.TextField(commandline, _command, new GUIStyle(GUI.skin.textField) { fontSize = Game.ConsoleSettings.FontSize});

            if (_show)
            {
                GUI.FocusControl(FocusControlName);
            }
        }
        */
    }
}