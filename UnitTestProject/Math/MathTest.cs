using System.Linq;
using Xunit;
using static MyClassLibrary.Math.Math;

namespace UnitTestProject.Math
{
    public class MathTest
    {
        [Theory]
        [InlineData(2, 8, 12, 20, 50)]
        [InlineData(1, 9, 43, 76, 87)]
        [InlineData(4, 1232, 2804, 9380)]
        public void TestGCG(params int[] numbers)
        {
            var result = numbers.First();
            var list = numbers.ToList();
            list.RemoveAt(0);

            var input = list.ToArray();

            Assert.Equal(result, GCD(input));
        }

        [Theory]
        [InlineData(50, 23, 1, 6, -13)]
        public void TestExtendedEuclidean(int a, int b, int gcd, int s, int t)
        {
            // ReSharper disable once UseDeconstruction
            var result = ExtendedEuclidean(a, b);
            Assert.Equal(gcd, result.gcd);
            Assert.Equal(s, result.s);
            Assert.Equal(t, result.t);
        }

        [Fact]
        public void TestSqrt()
        {
            Assert.Equal(4.0f, Sqrt(16.0f), 1);
        }

        [Theory]
        [InlineData(3, 6, 6)]
        [InlineData(5, 3, 15)]
        [InlineData(5, 24, 5 * 24)]
        [InlineData(5, 10, 10)]
        public void TestLCM(int a, int b, int result)
        {
            Assert.Equal(result, LCM(a, b));
        }
    }
}
