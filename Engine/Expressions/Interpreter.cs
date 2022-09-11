using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RedOwl.Engine
{
    // https://en.wikipedia.org/wiki/Shunting-yard_algorithm
    // https://www.codeproject.com/Tips/351042/Shunting-Yard-algorithm-in-Csharp
    // https://rosettacode.org/wiki/Parsing/Shunting-yard_algorithm#C.23
    // https://github.com/munificent/bantam/tree/master/src/com/stuffwithstuff/bantam
    // https://wiki.unity3d.com/index.php/ExpressionParser
    // https://github.com/bijington/expressive
    public class Interpreter<T> where T : IConvertible
    {
        public Dictionary<string, T> Variables;

        private Grammar<T> _grammar;
        private Lexer _lexer;
        private Parser<T> _parser;

        public Grammar<T> Grammar => _grammar;

        public T this[string key]
        {
            get => Variables[key];
            set => Variables[key] = value;
        }

        public Interpreter()
        {
            Variables = new Dictionary<string, T>();

            _grammar = new Grammar<T>();
            _lexer = new Lexer();
            _parser = new Parser<T>(_grammar);
        }

        // TODO: auto register conversion function parslet? but every const/func is typed is this needed?
        // Would this allow use to make const/funcs take a more generic type to support a wider array of functions?
        // public T Convert()
        // {
        //     return (x) => (double)Convert.ChangeType(x, typeof(double))
        // }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Get(string key) => 
            Variables.TryGetValue(key, out var value) ? value : default;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerable<string> GetAll() => 
            Variables.Keys;

        public T Set(string key, T value, bool force = true)
        {
            if (force || !Variables.ContainsKey(key))
                Variables[key] = value;
            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Eval(string expression) => 
            Compile(expression)(Variables);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Func<IDictionary<string, T>, T> Compile(string expression) =>
            _parser.Parse(_lexer.Read(expression)).Evaluate;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddParselet(TokenTypes type, IPrefixParselet parselet) => 
            _grammar.AddParselet(type, parselet);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddParselet(TokenTypes type, IInfixParselet parselet) => 
            _grammar.AddParselet(type, parselet);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddFunction(string key, Func<T> function, bool force = false) =>
            _grammar.AddFunction(key, function, force);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddFunction(string key, Func<bool> function, bool force = false) =>
            _grammar.AddFunction(key, function, force);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddFunction(string key, Func<T, T> function, bool force = false) =>
            _grammar.AddFunction(key, function, force);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddFunction(string key, Func<T, bool> function, bool force = false) =>
            _grammar.AddFunction(key, function, force);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddFunction(string key, Func<T, T, T> function, bool force = false) =>
            _grammar.AddFunction(key, function, force);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddFunction(string key, Func<T, T, T, T> function, bool force = false) =>
            _grammar.AddFunction(key, function, force);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddFunction(string key, Func<T, T, T, T, T> function, bool force = false) =>
            _grammar.AddFunction(key, function, force);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddFunction(string key, Func<T, T, T, T, T, T> function, bool force = false) =>
            _grammar.AddFunction(key, function, force);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddFunction(string key, Func<T[], T> function, bool force = false) =>
            _grammar.AddFunction(key, function, force);
        
        public void Prefix(TokenTypes type, Func<T, T> function, int precedence = Precedences.PREFIX) => 
            _grammar.Prefix(type, function, precedence);

        public void Postfix(TokenTypes type, Func<T, T> function, int precedence = Precedences.POSTFIX) => 
            _grammar.Postfix(type, function, precedence);

        public void InfixLeft(TokenTypes type, Func<T, T, T> function, int precedence) => 
            _grammar.InfixLeft(type, function, precedence);

        public void InfixRight(TokenTypes type, Func<T, T, T> function, int precedence) => 
            _grammar.InfixRight(type, function, precedence);
        
        public static implicit operator Dictionary<string, T>(Interpreter<T> s) => s.Variables;
    }
}