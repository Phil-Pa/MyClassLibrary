using System;
using System.Collections.Generic;
using System.Text;
using MyClassLibrary.Math;
using Xunit;

namespace UnitTestProject.Math
{
	public class FractionTest
	{

		[Fact]
		public void TestEmptyFraction()
		{
			Fraction fraction = new Fraction();
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
			Fraction fraction = new Fraction(3, 5);

			Assert.Equal(3f / 5f, fraction.ToFloat());
		}

		[Fact]
		public void TestFractionEqual()
		{
			Fraction fraction1 = new Fraction(3, 5);
			Fraction fraction2 = new Fraction(3, 5);

			Fraction fraction3 = new Fraction(3, 6);
			Fraction fraction4 = new Fraction(-6, -10);

			Assert.True(fraction1 == fraction2);
			Assert.True(fraction1 != fraction3);
			Assert.True(fraction1 == fraction4);

			Assert.True(Equals(fraction1, fraction2));
		}

		[Fact]
		public void TestFractionLessAndGreater()
		{
			Fraction fraction1 = new Fraction(3, 5);
			Fraction fraction3 = new Fraction(3, 6);
			Fraction fraction4 = new Fraction(-6, 10);

			Assert.True(fraction1 > fraction3);
			Assert.True(fraction1 > fraction4);
		}

		[Fact]
		public void TestHashCode()
		{
			Fraction fraction1 = new Fraction(3, 5);
			Fraction fraction3 = new Fraction(3, 6);

			Assert.False(fraction3.GetHashCode() == fraction1.GetHashCode());
		}
	}
}
