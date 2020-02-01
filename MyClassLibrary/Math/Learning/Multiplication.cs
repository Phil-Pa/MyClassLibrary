using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyClassLibrary.Math.Learning
{
	public static class Multiplication
	{

		/// <summary>
		/// Determines in which order the algorithm calculates the temporary result. <br/>
		/// <see cref="AInnerToBOuter"/> calculates aaaaa &lt; to bbbbb &lt; <br/>
		/// <see cref="AInnerToBInner"/> calculates aaaaa &lt; to &gt; bbbbb <br/>
		/// <see cref="AOuterToBOuter"/> calculates &gt; aaaaa to bbbbb &lt; <br/>
		/// <see cref="AOuterToBInner"/> calculates &gt; aaaaa to &gt; bbbbb <br/>
		/// </summary>
		public enum TempMultiplicationOrder
		{
			AInnerToBOuter,
			AInnerToBInner,
			AOuterToBOuter,
			AOuterToBInner
		}

		private const string NewLine = "\n";

		public static (int result, string calculation) DoMultiply(int a,
			int b,
			in TempMultiplicationOrder tempMultiplicationOrder = TempMultiplicationOrder.AInnerToBOuter)
		{

			string strA = a.ToString();
			string strB = b.ToString();

			string aTimesB = " " + strA + "*" + strB;

			StringBuilder sb = new StringBuilder(aTimesB + NewLine);

			int numDigitsA = strA.Length;
			int numDigitsB = strB.Length;

			int aTimesBLength = aTimesB.Length;

			switch (tempMultiplicationOrder)
			{
				case TempMultiplicationOrder.AInnerToBOuter:
					for (int i = numDigitsA - 1; i >= 0; i--)
					{
						for (int j = numDigitsB - 1; j >= 0; j--)
						{
							CalculateTempResult(strA, i, strB, j, aTimesBLength, sb, tempMultiplicationOrder);
						}
					}
					break;
				case TempMultiplicationOrder.AInnerToBInner:
					for (int i = numDigitsA - 1; i >= 0; i--)
					{
						for (int j = 0; j < numDigitsB; j++)
						{
							CalculateTempResult(strA, i, strB, j, aTimesBLength, sb, tempMultiplicationOrder);
						}
					}
					break;
				case TempMultiplicationOrder.AOuterToBOuter:
					for (int i = 0; i < numDigitsA; i++)
					{
						for (int j = numDigitsB - 1; j >= 0; j--)
						{
							CalculateTempResult(strA, i, strB, j, aTimesBLength, sb, tempMultiplicationOrder);
						}
					}
					break;
				case TempMultiplicationOrder.AOuterToBInner:
					for (int i = 0; i < numDigitsA; i++)
					{
						for (int j = 0; j < numDigitsB; j++)
						{
							CalculateTempResult(strA, i, strB, j, aTimesBLength, sb, tempMultiplicationOrder);
						}
					}
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(tempMultiplicationOrder), tempMultiplicationOrder, null);
			}

			var lines = sb.ToString().Split(NewLine)[1..^1].Select(str => str.Substring(1)).ToArray();
			var newLines = lines.Pivot().Select(chars => new string(chars.ToArray()).Replace(" ", string.Empty)).Where(str => str.Length > 0).Reverse().ToList();

			StringBuilder sbOverflow = new StringBuilder(aTimesBLength);
			sbOverflow.Append('ü');

			StringBuilder overflow = new StringBuilder(aTimesBLength);
			overflow.Append(new string(' ', aTimesBLength));
			overflow[0] = 'ü';
			overflow[overflow.Capacity - 1] = '\n';

			int x = aTimesBLength;

			for (int i = 0; i < newLines.Count; i++)
			{
				int sum = newLines[i].Sum(c => c.ToInt());
				if (sum >= 10)
				{
					overflow[x] = sum.ToString()[0];
					if (i + 1 < newLines.Count)
						newLines[i + 1] += (sum % 10).ToString();
				}

				x--;
			}

			if (overflow.Length > 1)
				sb.Append(overflow.ToString());

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

		private static void CalculateTempResult(string strA, in int i, string strB, in int j, in int aTimesBLength, StringBuilder sb, in TempMultiplicationOrder tempMultiplicationOrder)
		{
			int tempResult = strA[i].ToInt() * strB[j].ToInt();
			string strTempResult = tempResult.ToString();

			string spacesAfterTempResult = new string(' ', i + j);
			string spaces =
				new string(' ', aTimesBLength - strTempResult.Length - spacesAfterTempResult.Length - 1); // -1 for the + sign

			var (spacesBeforeResult, spacesAfterResult) =
				GetTempResultSpaces(strA.Length, strB.Length, strTempResult.Length, aTimesBLength, i, j, tempMultiplicationOrder);

			sb.Append("+").Append(spacesBeforeResult).Append(strTempResult).Append(spacesAfterResult).Append(NewLine);
		}

		private static (string spacesBeforeResult, string spacesAfterResult) GetTempResultSpaces(
			in int aLength, in int bLength,
			in int tempResultLength, in int aTimesBLength, 
			in int i, in int j, in TempMultiplicationOrder tempMultiplicationOrder)
		{

			string spacesBeforeResult = null, spacesAfterResult = null;

			switch (tempMultiplicationOrder)
			{
				case TempMultiplicationOrder.AInnerToBOuter:

					// length of something like " 4*6" must be 4

					if (aLength == 1 && bLength == 1)
						return (new string(' ', 4 - 1 - tempResultLength), string.Empty);

					//string spacesAfterTempResult = new string(' ', bLength - (aLength - i - 1) + (bLength - j - 1));
					//string spacesAfterTempResult = new string(' ', bLength - i + j);

					//int numSpaces = System.Math.Abs((aLength - i) - (bLength - j));
					int numSpaces = GetTempAfterSpaces(aLength - i, bLength - j);

					string spacesAfterTempResult = new string(' ', numSpaces);

					string spaces =
						new string(' ', aTimesBLength - tempResultLength  - spacesAfterTempResult.Length - 1); // -1 for the + sign

					spacesBeforeResult = spaces;
					spacesAfterResult = spacesAfterTempResult;
					break;
				case TempMultiplicationOrder.AInnerToBInner:
					break;
				case TempMultiplicationOrder.AOuterToBOuter:
					break;
				case TempMultiplicationOrder.AOuterToBInner:
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(tempMultiplicationOrder), tempMultiplicationOrder, null);
			}

			return (spacesBeforeResult, spacesAfterResult);
		}

		private static int GetTempAfterSpaces(in int i, in int j)
		{

			//int lower = System.Math.Min(i, j);
			//int diff = System.Math.Abs(i - j);

			//if (i == 1 || j == 1)
			//	return System.Math.Abs(i - j);

			//if (i == 2 || j == 2)
			//	return System.Math.Abs(i - j) + 2;

			//if (i == 3 || j == 3)
			//	return System.Math.Abs(i - j) + 4;

			//for (int k = 0; k < 10; k++)
			//{
			//	if (i == k || j == k)
			//		return System.Math.Abs(i - j) + 2 * (k - 1);
			//}

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
