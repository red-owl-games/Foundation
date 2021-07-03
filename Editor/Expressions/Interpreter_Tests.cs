using System.Collections.Generic;
using NUnit.Framework;
using RedOwl.Engine;
using Unity.Mathematics;

public class Interpreter_Tests
{
    [Test]
    public void Exception_Test()
    {
        var i = new MathInterpreter();

        Assert.Throws<ParseException>(() => i.Compile("(i + 2"));
        Assert.Throws<ParseException>(() => i.Eval("1 = 2"));
        // Doesn't work because its treated as a variable and returns the default if not found in variables
        //Assert.Throws<ParseException>(() => i.Eval("missingConstant"));
        Assert.Throws<ParseException>(() => i.Eval("missingConstant()"));
        Assert.Throws<ParseException>(() => i.Eval("missingFunc1(1)"));
        Assert.Throws<ParseException>(() => i.Eval("missingFunc2(1, 2)"));
        Assert.Throws<ParseException>(() => i.Eval("missingFuncN(1, 2, 3, 4"));
    }
    
    [Test]
    public void Number_Test()
    {
        var i = new MathInterpreter();

        Assert.AreEqual(11, i.Eval("11"));
        Assert.AreEqual(1.1, i.Eval("1.1"));
        Assert.AreEqual(0.1, i.Eval("0.1"));
        Assert.AreEqual(.1, i.Eval(".1"));
    }
    
    [Test]
    public void Assignment_Test()
    {
        var i = new MathInterpreter();

        Assert.AreEqual(5, i.Eval("a = 5"));
        Assert.AreEqual(7, i.Eval("a + 2"));
        Assert.AreEqual(5, i.Get("a"));
    }
    
    [Test]
    public void Equality_Test()
    {
        var i = new MathInterpreter();
        i.Variables["a"] = 0;
        i.Variables["b"] = 1;

        Assert.AreEqual(1, i.Eval("(5 == 5) ? 1 : 2"));
        Assert.AreEqual(2, i.Eval("(5 != 5) ? 1 : 2"));
        Assert.AreEqual(1, i.Eval("(4 < 5) ? 1 : 2"));
        Assert.AreEqual(2, i.Eval("(6 < 5) ? 1 : 2"));
        Assert.AreEqual(1, i.Eval("(5 <= 5) ? 1 : 2"));
        Assert.AreEqual(2, i.Eval("(6 <= 5) ? 1 : 2"));
        Assert.AreEqual(1, i.Eval("(6 > 5) ? 1 : 2"));
        Assert.AreEqual(2, i.Eval("(4 > 5) ? 1 : 2"));
        Assert.AreEqual(1, i.Eval("(5 >= 5) ? 1 : 2"));
        Assert.AreEqual(2, i.Eval("(4 >= 5) ? 1 : 2"));
    }
    
    [Test]
    public void Math_Test()
    {
        var i = new MathInterpreter();
        
        Assert.AreEqual(3 + 4 * 2, i.Eval("3 + 4 * 2"));
        Assert.AreEqual(22 % 3, i.Eval("22 % 3"));
        Assert.AreEqual((5 + 3) * 8 ^ 2 - 5 * -2, i.Eval("(5 + 3) * 8 ^ 2 - 5 * -2"));
        Assert.AreEqual((5 + 3) * 8 ^ 2 - 5 * (3 << 2) + ~5 / 1 & 2, i.Eval("(5 + 3) * 8 ^ 2 - 5 * (3 << 2) + ~5 / 1 & 2"));
    }
    
    [Test]
    public void Bitwise_Test()
    {
        var i = new MathInterpreter();
        Assert.AreEqual(3 << 2, i.Eval("3 << 2"));
        Assert.AreEqual(4 >> 2, i.Eval("4 >> 2"));
        Assert.AreEqual(~5, i.Eval("~5"));
        Assert.AreEqual(1 & 2, i.Eval("1 & 2"));
        Assert.AreEqual(8 ^ 12, i.Eval("8 ^ 12"));
        Assert.AreEqual(10 | 9, i.Eval("10 | 9"));
    }
    
    [Test]
    public void Random_Test()
    {
        var i = new MathInterpreter(1);
        var rng = new Random(1);
        Assert.AreEqual(rng.NextDouble(), i.Eval("random()"));
        Assert.AreEqual(rng.NextDouble(), i.Eval("random()"));
        Assert.AreEqual(rng.NextDouble(), i.Eval("random()"));
        Assert.AreEqual(rng.NextDouble(5), i.Eval("random(5)"));
        Assert.AreEqual(rng.NextDouble(5), i.Eval("random(5)"));
        Assert.AreEqual(rng.NextDouble(5), i.Eval("random(5)"));
        Assert.AreEqual(rng.NextDouble(1,5), i.Eval("random(1, 5)"));
        Assert.AreEqual(rng.NextDouble(1,5), i.Eval("random(1, 5)"));
        Assert.AreEqual(rng.NextDouble(1,5), i.Eval("random(1, 5)"));
    }

    [Test]
    public void IncrementPrefix_Test()
    {
        var i = new MathInterpreter();
        
        var neg1 = -1;
        var zero = 0;
        var one = 1;
        var two = 2;

        i.Variables["neg1"] = -1;
        i.Variables["zero"] = 0;
        i.Variables["one"] = 1;
        i.Variables["two"] = 2;

        Assert.AreEqual(++neg1, i.Eval("++neg1"));
        Assert.AreEqual(++zero, i.Eval("++zero"));
        Assert.AreEqual(++one, i.Eval("++one"));
        Assert.AreEqual(--neg1, i.Eval("--neg1"));
        Assert.AreEqual(--zero, i.Eval("--zero"));
        Assert.AreEqual(--one, i.Eval("--one"));
        
        Assert.AreEqual(++one + two, i.Eval("++one + two"));
        Assert.AreEqual(--one + two, i.Eval("--one + two"));
    }
    
