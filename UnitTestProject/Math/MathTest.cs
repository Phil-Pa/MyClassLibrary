using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace UnitTestProject.Math
{
    public class MathTest
    {

        [Fact]
        public void TestGDC()
        {
            Assert.Equal(2, MyClassLibrary.Math.Math.GCD(8, 12, 20, 50));
            Assert.Equal(1, MyClassLibrary.Math.Math.GCD(9, 43, 76, 87)); 
            Assert.Equal(4, MyClassLibrary.Math.Math.GCD(1232, 2804, 9380, 8300, 9256));
        }

        [Fact]
        public void TestSqrt()
        {
            Assert.Equal(4.0f, MyClassLibrary.Math.Math.Sqrt(16.0f), 1);
        }
    }
}
