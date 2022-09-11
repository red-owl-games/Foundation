using System;
using System.Collections.Generic;
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
            AddConversions();
            AddEasingMethods();
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
            AddFunction("+=", (lhs, rhs) => lhs + rhs);
            AddFunction("--", (v) => v - 1);
            AddFunction("-=", (lhs, rhs) => lhs - rhs);
            AddFunction("*=", (lhs, rhs) => lhs * rhs);
            AddFunction("/=", (lhs, rhs) => lhs / rhs);
            AddFunction("==", (lhs, rhs) => math.abs(lhs - rhs) < math.EPSILON_DBL ? 1 : 0);
            AddFunction("!=", (lhs, rhs) => math.abs(lhs - rhs) > math.EPSILON_DBL ? 1 : 0);
            AddFunction(">", (lhs, rhs) => lhs > rhs ? 1 : 0);
            AddFunction(">=", (lhs, rhs) => lhs >= rhs ? 1 : 0);
            AddFunction("<", (lhs, rhs) => lhs < rhs ? 1 : 0);
            AddFunction("<=", (lhs, rhs) => lhs <= rhs ? 1 : 0);
            AddFunction("<<", (lhs, rhs) => (int) lhs << (int) rhs);
            AddFunction(">>", (lhs, rhs) => (int) lhs >> (int) rhs);
            AddFunction("??", (lhs, rhs) => lhs == 0 ? lhs : rhs);
        }
        
        private void AddConstantMethods()
        {
            AddFunction("math.E", () => math.E_DBL);
            AddFunction("math.LOG2E", () => math.LOG2E_DBL);
            AddFunction("math.LOG10E", () => math.LOG10E_DBL);
            AddFunction("math.LOG2", () => math.LN2_DBL);
            AddFunction("math.LOG10", () => math.LN10_DBL);
            AddFunction("math.PI", () => math.PI_DBL);
            AddFunction("math.SQRT2", () => math.SQRT2_DBL);
            AddFunction("math.EPSILON", () => math.EPSILON_DBL);
            AddFunction("math.INF", () => math.INFINITY_DBL);
            AddFunction("math.NAN", () => math.NAN_DBL);
        }

        private void AddMathMethods()
        {
            AddFunction("math.abs", math.abs);
            AddFunction("math.acos", math.acos);
            AddFunction("math.asin", math.asin);
            AddFunction("math.atan", math.atan);
            AddFunction("math.atan2", math.atan2);
            AddFunction("math.ceil", math.ceil);
            AddFunction("math.cos", math.cos);
            AddFunction("math.count", values => values.Length);
            AddFunction("math.exp", math.exp);
            AddFunction("math.exp2", math.exp2);
            AddFunction("math.exp10", math.exp10);
            AddFunction("math.floor", math.floor);
            AddFunction("math.frac", math.frac);
            AddFunction("math.log", math.log);
            AddFunction("math.log2", math.log2);
            AddFunction("math.log10", math.log10);
            AddFunction("math.pow", math.pow);
            AddFunction("math.reciprocal", math.rcp);
            AddFunction("math.round", math.round);
            AddFunction("math.rsqrt", math.rsqrt);
            AddFunction("math.saturate", math.saturate);
            AddFunction("math.sign", math.sign);
            AddFunction("math.sin", math.sin);
            AddFunction("math.sqrt", math.sqrt);
            AddFunction("math.step", math.step);
            AddFunction("math.sum", values => values.Sum());
            AddFunction("math.tan", math.tan);
            AddFunction("math.trunc", math.trunc);

            AddFunction("math.min", math.min);
            AddFunction("math.max", math.max);

            AddFunction("math.isfinite", math.isfinite);
            AddFunction("math.isinf", math.isinf);
            AddFunction("math.isinfinite", math.isinf);
            AddFunction("math.isnan", math.isnan);
        }

        private void AddConversions()
        {
            AddFunction("math.radians", math.radians);
            AddFunction("math.degrees", math.degrees);

            AddFunction("math.ToCelsius", (fahrenheit) => (fahrenheit - 32f) * .5556);
            AddFunction("math.ToFahrenheit", (celsius) => celsius * 1.8 + 32f);
            
            AddFunction("math.remap", (x, low1, high1, low2, high2) => low2 + (x - low1) * (high2 - low2) / (high1 - low1));
        }

        private void AddEasingMethods()
        {
            AddFunction("ease.InSine", (x, value) => value * (1 - math.cos(x * math.PI / 2)));
            AddFunction("ease.OutSine", (x, value) => value * math.sin(x * math.PI / 2));
            AddFunction("ease.InOutSine", (x, value) => value * (-(math.cos(math.PI * x) - 1) / 2));
            
            AddFunction("ease.InQuad", (x, value) => value * math.pow(x, 2));
            AddFunction("ease.OutQuad", (x, value) => value * (1 - math.pow(1 - x, 2)));
            AddFunction("ease.InOutQuad", (x, value) => value * (x < 0.5 ? 2 * x * x : 1 - math.pow(-2 * x + 2, 2) / 2));
            
            AddFunction("ease.InCubic", (x, value) => value * math.pow(x, 3));
            AddFunction("ease.OutCubic", (x, value) => value * (1 - math.pow(1 - x, 3)));
            AddFunction("ease.InOutCubic", (x, value) => value * (x < 0.5 ? 4 * x * x * x : 1 - math.pow(-2 * x + 2, 3) / 2));
            
            AddFunction("ease.InQuart", (x, value) => value * math.pow(x, 4));
            AddFunction("ease.OutQuart", (x, value) => value * (1 - math.pow(1 - x, 4)));
            AddFunction("ease.InOutQuart", (x, value) => value * (x < 0.5 ? 8 * x * x * x * x : 1 - math.pow(-2 * x + 2, 4) / 2));
            
            AddFunction("ease.InQuint", (x, value) => value * math.pow(x, 5));
            AddFunction("ease.OutQuint", (x, value) => value * (1 - math.pow(1 - x, 5)));
            AddFunction("ease.InOutQuint", (x, value) => value * (x < 0.5 ? 16 * x * x * x * x * x : 1 - math.pow(-2 * x + 2, 5) / 2));
            
            AddFunction("ease.InExpo", (x, value) => value * (x == 0 ? 0 : math.pow(2, 10 * x - 10)));
            AddFunction("ease.OutExpo", (x, value) => value * (Math.Abs(x - 1) < math.EPSILON_DBL ? 1 : 1 - math.pow(2, -10 * x)));
            AddFunction("ease.InOutExpo", (x, value) => value * (x == 0 ? 0 : Math.Abs(x - 1) < math.EPSILON_DBL ? 1 : x < 0.5 ? math.pow(2, 20 * x - 10) / 2 : (2 - math.pow(2, -20 * x + 10)) / 2));
            
            AddFunction("ease.InCirc", (x, value) => value * (1 - math.sqrt(1 - math.pow(x, 2))));
            AddFunction("ease.OutCirc", (x, value) => value * math.sqrt(1 - math.pow(x - 1, 2)));
            AddFunction("ease.InOutCirc", (x, value) => value * (x < 0.5 ? (1 - math.sqrt(1 - math.pow(2 * x, 2))) / 2 : (math.sqrt(1 - math.pow(-2 * x + 2, 2)) + 1) / 2));
        }

        private void AddRandomMethods(uint? randomSeed)
        {
            var rng = new Random(randomSeed ?? (uint)DateTimeOffset.Now.ToUnixTimeSeconds());
            AddFunction("math.random", () => rng.NextDouble());
            AddFunction("math.random", (v) => rng.NextDouble(v));
            AddFunction("math.random", (lhs, rhs) => rng.NextDouble(lhs, rhs));
        }

        public static implicit operator MathInterpreter(Dictionary<string, double> variables) =>
            new() { Variables = variables };
    }
}