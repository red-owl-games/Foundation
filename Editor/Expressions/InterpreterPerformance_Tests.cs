//
// using System;
// using NUnit.Framework;
// using RedOwl.Engine;
// using Unity.PerformanceTesting;
//
// public class InterpreterPerformance_Tests
// {
//     [Test, Performance, Version("0.0.1")]
//     public void MemoryPerformance_Test()
//     {
//         var i = new MathInterpreter {Variables = {["a"] = 10, ["b"] = 20, ["d"] = 10}};
//         
//         Measure.Method(() => {i.Eval("sqrt(a / b) * 3 + PI + pow(5, d)");})
//             .WarmupCount(5)
//             .IterationsPerMeasurement(10000)
//             .MeasurementCount(10)
//             .GC()
//             .Run();
//     }
//     
//     [Test, Performance, Version("0.0.1")]
//     public void MemoryPerformanceCompiling_Test()
//     {
//         var i = new MathInterpreter {Variables = {["a"] = 10, ["b"] = 20, ["d"] = 10}};
//
//         Measure.Method(() => {i.Compile("sqrt(a / b) * 3 + PI + pow(5, d)");})
//             .WarmupCount(5)
//             .IterationsPerMeasurement(10000)
//             .MeasurementCount(10)
//             .GC()
//             .Run();
//     }
//     
//     [Test, Performance, Version("0.0.1")]
//     public void MemoryPerformanceCompiled_Test()
//     {
//         var i = new MathInterpreter {Variables = {["a"] = 10, ["b"] = 20, ["d"] = 10}};
//         var v = i.Variables;
//         var f = i.Compile("sqrt(a / b) * 3 + PI + pow(5, d)");
//         
//         Measure.Method(() => {double c = f(v);})
//             .WarmupCount(5)
//             .IterationsPerMeasurement(10000)
//             .MeasurementCount(100)
//             .GC()
//             .Run();
//     }
//     
//     [Test, Performance, Version("0.0.1")]
//     public void MemoryPerformanceBaseline_Test()
//     {
//         double a = 10;
//         double b = 20;
//         double d = 10;
//         Measure.Method(() => {double c = Math.Sqrt(a / b) * 3 + Math.PI + Math.Pow(5, d);})
//             .WarmupCount(5)
//             .IterationsPerMeasurement(10000)
//             .MeasurementCount(100)
//             .GC()
//             .Run();
//     }
// }
