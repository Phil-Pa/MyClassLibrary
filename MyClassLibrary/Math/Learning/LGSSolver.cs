using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyClassLibrary.Math.Learning
{
    public static class LGSSolver
    {

        public static (List<Fraction> result, string calculation) Solve(List<List<Fraction>> matrix)
        {
            // input is list of rows

            // a1 x + a2 y + a3 z = d -> matrix[0] -> a1, a2, a3
            // a1 x + a2 y + a3 z = d -> matrix[1] ...
            // a1 x + a2 y + a3 z = d ...

            // TODO: check that all rows have the same length

            var rank = matrix[0].Count;
            var output = new FormattedOutput(16);

            PrintMatrix(matrix, output);

            var temp = matrix[1][0];

            for (var i = 0; i < rank; i++)
                matrix[1][i] = matrix[1][i] * (matrix[0][0] / temp);

            temp = matrix[2][0];
            for (var i = 0; i < rank; i++)
                matrix[2][i] = matrix[2][i] * (matrix[0][0] / temp);

            PrintMatrix(matrix, output);

            for (var i = 0; i < rank; i++)
            {
                matrix[1][i] = matrix[1][i] - matrix[0][i];
                matrix[2][i] = matrix[2][i] - matrix[0][i];
                // 1,2..n
            }

            PrintMatrix(matrix, output);

            temp = matrix[2][1];

            // can skip index [0], since it is 0
            for (var i = 1; i < rank; i++)
                matrix[2][i] = matrix[2][i] * (matrix[1][1] / temp);

            PrintMatrix(matrix, output);

            for (var i = 1; i < rank; i++)
                matrix[2][i] = matrix[2][i] - matrix[1][i];

            PrintMatrix(matrix, output);

            try
            {
                var z = matrix[2][3] / matrix[2][2];
                var y = (matrix[1][3] / matrix[1][1]) - ((matrix[1][2] / matrix[1][1]) * z);
                var x = (matrix[0][3] - (matrix[0][1] * y) - (matrix[0][2] * z)) / matrix[0][0];

                return (new List<Fraction> { x, y, z }, output.ToString());
            }
            catch
            {
                return (new List<Fraction>(), output.ToString());
            }
        }

        private static void PrintMatrix(List<List<Fraction>> matrix, FormattedOutput output)
        {
            for (var i = 0; i < matrix.Count; i++)
            {
                output.AddRow(matrix[i].Select(fraction => fraction.ToString()).ToArray());
            }

            output.AddEmptyRow();
        }
    }
}
