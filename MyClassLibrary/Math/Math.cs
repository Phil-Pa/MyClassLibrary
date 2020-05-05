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

            var result = a == 0 ? b : a;

            //if (a < 0 && b < 0)


            return result;
        }

        public static int GCD(params int[] values)
        {
            if (values.Length == 2)
                return InternalGCD(values[0], values[1]);

            var gdc = InternalGCD(values[0], values[1]);

            for (var i = 1; i < values.Length - 1; i++)
            {
                var newGDC = InternalGCD(gdc, values[i + 1]);
                gdc = System.Math.Min(gdc, newGDC);
            }

            return gdc;
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

	}
}
