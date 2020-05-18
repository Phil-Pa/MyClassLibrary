using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace UnitTestProject.Learning
{
    public class ExtendedEuclideanTest
    {

        private readonly ITestOutputHelper outputHelper;

        public ExtendedEuclideanTest(ITestOutputHelper outputHelper)
        {
            this.outputHelper = outputHelper;
        }


        [Fact]
        public void Test()
        {

            var (gcd, calculation) = MyClassLibrary.Learning.ExtendedEuclidean.Calculate(4325, 654);
            Assert.Equal(1, gcd);

            outputHelper.WriteLine(calculation);
        }

    }
}
