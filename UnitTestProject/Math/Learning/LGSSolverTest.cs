using MyClassLibrary.Math;
using MyClassLibrary.Math.Learning;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace UnitTestProject.Math.Learning
{
    public class LGSSolverTest
    {

        private readonly ITestOutputHelper outputHelper;

        public LGSSolverTest(ITestOutputHelper outputHelper)
        {
            this.outputHelper = outputHelper;
        }

        [Fact]
        public void Test()
        {

            var matrix = new List<List<Fraction>>();

            for (int i = 0; i < 3; i++)
            {
                var list = new List<Fraction>();
                for (int j = 0; j < 4; j++)
                {
                    list.Add(Fraction.Random());
                }
                matrix.Add(list);
            }

            var res = LGSSolver.Solve(matrix);

            outputHelper.WriteLine(res.calculation);
        }

    }
}
