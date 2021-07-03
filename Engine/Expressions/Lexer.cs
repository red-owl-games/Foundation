using System;
using System.Collections;
using System.Collections.Generic;

namespace RedOwl.Engine
{
    
    // TODO: I think if the lexer combind tokens like != and && it would make the parser work better
    public enum TokenTypes
    {
        LEFT_PAREN,
        RIGHT_PAREN,
        LEFT_BRACKET,
        RIGHT_BRACKET,
        LEFT_CURLY,
        RIGHT_CURLY,
        COMMA,
        LESS_THEN,
        GREATER_THEN,
        EQUALS,
        PLUS,
        MINUS,
        ASTERISK,
        SLASH,
        PIPE,
        CARET,
        TILDE,
        BANG,
        QUESTION,
        PERCENT,
        AMPERSAND,
        DOLLARSIGN,
        HASH,
        COLON,
        SEMI_COLON,
        PERIOD,
        
        NAME,
        NUMBER,
        EOF
    }

    public static class TokenTypesExtensions
    {
        private static Dictionary<TokenTypes, char> _symbolTable = new Dictionary<TokenTypes, char>()
        {
            [TokenTypes.LEFT_PAREN] = '{',
            [TokenTypes.LEFT_PAREN] = '(',
            [TokenTypes.RIGHT_PAREN] = ')',
            [TokenTypes.LEFT_BRACKET] = '[',
            [TokenTypes.RIGHT_BRACKET] = ']',
            [TokenTypes.LEFT_CURLY] = '{',
            [TokenTypes.RIGHT_CURLY] = '}',
            [TokenTypes.COMMA] = ',',
            [TokenTypes.LESS_THEN] = '<',
            [TokenTypes.GREATER_THEN] = '>',
            [TokenTypes.EQUALS] = '=',
            [TokenTypes.PLUS] = '+',
            [TokenTypes.MINUS] = '-',
            [TokenTypes.ASTERISK] = '*',
            [TokenTypes.SLASH] = '/',
            [TokenTypes.PIPE] = '|',
            [TokenTypes.CARET] = '^',
            [TokenTypes.TILDE] = '~',
            [TokenTypes.BANG] = '!',
            [TokenTypes.QUESTION] = '?',
            [TokenTypes.PERCENT] = '%',
            [TokenTypes.AMPERSAND] = '&',
            [TokenTypes.DOLLARSIGN] = '$',
            [TokenTypes.HASH] = '#',
            [TokenTypes.COLON] = ':',
            [TokenTypes.SEMI_COLON] = ';',
            [TokenTypes.PERIOD] = '.',
        };
        
        public static char? Symbol(this TokenTypes self)
        {
            if (_symbolTable.TryGetValue(self, out char symbol))
                return symbol;
            return null;
        }

        private static Dictionary<TokenTypes, string> _methodTable = new Dictionary<TokenTypes, string>
        {
            [TokenTypes.LESS_THEN] = "lt",
            [TokenTypes.GREATER_THEN] = "gt",
            [TokenTypes.EQUALS] = "eq",
            [TokenTypes.PLUS] = "add",
            [TokenTypes.MINUS] = "sub",
            [TokenTypes.ASTERISK] = "mul",
            [TokenTypes.SLASH] = "div",
            [TokenTypes.CARET] = "pow",
            [TokenTypes.BANG] = "not",
            [TokenTypes.QUESTION] = "if",
            [TokenTypes.PERCENT] = "percent",
            //[TokenTypes.AMPERSAND:     return "&";
            [TokenTypes.COLON] = "else",
        };

        public static string Method(this TokenTypes self)
        {
            if (_methodTable.TryGetValue(self, out string name))
                return name;
            return null;
        }
    }
    
    public readonly struct Token 
    {
        public TokenTypes Type { get; }
        public string Text { get; }
        
        public Token(TokenTypes type, string text) {
            Type = type;
            Text = text;
        }

        public override string ToString() => Text;
    }
    
    public class Lexer : IEnumerable<Token>
    {
        private int _index;
        private string _text;
        private Dictionary<char, TokenTypes> _punctuators;
        
        public Lexer() {
            _index = 0;
            _text = "";
            _punctuators = new Dictionary<char, TokenTypes>();
    
            foreach (var type in ForEachTokenType()) {
                char? symbol = type.Symbol();
                if (symbol != null) {
                    _punctuators.Add(symbol.Value, type);
                }
            }
        }

        public IEnumerator<Token> Read(string text)
        {
            _index = 0;
            _text = text;
            return GetEnumerator();
        }
        
        public IEnumerable<TokenTypes> ForEachTokenType()
        {
            foreach (string name in Enum.GetNames(typeof(TokenTypes)))
            {
                if (Enum.TryParse<TokenTypes>(name, out var type))
                {
                    yield return type;
                }
            }
        }

        private bool IsAllDigit(string text)
        {
            foreach (char c in text)
            {
                if (!char.IsDigit(c)) return false;
            }

            return true;
        }

        public IEnumerator<Token> GetEnumerator()
        {
            var length = _text.Length;
            while (_index < length) {
                char c = _text[_index++];
      
                // fast out on whitespace
                if (char.IsWhiteSpace(c)) {}
                else if (_punctuators.ContainsKey(c)) {// TODO: char.IsPunctuation(c) ?
                    yield return new Token(_punctuators[c], c.ToString());
                }
                else if (char.IsLetterOrDigit(c)) 
                {
                    int start = _index - 1;
                    while (_index < length) {
                        if (!char.IsLetterOrDigit(_text[_index])) break;
                        _index++;
                    }
        
                    string name = _text.Substring(start, _index - start);
                    if (IsAllDigit(name))
                    {
                        yield return new Token(TokenTypes.NUMBER, name);
                    }
                    else
                    {
                        yield return new Token(TokenTypes.NAME, name);
                    }
                }
            }
    
            // Once we've reached the end of the string, just return EOF tokens. We'll
            // just keeping returning them as many times as we're asked so that the
            // parser's lookahead doesn't have to worry about running out of tokens.
            yield return new Token(TokenTypes.EOF, "");
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}