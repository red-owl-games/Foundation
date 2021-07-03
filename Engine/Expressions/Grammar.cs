using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RedOwl.Engine
{
    public class Grammar<T>
    {
        private Dictionary<string, Func<T>> _constants;
        private Dictionary<string, Func<T, T>> _functions1;
        private Dictionary<string, Func<T, T, T>> _functions2;
        private Dictionary<string, Func<T[], T>> _functionsN;
        
        private Dictionary<TokenTypes, IPrefixParselet> _prefixParselets;
        private Dictionary<TokenTypes, IInfixParselet> _infixParselets;

        public Grammar()
        {
            _constants = new Dictionary<string, Func<T>>();
            _functions1 = new Dictionary<string, Func<T, T>>();
            _functions2 = new Dictionary<string, Func<T, T, T>>();
            _functionsN = new Dictionary<string, Func<T[], T>>();
            
            _prefixParselets = new Dictionary<TokenTypes, IPrefixParselet>();
            _infixParselets = new Dictionary<TokenTypes, IInfixParselet>();
        }
        
        public IEnumerable<string> GetTokenKeys(TokenTypes type)
        {
            yield return type.Symbol().ToString();
            yield return type.Method();
        }
        
        public void AddFunction(string key, Func<T> function, bool force = false)
        {
            if (force || !_constants.ContainsKey(key))
                _constants.Add(key, function);
        }
        
        public void AddFunction(string key, Func<bool> function, bool force = false)
        {
            if (force || !_constants.ContainsKey(key))
                _constants.Add(key, () => function() ? (T)Convert.ChangeType(1, typeof(T)) : (T)Convert.ChangeType(0, typeof(T)));
        }
        
        public void AddFunction(string key, Func<T, T> function, bool force = false)
        {
            if (force || !_functions1.ContainsKey(key))
                _functions1.Add(key, function);
        }

        public void AddFunction(string key, Func<T, bool> function, bool force = false)
        {
            if (force || !_functions1.ContainsKey(key))
                _functions1.Add(key, (x) => function(x) ? (T)Convert.ChangeType(1, typeof(T)) : (T)Convert.ChangeType(0, typeof(T)));
        }
        
        public void AddFunction(string key, Func<T, T, T> function, bool force = false)
        {
            if (force || !_functions2.ContainsKey(key))
                _functions2.Add(key, function);
        }
        
        public void AddFunction(string key, Func<T[], T> function, bool force = false)
        {
            if (force || !_functions2.ContainsKey(key))
                _functionsN.Add(key, function);
        }
        
        public Func<T> GetConstant(string key, bool throwIfMissing = false)
        {
            if (_constants.TryGetValue(key, out var function)) return function;
            if (throwIfMissing) throw new ParseException($"Failed to find method for '{key}'");
            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Func<T, T> GetFunction1(string key, bool throwIfMissing = false)
        {
            if (_functions1.TryGetValue(key, out var function)) return function;
            if (throwIfMissing) throw new ParseException($"Failed to find method for '{key}'");
            return null;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Func<T, T, T> GetFunction2(string key, bool throwIfMissing = false)
        {
            if (_functions2.TryGetValue(key, out var function)) return function;
            if (throwIfMissing) throw new ParseException($"Failed to find method for '{key}'");
            return null;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Func<T[], T> GetFunctionN(string key, bool throwIfMissing = false)
        {
            if (_functionsN.TryGetValue(key, out var function)) return function;
            if (throwIfMissing) throw new ParseException($"Failed to find method for '{key}'");
            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddParselet(TokenTypes type, IPrefixParselet parselet) => 
            _prefixParselets.Add(type, parselet);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddParselet(TokenTypes type, IInfixParselet parselet) => 
            _infixParselets.Add(type, parselet);
        
        public void Prefix(TokenTypes type, Func<T, T> function, int precedence = Precedences.PREFIX)
        {
            AddParselet(type, new PrefixOperatorParselet(precedence));
            foreach (string key in GetTokenKeys(type))
            {
                if (string.IsNullOrEmpty(key)) continue;
                AddFunction(key, function);
            }
        }

        public void Postfix(TokenTypes type, Func<T, T> function, int precedence = Precedences.POSTFIX)
        {
            AddParselet(type, new PostfixOperatorParselet(precedence));
            foreach (string key in GetTokenKeys(type))
            {
                if (string.IsNullOrEmpty(key)) continue;
                AddFunction(key, function);
            }
        }

        public void InfixLeft(TokenTypes type, Func<T, T, T> function, int precedence)
        {
            AddParselet(type, new BinaryOperatorParselet(precedence, false));
            foreach (string key in GetTokenKeys(type))
            {
                if (string.IsNullOrEmpty(key)) continue;
                AddFunction(key, function);
            }
        }

        public void InfixRight(TokenTypes type, Func<T, T, T> function, int precedence)
        {
            AddParselet(type, new BinaryOperatorParselet(precedence, true));
            foreach (string key in GetTokenKeys(type))
            {
                if (string.IsNullOrEmpty(key)) continue;
                AddFunction(key, function);
            }
        }

        public int GetPrecedence(Token token) {
            if (_infixParselets.TryGetValue(token.Type, out var parser))
            {
                return parser.Precedence;
            }
            return 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetPrefix(TokenTypes tokenType, out IPrefixParselet parselet) => 
            _prefixParselets.TryGetValue(tokenType, out parselet);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetInfix(TokenTypes type, out IInfixParselet parselet) =>
            _infixParselets.TryGetValue(type, out parselet);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetConstant(string key, out Func<T> function) => 
            _constants.TryGetValue(key, out function);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetFunction1(string key, out Func<T, T> function) => 
            _functions1.TryGetValue(key, out function);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetFunction2(string key, out Func<T, T, T> function) => 
            _functions2.TryGetValue(key, out function);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetFunctionN(string key, out Func<T[], T> function) => 
            _functionsN.TryGetValue(key, out function);
    }
}