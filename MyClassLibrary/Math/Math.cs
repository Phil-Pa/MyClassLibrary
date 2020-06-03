// ReSharper disable All

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MyClassLibrary.Math
{
	public static class Math
	{

		public static double Hypot(in int a, in int b)
		{
			return System.Math.Sqrt(a * a + b * b);
		}

        private static int InternalGCD(int a, int b)
        {
            a = System.Math.Abs(a);
            b = System.Math.Abs(b);

            while (a != 0 && b != 0)
            {
                if (a > b)
                    a %= b;
                else
                    b %= a;
            }

            return a == 0 ? b : a;
        }

        public static int BinaryGCD(int u, int v)
        {
            var shift = 0;

            /* GCD(0,v) == v; GCD(u,0) == u, GCD(0,0) == 0 */
            if (u == 0) return v;
            if (v == 0) return u;

            /* Let shift := lg K, where K is the greatest power of 2
                dividing both u and v. */
            while (((u | v) & 1) == 0)
            {
                shift++;
                u >>= 1;
                v >>= 1;
            }

            while ((u & 1) == 0)
                u >>= 1;

            /* From here on, u is always odd. */
            do
            {
                /* remove all factors of 2 in v -- they are not common */
                /*   note: v is not zero, so while will terminate */
                while ((v & 1) == 0)
                    v >>= 1;

                /* Now u and v are both odd. Swap if necessary so u <= v,
                    then set v = v - u (which is even). For bignums, the
                     swapping is just pointer movement, and the subtraction
                      can be done in-place. */
                if (u > v)
                {
                    var t = v; v = u; u = t; // Swap u and v.
                }

                v -= u; // Here v >= u.
            } while (v != 0);

            /* restore common factors of 2 */
            return u << shift;
        }

        public static int GCD(params int[] values)
        {
            if (values.Length == 2)
                return InternalGCD(values[0], values[1]);

            var gdc = InternalGCD(values[0], values[1]);

            for (var i = 1; i < values.Length - 1; i++)
            {
                var newGcd = InternalGCD(gdc, values[i + 1]);
                gdc = System.Math.Min(gdc, newGcd);
            }

            return gdc;
        }

        public static int Phi(int n)
        {
            var result = 1;
            for (var i = 2; i < n; i++)
            {
                if (GCD(i, n) == 1)
                    result++;
            }
            return result;
        }

        public static int LCM(in int a, in int b)
        {
            int num1, num2;
            if (a > b)
            {
                num1 = a; num2 = b;
            }
            else
            {
                num1 = b; num2 = a;
            }

            for (var i = 1; i < num2; i++)
            {
                if ((num1 * i) % num2 == 0)
                {
                    return i * num1;
                }
            }
            return num1 * num2;
        }

        public static (int gcd, int s, int t) ExtendedEuclidean(int a, int b)
        {
            Debug.Assert(a > b);
            Debug.Assert(b > 0);
            
            var gcd = Math.GCD(a, b);

            var calcST = new Func<int, int, int, int, int, (int, int)>((int quotient, int prev1S, int prev2S, int prev1T, int prev2T) =>
            {
                return (prev2S - prev1S * quotient, prev2T - prev1T * quotient);
            });

            var quotient = System.Math.DivRem(a, b, out var remainder);

            var prev1S = 0;
            var prev2S = 1;
            var prev1T = 1;
            var prev2T = 0;

            var (temp1, temp2) = calcST(quotient, prev1S, prev2S, prev1T, prev2T);
            prev2S = prev1S;
            prev2T = prev1T;
            prev1S = temp1;
            prev1T = temp2;

            a = b;
            b = remainder;

            while (true)
            {
                quotient = System.Math.DivRem(a, b, out var rem);

                a = b;
                b = rem;

                if (rem == 0)
                    break;
                
                (temp1, temp2) = calcST(quotient, prev1S, prev2S, prev1T, prev2T);
                prev2S = prev1S;
                prev2T = prev1T;
                prev1S = temp1;
                prev1T = temp2;
            }

            return (gcd, prev1S, prev1T);
        }

        public static unsafe float Sqrt(float a)
        {
            var y = a;
            var i = *(int*)&y;
            i = 0x5f3759df - (i >> 1);
            y = *(float*)&i;
            y *= (1.5f - (a * 0.5f * y * y));
            return 1.0f / y;
        }
    }
}
