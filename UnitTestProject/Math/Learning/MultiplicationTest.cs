using System;
using System.Collections.Generic;
using System.Text;
using MyClassLibrary.Math.Learning;
using Xunit;
using Xunit.Abstractions;

namespace UnitTestProject.Math.Learning
{
	public class MultiplicationTest
	{
		private readonly ITestOutputHelper _testOutputHelper;

		public MultiplicationTest(ITestOutputHelper testOutputHelper)
		{
			_testOutputHelper = testOutputHelper;
		}

		[Fact]
		public void TestThrowIfNegativeInput()
		{
			Assert.Throws<ArgumentException>(() =>
			{
				var (result, calculation) = Multiplication.DoMultiply(-2, 0);
			});
		}

		[Fact]
		public void TestThrowIfOverflow()
		{
			Assert.Throws<ArgumentException>(() =>
			{
				var (result, calculation) = Multiplication.DoMultiply(int.MaxValue / 2, int.MaxValue / 2);
			});
		}

		[Theory]
		[InlineData(0, 0, 0, " 0*0\n+  0\n=  0")]
		[InlineData(2, 2, 4, " 2*2\n+  4\n=  4")]
		[InlineData(4, 4, 16, " 4*4\n+ 16\n= 16")]
		public void TestSimpleMultiplication(int a, int b, int correctResult, string calculationResult)
		{
			var (result, calculation) = Multiplication.DoMultiply(a, b);

			_testOutputHelper.WriteLine(calculation);

			Assert.Equal(correctResult, result);
			Assert.Equal(calculationResult, calculation);
		}

		[Theory]
		[InlineData(23, 6, 138, " 23*6\n+  18\n+ 12 \n= 138")]
		[InlineData(1121, 312, 349752, " 1121*312\n+       2\n+      1 \n+     3  \n+      4 \n+     2  \n+    6   \n+     2  \n+    1   \n+   3    \n+    2   \n+   1    \n+  3     \n=  349752")]
		public void TestSimpleMultiplicationWithSpacesAfterTempResult(int a, int b, int correctResult, string calculationResult)
		{
			var (result, calculation) = Multiplication.DoMultiply(a, b);

			_testOutputHelper.WriteLine(calculation);

			Assert.Equal(correctResult, result);
			Assert.Equal(calculationResult, calculation);
		}

		[Theory]
		[InlineData(321, 98, 31458, " 321*98\n+     8\n+    9 \n+   16 \n+  18  \n+  24  \n+ 27   \nü 111  \n= 31458")]
		[InlineData(28341, 39653, 1123805673, "")]
		public void TestDifficultMultiplication(int a, int b, int correctResult, string calculationResult)
		{
			var (result, calculation) = Multiplication.DoMultiply(a, b);

			_testOutputHelper.WriteLine(calculation);

			if (string.IsNullOrEmpty(calculationResult))
				return;

			Assert.Equal(correctResult, result);
			Assert.Equal(calculationResult, calculation);
		}

	}
}