    [Test]
    public void IncrementPostfix_Test()
    {
        var i = new MathInterpreter();
        
        var neg1 = -1;
        var zero = 0;
        var one = 1;
        var two = 2;

        i.Variables["neg1"] = -1;
        i.Variables["zero"] = 0;
        i.Variables["one"] = 1;
        i.Variables["two"] = 2;

        Assert.AreEqual(neg1++, i.Eval("neg1++"));
        Assert.AreEqual(zero++, i.Eval("zero++"));
        Assert.AreEqual(one++, i.Eval("one++"));
        Assert.AreEqual(neg1--, i.Eval("neg1--"));
        Assert.AreEqual(zero--, i.Eval("zero--"));
        Assert.AreEqual(one--, i.Eval("one--"));
        
        Assert.AreEqual(one++ + two, i.Eval("one++ + two"));
        Assert.AreEqual(one-- + two, i.Eval("one-- + two"));
    }

    [Test]
    public void Variable_Test()
    {
        var i = new MathInterpreter();
        var a = 10;
        i.Set("a", 10);
        Assert.AreEqual(++a, i.Eval("++a"));
        a = 20;
        i.Set("a", 20);
        Assert.AreEqual(++a, i.Eval("++a"));
        Assert.AreEqual(a, i.Get("a"));
    }

    [Test]
    public void Compile_Test()
    {
        var i = new MathInterpreter();

        var exp1 = i.Compile("++a");
        var exp2 = i.Compile("--a");
        i.Set("a", 20);
        Assert.AreEqual(21, i.Eval("++a"));
        
        var vars1 = new Dictionary<string, double> {["a"] = 10};
        Assert.AreEqual(11, exp1(vars1));
        Assert.AreEqual(12, exp1(vars1));
        Assert.AreEqual(11, exp2(vars1));
        Assert.AreEqual(10, exp2(vars1));
        Assert.AreEqual(20, i.Eval("--a"));
    }

    public class Switchlock
    {
        public bool val;

        public bool Check()
        {
            val = !val;
            return val;
        }
    }
    
    [Test]
    public void Ternary_Test()
    {
        var i = new MathInterpreter();
        var switchlock = new Switchlock();
        i.AddFunction("checkSwitch", switchlock.Check);

        Assert.AreEqual(5, i.Eval("checkSwitch ? 5 : 6"));
        Assert.AreEqual(6, i.Eval("checkSwitch ? 5 : 6"));
        Assert.AreEqual(5, i.Eval("checkSwitch ? 5 : 6"));
        
        Assert.AreEqual(5, i.Eval("(5 == 5) ? 5 : 6"));
        Assert.AreEqual(6, i.Eval("(5 == 6) ? 5 : 6"));
        Assert.AreEqual(6, i.Eval("0 ? 5 : 6"));
        Assert.AreEqual(5, i.Eval("!0 ? 5 : 6"));
        Assert.AreEqual(5, i.Eval("1 ? 5 : 6"));
        Assert.AreEqual(6, i.Eval("!1 ? 5 : 6"));

        i.Variables["a"] = 0;
        i.Variables["b"] = 1;
        Assert.AreEqual(1, i.Eval("!a"));
        Assert.AreEqual(1, i.Eval("a!"));
        Assert.AreEqual(0, i.Eval("!b"));
        Assert.AreEqual(0, i.Eval("b!"));
        Assert.AreEqual(6, i.Eval("a ? 5 : 6"));
        Assert.AreEqual(5, i.Eval("!a ? 5 : 6"));
        Assert.AreEqual(5, i.Eval("b ? 5 : 6"));
        Assert.AreEqual(6, i.Eval("!b ? 5 : 6"));
    }
    
    // TODO: not implemeneted yet
    // [Test]
    // public void NullCoalescing_Test()
    // {
    //     var i = new MathInterpreter();
    //
    //     Assert.AreEqual(1, i.Eval("a ?? 1"));
    //     i.Variables["a"] = 0;
    //     Assert.AreEqual(0, i.Eval("a ?? 1"));
    // }

    [Test]
    public void Constant_Test()
    {
        var i = new MathInterpreter();
        
        Assert.AreEqual(math.PI_DBL, i.Eval("PI"));
    }
    
    [Test]
    public void Function_Test()
    {
        var i = new MathInterpreter();
        
        Assert.AreEqual(math.LOG2E_DBL, i.Eval("log2(E)"));
    }
    
    [Test]
    public void CustomFunction_Test()
    {
        var i = new MathInterpreter();
        i.AddFunction("sum3", args => args[0] + args[1] + args[2]);
        i.AddFunction("sum5", args => args[0] + args[1] + args[2] + args[3] + args[4]);
        
        Assert.AreEqual(3, i.Eval("sum3(1, 1, 1)"));
        Assert.AreEqual(5, i.Eval("sum5(1, 1, 1, 1, 1)"));
    }

    // // A Test behaves as an ordinary method
    // [Test]
    // public void Interpreter_TestsSimplePasses()
    // {
    //     // Use the Assert class to test conditions
    // }
    //
    // // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // // `yield return null;` to skip a frame.
    // [UnityTest]
    // public IEnumerator Interpreter_TestsWithEnumeratorPasses()
    // {
    //     // Use the Assert class to test conditions.
    //     // Use yield to skip a frame.
    //     yield return null;
    // }
}
