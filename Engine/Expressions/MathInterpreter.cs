using System;
using System.Linq;
using Unity.Mathematics;
using Random = Unity.Mathematics.Random;

namespace RedOwl.Engine
{
    // https://en.wikipedia.org/wiki/Shunting-yard_algorithm
    // https://www.codeproject.com/Tips/351042/Shunting-Yard-algorithm-in-Csharp
    // https://rosettacode.org/wiki/Parsing/Shunting-yard_algorithm#C.23
    // https://github.com/munificent/bantam/tree/master/src/com/stuffwithstuff/bantam
    // https://wiki.unity3d.com/index.php/ExpressionParser
    // https://github.com/bijington/expressive
    public class MathInterpreter : Interpreter<double>
    {
        public MathInterpreter(uint? randomSeed = null)
        {
            AddCustomParselets();
            AddPrefixParselets();
            AddPostfixParselets();
            AddInfixParselets();
            AddDualCharacterMethods();
            AddConstantMethods();
            AddMathMethods();
            AddRandomMethods(randomSeed);
        }

        private void AddCustomParselets()
        {
            AddParselet(TokenTypes.NAME, new NameParselet());
            AddParselet(TokenTypes.NUMBER, new NumberParselet());
            AddParselet(TokenTypes.PERIOD, new NumberParselet());
            AddParselet(TokenTypes.PLUS, new IncrementParselet(Precedences.UNARY));
            AddParselet(TokenTypes.MINUS, new IncrementParselet(Precedences.UNARY));
            AddParselet(TokenTypes.EQUALS, new EqualsParselet());
            AddParselet(TokenTypes.BANG, new BangParselet());
            AddParselet(TokenTypes.GREATER_THEN, new GreaterThanParselet());
            AddParselet(TokenTypes.LESS_THEN, new LessThanParselet());
            AddParselet(TokenTypes.QUESTION, new ConditionalParselet());
            //AddParselet(TokenTypes.QUESTION, new QuestionMarkParselet()); // Not Implemented Yet
            AddParselet(TokenTypes.LEFT_PAREN, new GroupParselet());
            AddParselet(TokenTypes.LEFT_PAREN, new CallParselet());
        }

        private void AddPrefixParselets()
        {
            Prefix(TokenTypes.TILDE, (v) => ~(int) v, Precedences.UNARY);
            Prefix(TokenTypes.BANG, (v) => Convert.ToDouble(!Convert.ToBoolean(v)), Precedences.UNARY);
        }

        private void AddPostfixParselets()
        {
            //Postfix(TokenTypes.BANG, (v) => Convert.ToDouble(!Convert.ToBoolean(v)), Precedences.UNARY);
        }

        private void AddInfixParselets()
        {
            InfixLeft(TokenTypes.PLUS, (lhs, rhs) => lhs + rhs, Precedences.SUM);
            InfixLeft(TokenTypes.MINUS, (lhs, rhs) => lhs - rhs, Precedences.SUM);
            InfixLeft(TokenTypes.ASTERISK, (lhs, rhs) => lhs * rhs, Precedences.PRODUCT);
            InfixLeft(TokenTypes.SLASH, (lhs, rhs) => lhs / rhs, Precedences.PRODUCT);
            InfixLeft(TokenTypes.PERCENT, (lhs, rhs) => lhs % rhs, Precedences.PRODUCT);
            InfixLeft(TokenTypes.AMPERSAND, (lhs, rhs) => (int) lhs & (int) rhs, Precedences.BITWISE_AND);
            InfixLeft(TokenTypes.PIPE, (lhs, rhs) => (int) lhs | (int) rhs, Precedences.BITWISE_OR);
            InfixRight(TokenTypes.CARET, (lhs, rhs) => (int) lhs ^ (int) rhs, Precedences.BITWISE_XOR);
        }

        private void AddDualCharacterMethods()
        {
            AddFunction("++", (v) => v + 1);
            AddFunction("--", (v) => v - 1);
            AddFunction("==", (lhs, rhs) => math.abs(lhs - rhs) < math.EPSILON_DBL ? 1 : 0);
            AddFunction("!=", (lhs, rhs) => math.abs(lhs - rhs) > math.EPSILON_DBL ? 1 : 0);
            AddFunction(">", (lhs, rhs) => lhs > rhs ? 1 : 0);
            AddFunction(">=", (lhs, rhs) => lhs >= rhs ? 1 : 0);
            AddFunction("<", (lhs, rhs) => lhs < rhs ? 1 : 0);
            AddFunction("<=", (lhs, rhs) => lhs <= rhs ? 1 : 0);
            AddFunction("<<", (lhs, rhs) => (int) lhs << (int) rhs);
            AddFunction(">>", (lhs, rhs) => (int) lhs >> (int) rhs);
        }
        
        private void AddConstantMethods()
        {
            AddFunction("E", () => math.E_DBL);
            AddFunction("LOG2E", () => math.LOG2E_DBL);
            AddFunction("LOG10E", () => math.LOG10E_DBL);
            AddFunction("LOG2", () => math.LN2_DBL);
            AddFunction("LOG10", () => math.LN10_DBL);
            AddFunction("PI", () => math.PI_DBL);
            AddFunction("SQRT2", () => math.SQRT2_DBL);
            AddFunction("EPSILON", () => math.EPSILON_DBL);
            AddFunction("INF", () => math.INFINITY_DBL);
            AddFunction("NAN", () => math.NAN_DBL);
        }

        private void AddMathMethods()
        {
            AddFunction("abs", math.abs);
            AddFunction("acos", math.acos);
            AddFunction("asin", math.asin);
            AddFunction("atan", math.atan);
            AddFunction("atan2", math.atan2);
            AddFunction("ceil", math.ceil);
            AddFunction("cos", math.cos);
            AddFunction("count", values => values.Length);
            AddFunction("exp", math.exp);
            AddFunction("exp2", math.exp2);
            AddFunction("exp10", math.exp10);
            AddFunction("floor", math.floor);
            AddFunction("frac", math.frac);
            AddFunction("log", math.log);
            AddFunction("log2", math.log2);
            AddFunction("log10", math.log10);
            AddFunction("pow", math.pow);
            AddFunction("reciprocal", math.rcp);
            AddFunction("round", math.round);
            AddFunction("rsqrt", math.rsqrt);
            AddFunction("saturate", math.saturate);
            AddFunction("sign", math.sign);
            AddFunction("sin", math.sin);
            AddFunction("sqrt", math.sqrt);
            AddFunction("step", math.step);
            AddFunction("sum", values => values.Sum());
            AddFunction("tan", math.tan);
            AddFunction("trunc", math.trunc);
            
            AddFunction("radians", math.radians);
            AddFunction("degrees", math.degrees);

            AddFunction("min", math.min);
            AddFunction("max", math.max);

            AddFunction("isfinite", math.isfinite);
            AddFunction("isinf", math.isinf);
            AddFunction("isinfinite", math.isinf);
            AddFunction("isnan", math.isnan);
        }

        private void AddRandomMethods(uint? randomSeed)
        {
            var rng = new Random(randomSeed ?? (uint) Environment.TickCount);
            AddFunction("random", () => rng.NextDouble());
            AddFunction("random", (v) => rng.NextDouble(v));
            AddFunction("random", (lhs, rhs) => rng.NextDouble(lhs, rhs));
        }


    }
}