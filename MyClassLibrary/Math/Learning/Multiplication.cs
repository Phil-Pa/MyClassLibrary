using System;
using System.Collections.Generic;
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
			TempMultiplicationOrder tempMultiplicationOrder = TempMultiplicationOrder.AInnerToBOuter)
		{

			string strA = a.ToString();
			string strB = b.ToString();

			string aTimesB = " " + strA + "*" + strB;

			StringBuilder sb = new StringBuilder(aTimesB + NewLine);

			int numDigitsA = strA.Length;
			int numDigitsB = strB.Length;

			switch (tempMultiplicationOrder)
			{
				case TempMultiplicationOrder.AInnerToBOuter:
					for (int i = numDigitsA - 1; i >= 0; i--)
					{
						for (int j = numDigitsB - 1; j >= 0; j--)
						{
							CalculateTempResult(strA, i, strB, j, aTimesB, sb, tempMultiplicationOrder);
						}
					}
					break;
				case TempMultiplicationOrder.AInnerToBInner:
					for (int i = numDigitsA - 1; i >= 0; i--)
					{
						for (int j = 0; j < numDigitsB; j++)
						{
							CalculateTempResult(strA, i, strB, j, aTimesB, sb, tempMultiplicationOrder);
						}
					}
					break;
				case TempMultiplicationOrder.AOuterToBOuter:
					for (int i = 0; i < numDigitsA; i++)
					{
						for (int j = numDigitsB - 1; j >= 0; j--)
						{
							CalculateTempResult(strA, i, strB, j, aTimesB, sb, tempMultiplicationOrder);
						}
					}
					break;
				case TempMultiplicationOrder.AOuterToBInner:
					for (int i = 0; i < numDigitsA; i++)
					{
						for (int j = 0; j < numDigitsB; j++)
						{
							CalculateTempResult(strA, i, strB, j, aTimesB, sb, tempMultiplicationOrder);
						}
					}
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(tempMultiplicationOrder), tempMultiplicationOrder, null);
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

		private static void CalculateTempResult(string strA, in int i, string strB, in int j, string aTimesB, StringBuilder sb, TempMultiplicationOrder tempMultiplicationOrder)
		{
			int tempResult = strA[i].ToInt() * strB[j].ToInt();
			string strTempResult = tempResult.ToString();

			string spacesAfterTempResult = new string(' ', i + j);
			string spaces =
				new string(' ', aTimesB.Length - strTempResult.Length - spacesAfterTempResult.Length - 1); // -1 for the + sign

			sb.Append("+").Append(spaces).Append(strTempResult).Append(spacesAfterTempResult).Append(NewLine);
		}
	}
}
