using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace RedOwl.Engine
{
    public interface IParselet {}
    
    public interface IPrefixParselet : IParselet
    {
        IExpressionNode<T> Parse<T>(IParser<T> parser, Token token) where T : IConvertible;
    }
    
    public interface IInfixParselet : IParselet
    {
        int Precedence { get; }
        IExpressionNode<T> Parse<T>(IParser<T> parser, IExpressionNode<T> left, Token token) where T : IConvertible;
    }

    public class NameParselet : IPrefixParselet
    {
        public IExpressionNode<T> Parse<T>(IParser<T> parser, Token token) where T : IConvertible
        {
            // is token.Text a constant?
            // TODO: this logic is muddy
            if (parser.LookAhead(0).Type != TokenTypes.LEFT_PAREN && parser.Grammar.TryGetConstant(token.Text, out var constant))
            {
                return new CallExpressionNode<T>((v) => constant());
            }
            
            var name = new NameExpressionNode<T>(token.Text);

            // TODO: does this really have to live here? is there no way for a prefix to get wants on the left of it?
            if (parser.LookAhead(0).Type == TokenTypes.PLUS && parser.LookAhead(1).Type == TokenTypes.PLUS)
            {
                parser.Match(TokenTypes.PLUS);
                parser.Match(TokenTypes.PLUS);
                return new PostIncrementExpressionNode<T>(parser.Grammar.GetFunction1("++"), name);
            }
            
            if (parser.LookAhead(0).Type == TokenTypes.MINUS && parser.LookAhead(1).Type == TokenTypes.MINUS)
            {
                parser.Match(TokenTypes.MINUS);
                parser.Match(TokenTypes.MINUS);
                return new PostIncrementExpressionNode<T>(parser.Grammar.GetFunction1("--"), name);
            }

            return name;
        }
    }
    
    public class NumberParselet : IPrefixParselet 
    {
        public IExpressionNode<T> Parse<T>(IParser<T> parser, Token token) where T : IConvertible
        {
            // Check if next Token == TokenTypes.PERIOD then continue parseing to make a decimal number
            var noLeadingDecimal = token.Type == TokenTypes.PERIOD;
            if (noLeadingDecimal || parser.Match(TokenTypes.PERIOD))
            {
                var right = parser.ParseNext();
                if (right is NumberExpressionNode<T> number)
                {
                    return new NumberExpressionNode<T>(noLeadingDecimal ? $"0.{number.Number}" : $"{token.Text}.{number.Number}");
                }
                return right;
            }
            return new NumberExpressionNode<T>(token.Text);
        }
    }

    public class IncrementParselet : IPrefixParselet
    {
        public int Precedence { get; }
        
        public IncrementParselet(int precedence) {
            Precedence = precedence;
        }
        
        public IExpressionNode<T> Parse<T>(IParser<T> parser, Token token) where T : IConvertible
        {
            bool matched = parser.Match(token.Type);
            var right = parser.ParseNext(Precedence);
            switch (right)
            {
                case NameExpressionNode<T> name:
                    return new PreIncrementExpressionNode<T>(parser.Grammar.GetFunction1($"{token.Text}{token.Text}", true), name);
                case NumberExpressionNode<T> number:
                    return new NumberExpressionNode<T>($"{token.Text}{number.Number}");
                default:
                    return right;
            }
        }
    }

    public class PrefixOperatorParselet : IPrefixParselet 
    {
        public int Precedence { get; }
        
        public PrefixOperatorParselet(int precedence) {
            Precedence = precedence;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IExpressionNode<T> Parse<T>(IParser<T> parser, Token token) where T : IConvertible => 
            new PrefixExpressionNode<T>(parser.Grammar.GetFunction1(token.Text, true), parser.ParseNext(Precedence));
    }

    public class PostfixOperatorParselet : IInfixParselet 
    {
        public int Precedence { get; }

        public PostfixOperatorParselet(int precedence) {
            Precedence = precedence;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IExpressionNode<T> Parse<T>(IParser<T> parser, IExpressionNode<T> left, Token token) where T : IConvertible => 
            new PostfixExpressionNode<T>(left, parser.Grammar.GetFunction1(token.Text, true));
    }
    
    public class BinaryOperatorParselet : IInfixParselet 
    {
        public int Precedence { get; }
        private readonly bool _isRight;
        
        public BinaryOperatorParselet(int precedence, bool isRight) {
            Precedence = precedence;
            _isRight = isRight;
        }
        public IExpressionNode<T> Parse<T>(IParser<T> parser, IExpressionNode<T> left, Token token) where T : IConvertible
        {
            // To handle right-associative operators like "^", we allow a slightly
            // lower precedence when parsing the right-hand side. This will let a
            // parselet with the same precedence appear on the right, which will then
            // take *this* parselet's result as its left-hand argument.
            var right = parser.ParseNext(Precedence - (_isRight ? 1 : 0));
            return new OperatorExpressionNode<T>(left, parser.Grammar.GetFunction2(token.Text, true), right);
        }
    }
    
    public class EqualsParselet : IInfixParselet
    {
        public int Precedence => Precedences.EQUALITY;

        public IExpressionNode<T> Parse<T>(IParser<T> parser, IExpressionNode<T> left, Token token) where T : IConvertible
        {
            if (parser.Match(TokenTypes.EQUALS))
                return new OperatorExpressionNode<T>(left, parser.Grammar.GetFunction2("==", true), parser.ParseNext(Precedence - 1));
            if (!(left is NameExpressionNode<T>))
                throw new ParseException("The left-hand side of an assignment must be a name.");
            return new AssignExpressionNode<T>(((NameExpressionNode<T>) left).Name, parser.ParseNext(Precedence - 1));
        }
    }

    public class BangParselet : IInfixParselet
    {
        public int Precedence => Precedences.EQUALITY;

        public IExpressionNode<T> Parse<T>(IParser<T> parser, IExpressionNode<T> left, Token token) where T : IConvertible
        {
            if (left is NameExpressionNode<T>)
                return new PostfixExpressionNode<T>(left, parser.Grammar.GetFunction1("!", true));
            
            if (parser.Match(TokenTypes.EQUALS))
                return new OperatorExpressionNode<T>(left, parser.Grammar.GetFunction2("!=", true), parser.ParseNext( Precedence - 1));

            // var right = parser.ParseNext(Precedence - 1);
            // if (!(right is NameExpressionNode<T>))
            //     return new PrefixExpressionNode<T>(parser.Grammar.GetFunction1("!", true), right);
            throw new ParseException("TODO:");
        }
    }
    
    public class GreaterThanParselet : IInfixParselet
    {
        public int Precedence => Precedences.RELATIONAL;

        public IExpressionNode<T> Parse<T>(IParser<T> parser, IExpressionNode<T> left, Token token) where T : IConvertible
        {
            if (parser.Match(TokenTypes.EQUALS))
                return new OperatorExpressionNode<T>(left, parser.Grammar.GetFunction2(">=", true), parser.ParseNext(Precedence - 1));
            if (parser.Match(TokenTypes.GREATER_THEN))
                return new OperatorExpressionNode<T>(left, parser.Grammar.GetFunction2(">>", true), parser.ParseNext(Precedence - 1));
            return new OperatorExpressionNode<T>(left, parser.Grammar.GetFunction2(">", true), parser.ParseNext(Precedence - 1));
        }
    }
    
    public class LessThanParselet : IInfixParselet
    {
        public int Precedence => Precedences.RELATIONAL;

        public IExpressionNode<T> Parse<T>(IParser<T> parser, IExpressionNode<T> left, Token token) where T : IConvertible
        {
            if (parser.Match(TokenTypes.EQUALS))
                return new OperatorExpressionNode<T>(left, parser.Grammar.GetFunction2("<=", true), parser.ParseNext(Precedence - 1));
            if (parser.Match(TokenTypes.LESS_THEN))
                return new OperatorExpressionNode<T>(left, parser.Grammar.GetFunction2("<<", true), parser.ParseNext(Precedence - 1));
            return new OperatorExpressionNode<T>(left, parser.Grammar.GetFunction2("<", true), parser.ParseNext(Precedence - 1));
        }
    }
    
    public class ConditionalParselet : IInfixParselet
    {
        public int Precedence => Precedences.TERNARY;

        public IExpressionNode<T> Parse<T>(IParser<T> parser, IExpressionNode<T> left, Token token) where T : IConvertible
        {
            var thenArm = parser.ParseNext();
            parser.Consume(TokenTypes.COLON);
            var elseArm = parser.ParseNext(Precedences.TERNARY - 1);
            return new ConditionalExpressionNode<T>(left, thenArm, elseArm);
        }
    }

    // Not used because NullCoalescing is not implemented
    // public class NullCoalescingParselet : IInfixParselet
    // {
    //     public int Precedence => Precedences.NULLCOALESCING;
    //
    //     public IExpressionNode<T> Parse<T>(IParser<T> parser, IExpressionNode<T> left, Token token) where T : IConvertible
    //     {
    //         var right = parser.ParseNext(Precedences.NULLCOALESCING - 1);
    //         throw new NotImplementedException($"TODO: Null Coalescing not currently supported - {left} ?? {right}");
    //     }
    // }
    
    // public abstract class DualInfixParselet<TSingle, TDual> : IInfixParselet where TSingle : IInfixParselet, new() where TDual : IInfixParselet, new()
    // {
    //     private IInfixParselet single;
    //     private IInfixParselet dual;
    //
    //     public int Precedence => single.Precedence < dual.Precedence ? single.Precedence : dual.Precedence;
    //     
    //     protected DualInfixParselet()
    //     {
    //         single = new TSingle();
    //         dual = new TDual();
    //     }
    //     
    //     [MethodImpl(MethodImplOptions.AggressiveInlining)]
    //     public IExpressionNode<T> Parse<T>(IParser<T> parser, IExpressionNode<T> left, Token token) where T : IConvertible => 
    //         parser.Match(token.Type) ? dual.Parse(parser, left, token) : single.Parse(parser, left, token);
    // }
    
    // public class QuestionMarkParselet : DualInfixParselet<ConditionalParselet, NullCoalescingParselet> {}
    
    public class GroupParselet : IPrefixParselet 
    {
        public IExpressionNode<T> Parse<T>(IParser<T> parser, Token token) where T : IConvertible
        {
            var expressionNode = parser.ParseNext();
            parser.Consume(TokenTypes.RIGHT_PAREN);
            return expressionNode;
        }
    }
    
    public class CallParselet : IInfixParselet 
    {
        public int Precedence => Precedences.CALL;
        
        public IExpressionNode<T> Parse<T>(IParser<T> parser, IExpressionNode<T> left, Token token) where T : IConvertible
        {
            var args = new List<IExpressionNode<T>>();
        
            // Parse the comma-separated arguments until we hit, ")".
            // There may be no arguments at all.
            if (!parser.Match(TokenTypes.RIGHT_PAREN)) {
                args.Add(parser.ParseNext());
                while (parser.Match(TokenTypes.COMMA)) {
                    args.Add(parser.ParseNext());
                }
                parser.Consume(TokenTypes.RIGHT_PAREN);
            }

            if (left is CallExpressionNode<T>) return left;

            if (!(left is NameExpressionNode<T> name)) throw new ParseException($"Unable to parse '{token.Text}' {left}");
            switch (args.Count)
            {
                case 0:
                    var constant = parser.Grammar.GetConstant(name.Name, true);
                    return new CallExpressionNode<T>((v) => constant());
                case 1:
                    var function1 = parser.Grammar.GetFunction1(name.Name, true);
                    return new CallExpressionNode<T>((v) => function1(args[0].Evaluate(v)));
                case 2:
                    var function2 = parser.Grammar.GetFunction2(name.Name, true);
                    return new CallExpressionNode<T>((v) => function2(args[0].Evaluate(v), args[1].Evaluate(v)));
                default:
                    var functionN = parser.Grammar.GetFunctionN(name.Name, true);
                    return new CallExpressionNode<T>((v) =>
                    {
                        var argsEvaluated = new T[args.Count];
                        for (int i = 0; i < args.Count; i++)
                        {
                            argsEvaluated[i] =args[i].Evaluate(v);
                        }
                        return functionN(argsEvaluated.ToArray());
                    });
            }
        }
    }
}