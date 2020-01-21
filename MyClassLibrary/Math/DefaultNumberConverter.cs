using System;
using System.Numerics;
using System.Text;
using MyClassLibrary.Encoding;

namespace MyClassLibrary.Math
{
	public class DefaultNumberConverter : INumberConverter
	{

		private const string BaseDigits = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz+-";

		public string Convert(int baseFrom, int baseTo, string number)
		{
			BigInteger dec = ToDecimal(number, baseFrom);
			return baseTo == 10 ? dec.ToString() : FromDecimal(baseTo, dec);
		}

		private static int Evaluate(char c)
		{
			if (c >= '0' && c <= '9')
				return int.Parse(c.ToString());
			if (c >= 'A' && c <= 'Z')
				return c - 55;
			if (c >= 'a' && c <= 'z')
				return c - 61;
			return c switch
			{
				'+' => 62,
				'-' => 63,
				_ => throw new ArgumentException()
			};
		}

		private static BigInteger ToDecimal(string str, int @base)
		{
			var len = str.Length;
			BigInteger power = BigInteger.One;
			BigInteger num = BigInteger.Zero;
			var i = len - 1;

			while (i >= 0)
			{

				var q = Evaluate(str[i]);
				BigInteger u = new BigInteger(q);
				num += u * power;
				power *= @base;

				--i;
			}

			return num;
		}

		private static char ReVal(int num) => BaseDigits[num];

		private static string FromDecimal(int base1, BigInteger inputNum)
		{
			StringBuilder sb = new StringBuilder();
			BigInteger bbase1 = new BigInteger(base1);

			while (inputNum > BigInteger.Zero)
			{
				BigInteger quotient = BigInteger.DivRem(inputNum, bbase1, out BigInteger remainder);
				sb.Append(ReVal(int.Parse(remainder.ToString())));
				inputNum = quotient;
			}

			return sb.ToString().Reverse();
		}
	}
}
