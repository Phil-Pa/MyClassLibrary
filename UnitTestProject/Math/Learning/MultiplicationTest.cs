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
		public void TestSimpleMultiplicationWithSpacesAfterTempResult(int a, int b, int correctResult, string calculationResult)
		{
			var (result, calculation) = Multiplication.DoMultiply(a, b);

			_testOutputHelper.WriteLine(calculation);

			Assert.Equal(correctResult, result);
			Assert.Equal(calculationResult, calculation);
		}

		[Theory]
		[InlineData(321, 98, 31458, " 321*98\n+     8\n+    9 \n+   16 \n+  18  \n+  24  \n+ 27   \nü 111  \n= 31458")]
		public void TestDifficultMultiplication(int a, int b, int correctResult, string calculationResult)
		{
			var (result, calculation) = Multiplication.DoMultiply(a, b);

			_testOutputHelper.WriteLine(calculation);

			Assert.Equal(correctResult, result);
			Assert.Equal(calculationResult, calculation);
		}

	}
}
