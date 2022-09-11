using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RedOwl.Engine
{
    public static class Precedences 
    {
        // Ordered in increasing precedence.
        public const int ASSIGNMENT      = 1;
        public const int TERNARY         = 2;
        public const int NULL_COALESCING  = 3;
        public const int CONDITIONAL_OR    = 4;
        public const int CONDITIONAL_AND  = 5;
        public const int BITWISE_OR       = 6;
        public const int BITWISE_XOR      = 7;
        public const int BITWISE_AND      = 8;
        public const int EQUALITY        = 9;
        public const int RELATIONAL      = 10;
        public const int BITWISE_SHIFT    = 11;
        public const int SUM             = 12;
        public const int PRODUCT         = 13;
        public const int UNARY           = 14;
        public const int PRIMARY         = 15;
        public const int PREFIX          = 16;
        public const int POSTFIX         = 17;
        public const int CALL            = 18;
    }

    public interface IParser<T> where T : IConvertible
    {
        Grammar<T> Grammar { get; }
        IExpressionNode<T> ParseNext();
        IExpressionNode<T> ParseNext(int precedence);

        bool Match(TokenTypes expected, bool consume = true);
        Token Consume();
        Token Consume(TokenTypes expected);
        Token LookAhead(int distance);
    }
    
    public class Parser<T> : IParser<T> where T : IConvertible
    {
        private IEnumerator<Token> _tokens;
        private List<Token> _read;
        private Grammar<T> _grammar;

        public Grammar<T> Grammar => _grammar;

        public Parser(Grammar<T> grammar)
        {
            _tokens = null;
            _read = new List<Token>();
            _grammar = grammar;
        }
        
        public IExpressionNode<T> Parse(IEnumerator<Token> tokens)
        {
            // TODO: should _tokens and _read be part of the Lexer?
            // This would make functions like Lexer.Consume() and Lexer.Match()
            // This would also mean the state of these gets automatically reset when Lexer.Read() happens
            _tokens = tokens;
            _read.Clear();
            return ParseNext();
        }
        
        public IExpressionNode<T> ParseNext(int precedence)
        {
            Token token = Consume();
            //Log.Always($"PARSENEXT: {token}");
            if (token.Type == TokenTypes.EOF)
            {
                return new NullExpressionNode<T>();
            }
            IExpressionNode<T> left = null;
            if (!_grammar.TryGetPrefix(token.Type, out var prefix))
            {
                if (_grammar.TryGetInfix(token.Type, out var infix))
                {
                    left = infix.Parse(this, left, token);
                }
                else
                {
                    throw new ParseException($"Could not find a parselet for token {token}");
                }
            }
            else
            {
                left = prefix.Parse(this, token);
            }

            while (precedence < _grammar.GetPrecedence(LookAhead(0))) {
                token = Consume();
                //Log.Always($"PARSENEXT2: {token}");
                if (_grammar.TryGetInfix(token.Type, out var infix))
                {
                    left = infix.Parse(this, left, token);
                }
            }

            return left;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IExpressionNode<T> ParseNext() => ParseNext(0);
        
        public bool Match(TokenTypes expected, bool consume = true) {
            Token token = LookAhead(0);
            if (token.Type != expected) {
                return false;
            }
    
            if (consume) Consume();
            return true;
        }
  
        public Token Consume(TokenTypes expected) {
            Token token = LookAhead(0);
            if (token.Type != expected) {
                throw new ParseException($"Expected token '{expected}' and found '{token.Type}");
            }
    
            return Consume();
        }
  
        public Token Consume() {
            // Make sure we've read the token.
            LookAhead(0);
            try
            {
                return _read[0];
            }
            finally
            {
                _read.RemoveAt(0);
            }
        }
  
        public Token LookAhead(int distance) {
            // Read in as many as needed.
            while (distance >= _read.Count)
            {
                _tokens.MoveNext();
                _read.Add(_tokens.Current);
            }
            // Get the queued token.
            //Log.Always($"Reading Token '{_read[distance]}|{_read[distance].Type}'");
            return _read[distance];
        }
    }
}