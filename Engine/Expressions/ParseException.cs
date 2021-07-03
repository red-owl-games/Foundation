using System;

namespace RedOwl.Engine
{
    public class ParseException : Exception 
    {
        public ParseException(string message) : base(message) {}
    }
}