using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RedOwl.Engine
{
    public interface IExpressionNode<T> where T : IConvertible
    {
        T Evaluate(IDictionary<string, T> variables);
    }

    public class NullExpressionNode<T> : IExpressionNode<T> where T : IConvertible
    {
        public T Evaluate(IDictionary<string, T> variables) => default;
    }

    public class NameExpressionNode<T> : IExpressionNode<T> where T : IConvertible
    {
        public string Name { get; }
        public NameExpressionNode(string name)
        {
            Name = name;
        }

        public T Evaluate(IDictionary<string, T> variables) => 
            variables.TryGetValue(Name, out var value) ? value : default;
    }

    public class NumberExpressionNode<T> : IExpressionNode<T> where T : IConvertible
    {
        public T Number { get; }
        
        public NumberExpressionNode(string name)
        {
            Number = (T)Convert.ChangeType(name, typeof(T));
        }

        public T Evaluate(IDictionary<string, T> variables)
        {
            return Number;
        }
    }

    public class AssignExpressionNode<T> : IExpressionNode<T> where T : IConvertible
    {
        private string Name { get; }
        private IExpressionNode<T> Right { get; }
        
        public AssignExpressionNode(string name, IExpressionNode<T> right)
        {
            Name = name;
            Right = right;
        }

        public T Evaluate(IDictionary<string, T> variables)
        {
            return variables[Name] = Right.Evaluate(variables);
        }
    }

    public class PreIncrementExpressionNode<T> : IExpressionNode<T> where T : IConvertible
    {
        private Func<T, T> Operator { get; }
        private NameExpressionNode<T> Right { get; }
        
        public PreIncrementExpressionNode(Func<T, T> @operator, NameExpressionNode<T> right)
        {
            Operator = @operator;
            Right = right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Evaluate(IDictionary<string, T> variables) => 
            variables[Right.Name] = Operator(Right.Evaluate(variables));
    }
    
    public class PostIncrementExpressionNode<T> : IExpressionNode<T> where T : IConvertible
    {
        private Func<T, T> Operator { get; }
        private NameExpressionNode<T> Left { get; }
        
        public PostIncrementExpressionNode(Func<T, T> @operator, NameExpressionNode<T> left)
        {
            Operator = @operator;
            Left = left;
        }

        public T Evaluate(IDictionary<string, T> variables)
        {
            var current = variables[Left.Name];
            variables[Left.Name] = Operator(Left.Evaluate(variables));
            return current;
        }
    }

    public class PrefixExpressionNode<T> : IExpressionNode<T> where T : IConvertible
    {
        private Func<T, T> Operator { get; }
        private IExpressionNode<T> Right { get; }

        public PrefixExpressionNode(Func<T, T> @operator, IExpressionNode<T> right)
        {
            Operator = @operator;
            Right = right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Evaluate(IDictionary<string, T> variables) => 
            Operator(Right.Evaluate(variables));
    }
    
    public class PostfixExpressionNode<T> : IExpressionNode<T> where T : IConvertible
    {
        private IExpressionNode<T> Left { get; }
        private Func<T, T> Operator { get; }
        
        public PostfixExpressionNode(IExpressionNode<T> left, Func<T, T> @operator)
        {
            Left = left;
            Operator = @operator;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Evaluate(IDictionary<string, T> variables) => 
            Operator(Left.Evaluate(variables));
    }

    public class OperatorExpressionNode<T> : IExpressionNode<T> where T : IConvertible
    {
        private IExpressionNode<T> Left { get; }
        private Func<T, T, T> Operator { get; }
        private IExpressionNode<T> Right { get; }
        public OperatorExpressionNode(IExpressionNode<T> left, Func<T, T, T> @operator, IExpressionNode<T> right)
        {
            Left = left;
            Operator = @operator;
            Right = right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Evaluate(IDictionary<string, T> variables) => 
            Operator(Left.Evaluate(variables), Right.Evaluate(variables));
    }
    
    public class ConditionalExpressionNode<T> : IExpressionNode<T> where T : IConvertible
    {
        private IExpressionNode<T> Condition { get; }
        private IExpressionNode<T> ThenArm { get; }
        private IExpressionNode<T> ElseArm { get; }
        public ConditionalExpressionNode(IExpressionNode<T> condition, IExpressionNode<T> thenArm, IExpressionNode<T> elseArm)
        {
            Condition = condition;
            ThenArm = thenArm;
            ElseArm = elseArm;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Evaluate(IDictionary<string, T> variables) => 
            Convert.ToBoolean(Condition.Evaluate(variables)) ? ThenArm.Evaluate(variables) : ElseArm.Evaluate(variables);
    }

    public class CallExpressionNode<T> : IExpressionNode<T> where T : IConvertible
    {
        private Func<IDictionary<string, T>, T> Function { get; }

        public CallExpressionNode(Func<IDictionary<string, T>, T> function)
        {
            Function = function;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Evaluate(IDictionary<string, T> variables) => 
            Function(variables);
    }
}