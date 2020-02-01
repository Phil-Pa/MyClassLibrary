using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace MyClassLibrary.Math
{
	public static class Combinations
	{
		public const string Empty = "-";

		public static IEnumerable<string> GetCombinations(in int n)
		{
			if (n <= 0)
				throw new ArgumentException("n must be greater than 0");

			if (n == 1)
				return new List<string>{Empty, "0"};

			var result = new List<string>{Empty};

			for (var i = 1; i <= n; i++)
			{
				result.AddRange(GetSpecificCombinationsOfLength(i));
			}

			for (var i = 0; i < result.Count; i++)
			{
				result[i] = SortNumbers(result[i]);
			}

			result.Sort(new StrCmpLogicalComparer());

			return result;
		}

		private static IEnumerable<string> GetSpecificCombinationsOfLength(int n)
		{
			if (n <= 0)
				throw new ArgumentException("n must be greater than 0");

			if (n == 1)
				return new List<string>{"0", "1"};

			var result = new List<string>();

			

			// add all pairs of combinations

			switch (n)
			{
				case 2:
				{
					for (var i = 0; i < n; i++)
					{
						for (var j = 0; j < n; j++)
						{
							result.Add(i.ToString() + j);
						}
					}

					break;
				}
				case 3:
				{
					for (var i = 0; i < n; i++)
					{
						for (var j = 0; j < n; j++)
						{
							for (var k = 0; k < n; k++)
							{
								result.Add(i.ToString() + j + n);
							}
						}
					}

					break;
				}
				case 4:
				{
					for (var i = 0; i < n; i++)
					{
						for (var j = 0; j < n; j++)
						{
							for (var k = 0; k < n; k++)
							{
								for (var l = 0; l < n; l++)
								{
									result.Add(i.ToString() + j + n + l);
								}
							}
						}
					}

					break;
				}
				case 5:
				{
					for (var i = 0; i < n; i++)
					{
						for (var j = 0; j < n; j++)
						{
							for (var k = 0; k < n; k++)
							{
								for (var l = 0; l < n; l++)
								{
									for (var m = 0; m < n; m++)
									{
										result.Add(i.ToString() + j + n + l + m);
									}
								}
							}
						}
					}

					break;
				}
			}

			// remove same pairs
			for (var i = 0; i < result.Count; i++)
			{
				for (var j = 0; j < result.Count; j++)
				{
					if (i == j)
						continue;

					while (i >= result.Count)
						i--;

					while (j >= result.Count)
						j--;

					var combination1 = result[i];
					var combination2 = result[j];

					if (CombinationsAreEqual(combination1, combination2))
					{
						result.Remove(combination1);
					}
				}
			}

			for (var i = 0; i < n; i++)
			{
				if (result[i].ContainsOnly(char.Parse(i.ToString())))
					result.Remove(result[i]);
			}

			return result;
		}

		private static string SortNumbers(IEnumerable<char> numbers)
		{
			var result = from n in numbers
				orderby n ascending
				select n;

			StringBuilder sb = new StringBuilder();
			foreach (var c in result)
			{
				sb.Append(c);
			}

			return sb.ToString();
		}

		private static bool CombinationsAreEqual(string a, string b)
		{
			return a.Distinct().Count() == b.Distinct().Count();
		}

		private class StrCmpLogicalComparer : Comparer<string>
		{

			public override int Compare(string x, string y)
			{
				if (x == null || y == null)
					return 0;

				if (x == Empty || y == Empty)
					return 1;

				if (x.Length < y.Length)
					return -1;

				if (y.Length < x.Length)
					return 1;

				for (var i = 0; i < x.Length; i++)
				{
					var n1 = (int)x[i];
					var n2 = (int)y[i];

					if (n1 != n2)
						return n1.CompareTo(n2);
				}

				return 0;
			}
		}
	}
}
