using MyClassLibrary.Encoding;
using System;
using System.Numerics;
using System.Text;

namespace MyClassLibrary.Math
{
	public class DefaultNumberConverter : INumberConverter
	{
		private const string BaseDigits = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz+-";

		public string Convert(int baseFrom, int baseTo, string number)
		{
			var dec = ToDecimal(number, baseFrom);
			return baseTo == 10 ? dec.ToString() : FromDecimal(baseTo, dec);
		}

		private static int Evaluate(char c)
		{
			if (c.IsBetweenAsciiCharactersInclusive('0', '9'))
				return int.Parse(c.ToString());
			if (c.IsBetweenAsciiCharactersInclusive('A', 'Z'))
				return c - 55;
			if (c.IsBetweenAsciiCharactersInclusive('a', 'z'))
				return c - 61;
			return c switch
			{
				'+' => 62,
				'-' => 63,
				_ => throw new ArgumentException("ivalid identifier")
			};
		}

		private static BigInteger ToDecimal(string str, int @base)
		{
			var len = str.Length;
			var power = BigInteger.One;
			var num = BigInteger.Zero;
			var i = len - 1;

			while (i >= 0)
			{
				var q = Evaluate(str[i]);
				var u = new BigInteger(q);
				num += u * power;
				power *= @base;

				--i;
			}

			return num;
		}

		private static char ReVal(int num)
		{
			return BaseDigits[num];
		}

		private static string FromDecimal(int base1, BigInteger inputNum)
		{
			var sb = new StringBuilder();
			var bbase1 = new BigInteger(base1);

			while (inputNum > BigInteger.Zero)
			{
				var quotient = BigInteger.DivRem(inputNum, bbase1, out var remainder);
				sb.Append(ReVal(int.Parse(remainder.ToString())));
				inputNum = quotient;
			}

			return sb.ToString().Reverse();
		}
	}
}