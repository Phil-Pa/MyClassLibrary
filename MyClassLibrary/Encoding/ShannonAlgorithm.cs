using MyClassLibrary.Math;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MyClassLibrary.Encoding
{
	/// <summary>
	/// A class for using the Shannon-Fano Algorithm
	/// </summary>
	public class ShannonAlgorithm : IEncodingAlgorithm
	{
		private IDictionary<string, char>? _decodingMap;
		private IDictionary<char, string>? _encodingMap;
		private readonly bool _enableErrorChecking;

		public ShannonAlgorithm(bool enableErrorChecking = true, IDictionary<char, string>? encodingMap = null)
		{
			_enableErrorChecking = enableErrorChecking;
			_encodingMap = encodingMap;
		}

		public string Encode(string str)
		{
			if (string.IsNullOrEmpty(str))
				return string.Empty;

			var encodingMap = _encodingMap ?? BuildEncodingMap(BuildProbabilityMap(str));

			if (_enableErrorChecking)
				CheckSymbolsAreInEncodingMap(str);

			var sb = new StringBuilder();

			foreach (var c in str)
			{
				sb.Append(encodingMap[c]);
			}

			return sb.ToString();
		}

		private static List<(Fraction, char)> BuildProbabilityMap(string str)
		{
			var chars = str.ToCharArray().Distinct().ToList();
			var numChars = str.Length;
			var map = new List<ValueTuple<Fraction, char>>();

			foreach (var c in chars)
			{
				var c1 = c;
				var charCount = str.Count(character => c1 == character);
				map.Add(new ValueTuple<Fraction, char>(new Fraction(charCount, numChars), c));
			}

			map = map.OrderByDescending(tuple => tuple.Item1).ToList();
			return map;
		}

		private static IDictionary<char, string> CreateEncodingMap(IEnumerable<(Fraction, char)> pMap)
		{
			var map = new Dictionary<char, string>();

			var chars = pMap.Select(tuple => tuple.Item2).Distinct();
			foreach (var c in chars)
			{
				map[c] = "";
			}

			return map;
		}

		private static void AddToEncodingMap(IDictionary<char, string> map, IEnumerable<(Fraction, char)> list, string symbol)
		{
			foreach (var pair in list)
			{
				map[pair.Item2] += symbol;
			}
		}

		private static void InternalBuildEncodingMap(IDictionary<char, string> map, IReadOnlyList<(Fraction, char)> pMap, int iteration)
		{
			if (pMap.IsEmpty())
				return;

			switch (pMap.Count)
			{
				case 1:
					AddToEncodingMap(map, new List<(Fraction, char)> { pMap[0] }, "1");
					return;

				case 2:
					AddToEncodingMap(map, new List<(Fraction, char)> { pMap[0] }, "0");
					AddToEncodingMap(map, new List<(Fraction, char)> { pMap[1] }, "1");
					return;
			}

			var limit = 0.5f / (iteration + 1);
			var fractionSum = pMap.Sum(tuple => tuple.Item1.ToFloat());
			if (fractionSum <= limit)
				limit = fractionSum / 2;

			var sum = 0f;

			var zeroList = new List<ValueTuple<Fraction, char>>();

			foreach (var tuple in pMap)
			{
				sum += tuple.Item1.ToFloat();
				zeroList.Add(tuple);
				if (sum >= limit)
				{
					var oneList = pMap.Where(p => !zeroList.Select(valueTuple => valueTuple.Item2).Contains(p.Item2)).ToList();

					AddToEncodingMap(map, zeroList, "0");
					AddToEncodingMap(map, oneList, "1");
					iteration++;

					InternalBuildEncodingMap(map, zeroList, iteration);
					InternalBuildEncodingMap(map, oneList, iteration);
					return;
				}
			}
		}

		private IDictionary<char, string> BuildEncodingMap(IReadOnlyCollection<(Fraction, char)> pMap)
		{
			var map = CreateEncodingMap(pMap);

			var iteration = 0;
			var sum = 0f;

			var zeroList = new List<(Fraction, char)>();

			foreach (var pair in pMap)
			{
				sum += pair.Item1.ToFloat();
				zeroList.Add(pair);

				if (sum >= 0.5f / (iteration + 1))
				{
					var oneList = pMap.Where(p => !zeroList.Select(tuple => tuple.Item2).Contains(p.Item2)).ToList();

					AddToEncodingMap(map, zeroList, "0");
					AddToEncodingMap(map, oneList, "1");
					iteration++;

					InternalBuildEncodingMap(map, zeroList, iteration);
					InternalBuildEncodingMap(map, oneList, iteration);
					break;
				}
			}

			_encodingMap = map;
			return map;
		}

		private void CheckSymbolsAreInEncodingMap(string str)
		{

			foreach (var c in str)
			{
				Debug.Assert(_encodingMap != null);
				if (!_encodingMap.Keys.Contains(c))
					throw new Exception($"{c} is not in encoding map");
			}
		}

		private IDictionary<string, char> AquireDecodingMap()
		{
			return _decodingMap ??= _encodingMap.ToDictionary(pair => pair.Value, pair => pair.Key);
		}

		public string Decode(string str)
		{
			if (string.IsNullOrEmpty(str))
				throw new ArgumentException("cant decode empty string");

			var decodingMap = AquireDecodingMap();

			StringBuilder sb = new StringBuilder(), result = new StringBuilder();

			foreach (var c in str)
			{
				sb.Append(c);

				var tempStr = sb.ToString();

				if (decodingMap.ContainsKey(tempStr))
				{
					result.Append(decodingMap[tempStr]);
					sb.Clear();
				}
			}

			return result.ToString();
		}
	}
}