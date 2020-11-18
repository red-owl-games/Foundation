using System;
using System.Collections.Generic;

namespace RedOwl.Engine
{
    [Command("clear", "Clears the console GUI's log text")]
    public class ClearCommand : ICommand
    {
        public void Invoke(string[] args)
        {
            Game.Console.Clear();
        }
    }
    
    [Command("help", "Prints all commands and their descriptions")]
    public class HelpCommand : ICommand
    {
        public void Invoke(string[] args)
        {
            var commands = new List<ICommandRegistration>(Game.Console.Commands);
            commands.Sort((x, y) => string.Compare(x.Name, y.Name, StringComparison.Ordinal));
            Log.Always("----------HELP----------");
            foreach (var command in commands)
            {
                Log.Always($"{command.Name} - {command.Description}");
            }
            Log.Always("----------HELP----------");
        }
    }
}