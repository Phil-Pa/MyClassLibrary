using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MyClassLibrary.Learning
{


    // (ExtendedEuclideanResult result, string calculation)
    public static class ExtendedEuclidean
    {

        public static (int gcd, string calculation) Calculate(int a, int b)
        {
            // assume a > b
            Debug.Assert(a > b);

            // TODO:
            var gcd = Math.Math.GCD(a, b);

            var aStr = a.ToString();
            var bStr = b.ToString();

            var output = new FormattedOutput(aStr.Length + 4);
            output.AddRow(string.Empty, aStr, bStr);
            output.AddRow(aStr, "1", "0");

            var s = 0;
            var t = 0;

            var quotient = System.Math.DivRem(a, b, out var remainder);
            output.AddRow(bStr, "0", "1", quotient.ToString());

            a = b;
            b = remainder;

            while (true)
            {
                quotient = System.Math.DivRem(a, b, out var rem);

                output.AddRow(b.ToString(), s.ToString(), t.ToString(), quotient.ToString());

                a = b;
                b = rem;

                if (rem == 0)
                    break;
            }

            return (gcd, output.ToString());
        }

    }
}
