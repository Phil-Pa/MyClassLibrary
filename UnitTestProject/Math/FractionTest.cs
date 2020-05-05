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
			Assert.Throws<ArgumentException>(() => new Fraction(0, 1));
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
	}
}