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
        public void TestFixedMatrix()
        {

            var matrix = new List<List<Fraction>>
            {
                new List<Fraction>
                {
                    Fraction.Of(2), Fraction.Of(3), Fraction.Of(2), Fraction.Of(-4)
                },
                new List<Fraction>
                {
                    Fraction.Of(-2), Fraction.Of(-2), Fraction.Of(2), Fraction.Of(1)
                },
                new List<Fraction>
                {
                    Fraction.Of(2), Fraction.Of(-4), Fraction.Of(-1), Fraction.Of(4)
                }
            };

            var (result, calculation) = LGSSolver.Solve(matrix);

            Assert.Equal(result[0], new Fraction(-1, 10));
            Assert.Equal(result[1], new Fraction(-23, 25));
            Assert.Equal(result[2], new Fraction(-13, 25));

            outputHelper.WriteLine(calculation);
            outputHelper.WriteLine(string.Join('\t', result));
        }

        [Fact]
        public void TestFixedMatrix2()
        {

            var matrix = new List<List<Fraction>>
            {
                new List<Fraction>
                {
                    Fraction.Of(2), Fraction.Of(7), Fraction.Of(-1), Fraction.Of(13)
                },
                new List<Fraction>
                {
                    Fraction.Of(17), Fraction.Of(-3), Fraction.Of(4), Fraction.Of(-9)
                },
                new List<Fraction>
                {
                    Fraction.Of(3), Fraction.Of(-2), Fraction.Of(1), Fraction.Of(-5)
                }
            };

            var (result, calculation) = LGSSolver.Solve(matrix);

            //Assert.Equal(result[0], new Fraction(-1, 10));
            //Assert.Equal(result[1], new Fraction(-23, 25));
            //Assert.Equal(result[2], new Fraction(-13, 25));

            outputHelper.WriteLine(calculation);
            outputHelper.WriteLine(string.Join('\t', result));
        }

        [Fact]
        public void TestFixedMatrix3()
        {

            var matrix = new List<List<Fraction>>
            {
                new List<Fraction>
                {
                    Fraction.Of(2), Fraction.Of(-2), Fraction.Of(3), Fraction.Of(2)
                },
                new List<Fraction>
                {
                    Fraction.Of(-6), Fraction.Of(6), Fraction.Of(-3), Fraction.Of(7)
                },
                new List<Fraction>
                {
                    Fraction.Of(-7), Fraction.Of(-7), Fraction.Of(4), Fraction.Of(-2)
                }
            };

            var (result, calculation) = LGSSolver.Solve(matrix);

            //Assert.Equal(result[0], new Fraction(-1, 10));
            //Assert.Equal(result[1], new Fraction(-23, 25));
            //Assert.Equal(result[2], new Fraction(-13, 25));

            outputHelper.WriteLine(calculation);
            outputHelper.WriteLine(string.Join('\t', result));
        }

        [Fact]
        public void TestRandomMatrix()
        {

            var matrix = new List<List<Fraction>>();

            for (var i = 0; i < 3; i++)
            {
                var list = new List<Fraction>();
                for (var j = 0; j < 4; j++)
                {
                    list.Add(Fraction.RandomInteger(1, 9));
                }
                matrix.Add(list);
            }



            var (result, calculation) = LGSSolver.Solve(matrix);

            outputHelper.WriteLine(calculation);
            outputHelper.WriteLine(string.Join('\t', result));
        }
    }
}
