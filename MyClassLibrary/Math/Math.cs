using System;
using System.Collections.Generic;
using System.Text;

namespace MyClassLibrary.Math
{
	public static class Math
	{

		public static double Hypot(int a, int b)
		{
			return System.Math.Sqrt(a * a + b * b);
		}

	}
}
