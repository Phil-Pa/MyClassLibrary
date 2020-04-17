﻿using MyClassLibrary.Encoding;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace UnitTestProject.Encoding
{
	public class ShannonAlgorithmTest
	{
		[Fact]
		public void TestSimple()
		{
			var encodingMap = new Dictionary<char, string>
			{
				{'a', "0"}, {'b', "10"}, {'c', "110"}
			};

			IEncodingAlgorithm encodingAlgorithm = new ShannonAlgorithm(true, encodingMap);

			string encoded = encodingAlgorithm.Encode("abcabcbac");

			Assert.Equal("010110010110100110", encoded);
		}

		[Fact]
		public void TestLargeDataset()
		{
			IEncodingAlgorithm encodingAlgorithm = new ShannonAlgorithm();

			string text = File.ReadAllText("E:/share/data.txt");

			string encoded = encodingAlgorithm.Encode(text);

			// ReSharper disable once RedundantAssignment
			string decoded = encodingAlgorithm.Decode(encoded);
			decoded = encodingAlgorithm.Decode(encoded);

			Assert.Equal(text, decoded);
		}

		[Fact]
		public void TestEdgeCases()
		{
			IEncodingAlgorithm encodingAlgorithm = new ShannonAlgorithm();
			Assert.Equal(string.Empty, encodingAlgorithm.Encode(""));

			Assert.Throws<ArgumentException>(() => encodingAlgorithm.Decode(""));

			encodingAlgorithm = new ShannonAlgorithm(true, new Dictionary<char, string>
			{
				{ '4', "0011" }
			});

			Assert.Throws<Exception>(() => encodingAlgorithm.Encode("5"));
		}
	}
}