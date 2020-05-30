using System;
using System.Collections.Generic;
using System.Text;
using BenchmarkDotNet.Attributes;
using Math = MyClassLibrary.Math.Math;

namespace MyClassLibraryBenchmark
{
    public class GCDBenchmark
    {

        [Params(3975, 39578, 690495, 1948473)]
        public int A { get; set; }

        [Params(5933, 27468, 395063, 3859375)]
        public int B { get; set; }

        [Benchmark]
        public int TestBinaryGCD()
        {
            return Math.BinaryGCD(A, B);
        }

        [Benchmark]
        public int TestNormalGCD()
        {
            return Math.GCD(A, B);
        }
    }
}
