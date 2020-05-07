using MyClassLibrary.Math;
using System;
using Xunit;

namespace UnitTestProject.Math
{
	public class FractionTest
	{
		[Fact]
		public void TestEmptyFraction()
		{
			var fraction = new Fraction();
			Assert.Equal(0, fraction.Numerator);
			Assert.Equal(0, fraction.Denominator);
		}

		[Fact]
		public void TestInvalidFraction()
		{
			Assert.Throws<ArgumentException>(() => new Fraction(1, 0));
		}

		[Fact]
		public void TestFractionToFloat()
		{
			var fraction = new Fraction(3, 5);

			Assert.Equal(3f / 5f, fraction.ToFloat());
		}

		[Fact]
		public void TestFractionEqual()
		{
			var fraction1 = new Fraction(3, 5);
			var fraction2 = new Fraction(3, 5);

			var fraction3 = new Fraction(3, 6);
			var fraction4 = new Fraction(-6, -10);

			Assert.True(fraction1 == fraction2);
			Assert.True(fraction1 != fraction3);
			Assert.True(fraction1 == fraction4);

			Assert.True(Equals(fraction1, fraction2));
		}

		[Fact]
		public void TestFractionLessAndGreater()
		{
			var fraction1 = new Fraction(3, 5);
			var fraction3 = new Fraction(3, 6);
			var fraction4 = new Fraction(-6, 10);

			Assert.True(fraction1 > fraction3);
			Assert.True(fraction1 > fraction4);
		}

		[Fact]
		public void TestHashCode()
		{
			var fraction1 = new Fraction(3, 5);
			var fraction3 = new Fraction(3, 6);

			Assert.False(fraction3.GetHashCode() == fraction1.GetHashCode());
		}

        [Fact]
        public void TestSimplifyForEquals()
        {
            var fraction = new Fraction(13, 39);
            Assert.True(new Fraction(1, 3) == fraction);

            fraction = new Fraction(14, 2);
            Assert.True(new Fraction(7, 1) == fraction);

            fraction = new Fraction(14, -2);
            Assert.True(new Fraction(7, -1) == fraction);

            fraction = new Fraction(14, -2);
            Assert.True(new Fraction(-7, 1) == fraction);

            fraction = new Fraction(-14, -2);
            Assert.True(new Fraction(7, 1) == fraction);

            fraction = new Fraction(-14, 2);
            Assert.True(new Fraction(7, -1) == fraction);
        }

        [Theory]
        [InlineData(-3, 7, 11, 6, 59, 42)]
        [InlineData(-34, 19, 16, 317, -10474, 6023)]
        public void TestAdd(int n1, int d1, int n2, int d2, int n3, int d3)
        {
            Assert.Equal(new Fraction(n3, d3), new Fraction(n1, d1) + new Fraction(n2, d2));
        }

        [Theory]
        [InlineData(57, 32, 126, 31, 3591, 496)]
        [InlineData(3461, 321, 4319, 3765, 14948059, 1208565)]
        [InlineData(-34, 17, 27, 62, -27, 31)]
        [InlineData(-34, 17, -27, 62, 27, 31)]
        public void TestMultiply(int n1, int d1, int n2, int d2, int n3, int d3)
        {
            Assert.Equal(new Fraction(n3, d3), new Fraction(n1, d1) * new Fraction(n2, d2));
        }

        [Theory]
        [InlineData(4376, 436, 463, 245, 217563, 26705)]
        [InlineData(-736, 92, 725, 186, -2213, 186)]
        [InlineData(-164, 436, -469, 195, 43126, 21255)]
        public void TestSubtract(int n1, int d1, int n2, int d2, int n3, int d3)
        {
            Assert.Equal(new Fraction(n3, d3), new Fraction(n1, d1) - new Fraction(n2, d2));
        }

        [Theory]
        [InlineData(316, 164, 195, 32, 2528, 7995)]
        [InlineData(-46, 316, 467, 7316, -84134, 36893)]
        [InlineData(-431, 496, -4976, 1257, 541767, 2468096)]
        public void TestDivide(int n1, int d1, int n2, int d2, int n3, int d3)
        {
            Assert.Equal(new Fraction(n3, d3), new Fraction(n1, d1) / new Fraction(n2, d2));
        }
    }
}