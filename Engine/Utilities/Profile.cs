using System;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Profiling;

namespace RedOwl.Engine
{
    /*
    public void Example()
    {
        using(Profile("Name.Of.Profile.Maker"))
        {
            ... Something you want to profile ...
        }
    }
    */
    public struct Profile : IDisposable
    {
        private static readonly Dictionary<string, ProfilerMarker> Markers = new Dictionary<string, ProfilerMarker>();

        private readonly string _name;

        public Profile(string name)
        {
            _name = name;
            if (Markers.TryGetValue(_name, out var marker))
                marker.Begin();
            else
            {
                marker = new ProfilerMarker(_name);
                Markers.Add(_name, marker);
                marker.Begin();
            }
        }

        public void Dispose()
        {
            if (Markers.TryGetValue(_name, out var marker))
                marker.End();
        }
    }
    
    /*
    public void Example()
    {
        using(Timer("Name.Of.Timer.Maker"))
        {
            ... Something you want to time ...
        }
    }
    */
    public struct Timer : IDisposable
    {
        private static readonly Dictionary<string, Stopwatch> Watches = new Dictionary<string, Stopwatch>();
        
        private readonly string _name;

        public Timer(string name)
        {
            _name = name;
            if (Watches.TryGetValue(_name, out var w))
                w.Restart();
            else
            {
                var watch = new Stopwatch();
                Watches.Add(_name, watch);
                watch.Start();
            }
        }

        public void Dispose()
        {
            if (Watches.TryGetValue(_name, out var w))
            {
                w.Stop();
                TimeSpan ts = w.Elapsed;
                Log.Always($"[{_name}] RunTime: {ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}.{ts.Milliseconds / 10:00}");
            }
        }
    }
}