using RedOwl.Engine;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    public enum LogTypes
    {
        Always,
        Debug,
        Info,
        Warn,
        Error
    }
    
    [HideMonoScript]
    public class DebugLog : MonoBehaviour
    {
        public LogTypes type;
        
        [SerializeField]
        [TextArea(3, 6)]
        private string message = "";

        public void Print() => Print(message);
        public void Print(string msg)
        {    
            switch (type)
            {
                case LogTypes.Always:
                    Log.Always(msg);
                    break;
                case LogTypes.Debug:
                    Log.Debug(msg);
                    break;
                case LogTypes.Info:
                    Log.Info(msg);
                    break;
                case LogTypes.Warn:
                    Log.Warn(msg);
                    break;
                case LogTypes.Error:
                    Log.Error(msg);
                    break;
            }
            
        }
    }
}