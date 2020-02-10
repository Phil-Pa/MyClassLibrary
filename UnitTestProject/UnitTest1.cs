using System;
using MyClassLibrary;
using MyClassLibrary.Math;
using MyClassLibrary.Math.Learning;
using Xunit;
using Xunit.Abstractions;

namespace UnitTestProject
{
	public class UnitTest1
	{
		private readonly ITestOutputHelper _testOutputHelper;

		public UnitTest1(ITestOutputHelper testOutputHelper)
		{
			_testOutputHelper = testOutputHelper;
		}

		[Fact]
		public void TestMethod1()
		{

			var result = Multiplication.DoMultiply(99999, 9999);

			_testOutputHelper.WriteLine(result.calculation);

			

		}
	}
}
