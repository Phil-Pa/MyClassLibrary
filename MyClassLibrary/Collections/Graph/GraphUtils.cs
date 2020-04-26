using System;
using System.Collections.Generic;

namespace MyClassLibrary.Collections.Graph
{
	public static class GraphUtils
	{
		private static readonly Dictionary<Type, int> IdCounterMap = new Dictionary<Type, int>();

		/// <summary>
		/// Generates an id for an object typ <paramref name="type"/> starting with id 0
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static int GenerateId(Type type)
		{
			if (!IdCounterMap.ContainsKey(type))
			{
				IdCounterMap.Add(type, 0);
			}

			return IdCounterMap[type]++;
		}

		/// <summary>
		/// Resets the id counter for every type
		/// </summary>
		public static void ResetIdCounter()
		{
			IdCounterMap.Clear();
		}
	}
}