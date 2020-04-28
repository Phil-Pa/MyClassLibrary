using BenchmarkDotNet.Attributes;
using MyClassLibrary.Math.Learning;

namespace MyClassLibraryBenchmark
{
    public class MultiplyBenchmark
    {
        // , 16, 32, 64, 128, 256, 512, 1024, 2048, 2048 * 2, 2048 * 2 * 2, 2048 * 2 * 2 * 2
        [Params(2, 96, 147, 3795, 99999)]
        public int MulA { get; set; }

        [Params(2, 96, 147, 3795, 99999)]
        public int MulB { get; set; }

        [Benchmark]
        public int TestMultiply()
        {
            return Multiplication.DoMultiply(MulA, MulB).result;
        }
    }
}