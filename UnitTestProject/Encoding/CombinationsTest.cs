using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyClassLibrary.Math;
using Xunit;

namespace UnitTestProject.Encoding
{
	public class CombinationsTest
	{

		[Fact]
		public void TestInvalidInput()
		{
			Assert.Throws<ArgumentException>(() => Combinations.GetCombinations(-2));
			Assert.Throws<ArgumentException>(() => Combinations.GetCombinations(0));
		}

		[Fact]
		public void Test1()
		{
			var result = Combinations.GetCombinations(1).ToList();

			Assert.Equal(2, result.Count);
			Assert.Equal(Combinations.Empty, result[0]);
			Assert.Equal("0", result[1]);

			result = Combinations.GetCombinations(2).ToList();

			Assert.Equal(4, result.Count);
			Assert.Equal(Combinations.Empty, result[0]);
			Assert.Equal("0", result[1]);
			Assert.Equal("1", result[2]);
			Assert.Equal("01", result[3]);

			result = Combinations.GetCombinations(3).ToList();

			Assert.Equal(8, result.Count);
			Assert.Equal(Combinations.Empty, result[0]);
			Assert.Equal("0", result[1]);
			Assert.Equal("1", result[2]);
			Assert.Equal("2", result[3]);
			Assert.Equal("01", result[4]);
			Assert.Equal("02", result[5]);
			Assert.Equal("12", result[6]);
			Assert.Equal("012", result[7]);

			result = Combinations.GetCombinations(5).ToList();
			Assert.Equal(32, result.Count);
		}

	}
}
