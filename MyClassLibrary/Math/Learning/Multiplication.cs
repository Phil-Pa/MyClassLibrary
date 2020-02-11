﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MyClassLibrary.Math.Learning
{
	public static class Multiplication
	{

		private const string NewLine = "\n";

		public static (int result, string calculation) DoMultiply(in int a, in int b)
		{

			string strA = a.ToString();
			string strB = b.ToString();

			string aTimesB = " " + strA + "*" + strB;

			StringBuilder sb = new StringBuilder(aTimesB + NewLine);

			int numDigitsA = strA.Length;
			int numDigitsB = strB.Length;

			int aTimesBLength = aTimesB.Length;

			for (int i = numDigitsA - 1; i >= 0; i--)
			{
				for (int j = numDigitsB - 1; j >= 0; j--)
				{
					CalculateTempResult(strA, i, strB, j, aTimesBLength, sb);
				}
			}

			var lines = sb.ToString().Split(NewLine)[1..^1].Select(str => str.Substring(1)).ToList();

			if (lines.Count > 1)
			{
				StringBuilder overflow = new StringBuilder(aTimesBLength + 1);
				overflow.Append(new string(' ', aTimesBLength + 1));
				overflow[0] = 'ü';
				overflow[overflow.Capacity - 1] = '\n';

				int length = lines[0].Length;
				bool hasOverflow = false;

				Debug.Assert(lines.All(str => str.Length == length));

				for (int i = length - 1; i >= 0; i--)
				{
					int columnSum = lines.Sum(str => str[i].ToInt());

					var values = lines.Select(str => str[i].ToInt()).ToList();

					if (columnSum >= 10)
					{
						var columnSumAsChar = columnSum.ToString()[0];

						StringBuilder lineBuilder = new StringBuilder(new string(' ', aTimesBLength))
						{
							[i] = columnSumAsChar
						};
						lines.Add(lineBuilder.ToString());
						hasOverflow = true;
						overflow[i] = columnSumAsChar;
					}
				}

				if (hasOverflow)
					sb.Append(overflow.ToString());
			}

			return CalculateFinalResult(a, b, aTimesB, sb);
		}

		private static (int result, string calculation) CalculateFinalResult(in int a, in int b, string aTimesB, StringBuilder sb)
		{
			int intResult = a * b;
			string strResult = intResult.ToString();

			string spacesResult = new string(' ', aTimesB.Length - intResult.ToString().Length - 1); // -1 for = sign

			sb.Append("=").Append(spacesResult).Append(strResult);

			return (intResult, sb.ToString());
		}

		private static void CalculateTempResult(string strA, in int i, string strB, in int j, in int aTimesBLength, StringBuilder sb)
		{
			int tempResult = strA[i].ToInt() * strB[j].ToInt();
			string strTempResult = tempResult.ToString();

			var (spacesBeforeResult, spacesAfterResult) =
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

			int numSpaces = GetTempAfterSpaces(aLength - i, bLength - j);

			string spacesAfterTempResult = new string(' ', numSpaces);

			string spaces =
				new string(' ', aTimesBLength - tempResultLength - spacesAfterTempResult.Length - 1); // -1 for the + sign

			var spacesBeforeResult = spaces;
			var spacesAfterResult = spacesAfterTempResult;

			return (spacesBeforeResult, spacesAfterResult);
		}

		private static int GetTempAfterSpaces(in int i, in int j)
		{

			int x = 1;
			while (true)
			{
				if (i == x || j == x)
					return System.Math.Abs(i - j) + 2 * (x - 1);

				x++;
			}
		}
	}
}
