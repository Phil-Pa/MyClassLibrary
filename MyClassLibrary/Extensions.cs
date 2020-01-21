using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyClassLibrary
{
	public static class Extensions
	{

		public static int Count<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
		{
			return Enumerable.Count(enumerable, predicate);
		}

		public static bool IsEmpty<T>(this IEnumerable<T> enumerable)
		{
			return !enumerable.Any();
		}

	}
}
