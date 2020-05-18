using System;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MyClassLibrary.Learning
{
	/// <summary>
	/// Static Multiplication class providing the <see cref="DoMultiply"/> method for multiplying in writing
	/// </summary>
	public static class Multiplication
	{
		private const string NewLine = "\n";

		public static (int result, string calculation) DoMultiply(in int a, in int b)
		{
			if (a < 0 || b < 0)
				throw new ArgumentException("cant multiply negative numbers");

			if (a * b < 0)
			{
				// overflow

				throw new ArgumentException(a + " * " + b + " is greater than int.MaxValue");
			}

			string strA = a.ToString();
			string strB = b.ToString();

			string aTimesB = " " + strA + "*" + strB;

			var sb = new StringBuilder(aTimesB + NewLine);

			var numDigitsA = strA.Length;
			var numDigitsB = strB.Length;

			var aTimesBLength = aTimesB.Length;

			for (var i = numDigitsA - 1; i >= 0; i--)
			{
				for (var j = numDigitsB - 1; j >= 0; j--)
				{
					CalculateTempResult(strA, i, strB, j, aTimesBLength, sb);
				}
			}

			var lines = sb.ToString().Split(NewLine)[1..^1].Select(str => str.Substring(1)).ToList();

			if (lines.Count <= 1)
				return CalculateFinalResult(a, b, aTimesB, sb);

			var overflow = new StringBuilder(aTimesBLength + 1);
			overflow.Append(new string(' ', aTimesBLength + 1));
			overflow[0] = 'ü';
			overflow[overflow.Capacity - 1] = '\n';

			var length = lines[0].Length;
			var hasOverflow = false;

			Debug.Assert(lines.All(str => str.Length == length));

			for (var i = length - 1; i >= 0; i--)
			{
				var columnSum = lines.Sum(str => str[i].ToInt());

				if (columnSum < 10)
					continue;

				var columnSumAsChar = columnSum.ToString()[0];

				var lineBuilder = new StringBuilder(new string(' ', aTimesBLength))
				{
					[i] = columnSumAsChar
				};
				lines.Add(lineBuilder.ToString());
				hasOverflow = true;
				overflow[i] = columnSumAsChar;
			}

			if (hasOverflow)
				sb.Append(overflow.ToString());

			return CalculateFinalResult(a, b, aTimesB, sb);
		}

		private static (int result, string calculation) CalculateFinalResult(in int a, in int b, string aTimesB, StringBuilder sb)
		{
			var intResult = a * b;
			var strResult = intResult.ToString();

			var spacesResult = new string(' ', aTimesB.Length - intResult.ToString().Length - 1); // -1 for = sign

			sb.Append("=").Append(spacesResult).Append(strResult);

			return (intResult, sb.ToString());
		}

		private static void CalculateTempResult(string strA, in int i, string strB, in int j, in int aTimesBLength, StringBuilder sb)
		{
			var tempResult = strA[i].ToInt() * strB[j].ToInt();
			var strTempResult = tempResult.ToString();

			(var spacesBeforeResult, var spacesAfterResult) =
				GetTempResultSpaces(strA.Length, strB.Length, strTempResult.Length, aTimesBLength, i, j);

			sb.Append("+").Append(spacesBeforeResult).Append(strTempResult).Append(spacesAfterResult).Append(NewLine);
		}

		private static (string spacesBeforeResult, string spacesAfterResult) GetTempResultSpaces(
			in int aLength, in int bLength,
			in int tempResultLength, in int aTimesBLength,
			in int i, in int j)
		{
			// length of something like " 4*6" must be 4

			if (aLength == 1 && bLength == 1)
				return (new string(' ', 4 - 1 - tempResultLength), string.Empty);

			var numSpaces = GetTempAfterSpaces(aLength - i, bLength - j);

			var spacesAfterTempResult = new string(' ', numSpaces);

			var spaces =
				new string(' ', aTimesBLength - tempResultLength - spacesAfterTempResult.Length - 1); // -1 for the + sign

			var spacesBeforeResult = spaces;
			var spacesAfterResult = spacesAfterTempResult;

			return (spacesBeforeResult, spacesAfterResult);
		}

		private static int GetTempAfterSpaces(in int i, in int j)
		{
			var x = 1;
			while (true)
			{
				if (i == x || j == x)
					return System.Math.Abs(i - j) + 2 * (x - 1);

				x++;
			}
		}
	}
}