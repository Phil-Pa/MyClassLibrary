using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using MyClassLibrary.Encoding;
using MyClassLibrary.Math;

namespace MyClassLibraryBenchmark
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var summary = BenchmarkRunner.Run<MyBenchmark>();

			Console.ReadKey();
		}
	}

	public class MyBenchmark
	{

		[ParamsSource(nameof(TextSource))]
		public string Text { get; set; }

		public static IEnumerable<string> TextSource()
		{
			const string text1 = "12938932842938742839642147883743894317478231743284923784742834729374823742394328742389173890213021842384672314897321894316748164328794238142190ß7438957384927ß846723784326148217464181874238578934577823";
			const string text2 = "hier ist mein ganz langer textkrjkefjdksffdsfhjdskfnm,ncvxmkfhjdksfjeiwrjfkvfndmv,ynkjdhfaoiwehjkdsfnd,msafdskajreiwhdkjsfda.nfasbyjhdflwekjdmsjdkls";

			return new[]
			{
				text1, text2
			};
		}

		[Benchmark]
		public string TestCompress()
		{

			return StringCompression.Compress(Text);
		}

		[Benchmark]
		public string TestDecompress()
		{

			var compressed = StringCompression.Compress(Text);
			var decompressed = StringCompression.Decompress(compressed);

			return decompressed;
		}
	}
}
