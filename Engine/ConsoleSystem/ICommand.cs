using System;

namespace RedOwl.Core
{
    public interface ICommand
    {
        void Invoke(string[] args);
    }
    
    public interface ICommandRegistration : ICommand
    {
        string Name { get; }
        string Description { get; }
    }

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class Command : Attribute
    {
        public string Name { get; private set; }
        public string Description { get; private set; }

        public Command(string name)
        {
            Name = name.ToLower();
            Description = string.Empty;
        }

        public Command(string name, string description)
        {
            Name = name.ToLower();
            Description = description;
        }
    }
    
    public readonly struct ConsoleCommand : ICommandRegistration
    {
        public string Name { get; }
        public string Description { get; }
        
        private readonly Action _callback;

        public ConsoleCommand(string id, string description, Action callback)
        {
            Name = id;
            Description = description;
            _callback = callback;
        }

        public void Invoke(string[] args)
        {
            _callback?.Invoke();
        }
    }

    public readonly struct ConsoleCommandClass : ICommandRegistration
    {
        public string Name { get; }
        public string Description { get; }

        private readonly ICommand _delegate;

        public ConsoleCommandClass(string id, string description, ICommand callback)
        {
            Name = id;
            Description = description;
            _delegate = callback;
        }

        public void Invoke(string[] args)
        {
            // TODO: should be Game.Inject here?
            _delegate.Invoke(args);
        }
    }
}