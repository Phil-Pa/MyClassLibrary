using System;
using System.Collections.Generic;
using System.Diagnostics;
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

		[DebuggerHidden]
		public static int ToInt(this char c)
		{
			if (c < '0' || c > '9')
				return 0;

			return (int)char.GetNumericValue(c);
		}

		[DebuggerHidden]
		public static byte ToByte(this char c)
		{
			if (c < '0' || c > '9')
				return 0;

			double result = char.GetNumericValue(c);
			Debug.Assert(result >= byte.MinValue && result <= byte.MaxValue);
			return (byte)result;
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

		public static IEnumerable<IEnumerable<T>> Pivot<T>(this IEnumerable<IEnumerable<T>> source)
		{
			var enumerators = source.Select(e => e.GetEnumerator()).ToArray();
			try
			{
				while (enumerators.All(e => e.MoveNext()))
				{
					yield return enumerators.Select(e => e.Current).ToArray();
				}
			}
			finally
			{
				Array.ForEach(enumerators, e => e.Dispose());
			}
		}

		public static bool IsBetweenAsciiCharactersInclusive(this char c, char lowerLimit, char upperLimit)
		{
			return c >= lowerLimit && c <= upperLimit;
		}

		public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
		{
			return source.Shuffle(new Random());
		}

		public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random rng)
		{
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (rng == null) throw new ArgumentNullException(nameof(rng));

			return source.ShuffleIterator(rng);
		}

		private static IEnumerable<T> ShuffleIterator<T>(
			this IEnumerable<T> source, Random rng)
		{
			var buffer = source.ToList();
			for (int i = 0; i < buffer.Count; i++)
			{
				int j = rng.Next(i, buffer.Count);
				yield return buffer[j];

				buffer[j] = buffer[i];
			}
		}

		public static IEnumerable<IEnumerable<T>> GetPermutations<T>(this IEnumerable<T> list, int length)
		{
			if (length == 1) return list.Select(t => new[] { t });

			var enumerable = list as T[] ?? list.ToArray();
			return GetPermutations(enumerable, length - 1)
				.SelectMany(t => enumerable.Where(e => !t.Contains(e)),
					(t1, t2) => t1.Concat(new[] { t2 }));
		}

		public static bool ContainsAll<T>(this IEnumerable<T> list, IEnumerable<T> items)
		{
			return items.All(list.Contains);
		}
	}
}