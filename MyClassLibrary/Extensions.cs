using System;
using System.Collections.Generic;
using System.Linq;

namespace MyClassLibrary
{
	public static class Extensions
	{
		public static bool ContainsOnly<T>(this IEnumerable<T> enumerable, T value) where T : IComparable<T>
		{
			return enumerable.All(item => item.CompareTo(value) == 0);
		}

		public static int ToInt(this string str)
		{
			bool success = int.TryParse(str, out int result);
			if (success)
				return result;
			else
				throw new ArgumentException("str can no be converted to an int");
		}

		public static int ToInt(this char c)
		{
			return (int)char.GetNumericValue(c);
		}

		public static int Count<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
		{
			return Enumerable.Count(enumerable, predicate);
		}

		public static bool IsEmpty<T>(this IEnumerable<T> enumerable)
		{
			return !enumerable.Any();
		}

		public static bool IsNotEmpty<T>(this IEnumerable<T> enumerable)
		{
			return !IsEmpty(enumerable);
		}

		public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> enumerable)
		{
			var list = new List<T>();
			foreach (var item in enumerable)
			{
				list.AddRange(item);
			}

			return list;
		}

		public static bool IsBetweenAsciiCharactersInclusive(this char c, char lowerLimit, char upperLimit)
		{
			return c >= lowerLimit && c <= upperLimit;
		}

	}
}
