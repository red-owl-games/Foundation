using System;
using UnityEngine;

namespace RedOwl.Core
{
    [Serializable]
    public class SaveGameMetafile : IRedOwlFile
    {
        #region TimePlayedTracking
        
        private static double _lastCounted;
        private static bool _previouslyCounted;
        private static double GetElapsedTime()
        {
            float timeSince = Time.realtimeSinceStartup;
            if (!_previouslyCounted)
            {
                _lastCounted = timeSince;
                _previouslyCounted = true;
            }

            double output = timeSince - _lastCounted;
            _lastCounted = timeSince;
            return output;
        }
        
        #endregion
        
        public string name;
	    
        public double lastSaved;
        public double timePlayed;
        
        public void OnBeforeSerialize()
        {
            lastSaved = TimeExtensions.DateTimeToUnixTimeStamp(DateTime.Now);
            timePlayed += GetElapsedTime();
        }

        public void OnAfterDeserialize()
        {
            
        }
    }
}