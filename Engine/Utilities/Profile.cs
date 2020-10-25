using System;
using System.Collections.Generic;
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
        private static readonly Dictionary<string, ProfilerMarker> markers = new Dictionary<string, ProfilerMarker>();

        private readonly string _name;

        public Profile(string name)
        {
            _name = name;
            if (markers.TryGetValue(_name, out var marker))
                marker.Begin();
            else
            {
                marker = new ProfilerMarker(_name);
                markers.Add(_name, marker);
                marker.Begin();
            }
        }

        public void Dispose()
        {
            if (markers.TryGetValue(_name, out var marker))
                marker.End();
        }
    }
}