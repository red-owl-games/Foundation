using System;

namespace RedOwl.Engine
{
    public class RedOwlException : Exception
    {
        public RedOwlException() {}

        public RedOwlException(string message) : base(message) {}

        public RedOwlException(string message, Exception inner) : base(message, inner) {}
    }
}