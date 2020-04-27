using BenchmarkDotNet.Attributes;
using MyClassLibrary.Algorithms.AStar;
using MyClassLibrary.Encoding;
using MyClassLibrary.Math.Learning;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MyClassLibrary.FileSystem;
using MyClassLibrary.FileSystem.CodeCounter;
using MyClassLibrary.TaskScheduling;

// ReSharper disable MemberCanBePrivate.Global

namespace MyClassLibraryBenchmark
{
	internal static class Program
	{
		private static void Main(string[] args)
        {

            //var summary = BenchmarkRunner.Run<AStarAlgorithmBenchmark>();

			//var task1 = new Task(name: "Task1", description: "Task1 Test", duration: TimeSpan.FromMinutes(20), isParallel: true, priority: 3, dependingTasks: null);
			//var task2 = new Task(name: "Task2", description: "Task2 Test", duration: TimeSpan.FromMinutes(15), isParallel: false, priority: 3, dependingTasks: null);
			//var task3 = new Task(name: "Task3", description: "Task3 Test", duration: TimeSpan.FromMinutes(25), isParallel: true, priority: 3, new List<Task>() { task1 });
			//var task4 = new Task(name: "Task4", description: "Task4 Test", duration: TimeSpan.FromMinutes(10), isParallel: false, priority: 3, new List<Task>() { task2 });
			//var task5 = new Task(name: "Task5", description: "Task5 Test", duration: TimeSpan.FromMinutes(30), isParallel: true, priority: 3, new List<Task>() { task3 });
			//var task6 = new Task(name: "Task6", description: "Task6 Test", duration: TimeSpan.FromMinutes(20), isParallel: false, priority: 3, new List<Task>() { task4 });
			//var task7 = new Task(name: "Task7", description: "Task7 Test", duration: TimeSpan.FromMinutes(15), isParallel: true, priority: 3, new List<Task>() { task5, task6 });

			//var tasks = new List<Task> { task1, task2, task3, task4, task5, task6, task7 };

			//var scheduler = new TaskScheduler(tasks, 1);
			//var sequence = scheduler.CalculateTaskSequence();

			//var watch = Stopwatch.StartNew();
			//var grid = AStarAlgorithm.CreateGrid(200, 10);
			//var algorithm = new AStarAlgorithm(grid);

			//for (var i = 0; i < 1; i++)
			//{
			//	var path = algorithm.FindPath();
			//	Console.WriteLine(path.Count);
			//}

			//watch.Stop();

			//Console.WriteLine(watch.ElapsedMilliseconds);

            const string path = @"C:\Users\Phil\source\projects\csharp\libraries";

			var fileInterpreter = new CodeReader();
			var fileReader = new FileReader();
			var directoryReader = new DirectoryReader();
			var converter = new DirectoryTreeToGraphConverter<Language, CodeStats>(fileReader, directoryReader);
			var analyzer = new DirectoryAnalyzer<Language, CodeStats>(converter, path, CodeStats.Default, fileInterpreter);

            var stats = analyzer.GetTotalStats();

            var fileInterpreter2 = new FileSizeInterpreter();
            var converter2 = new DirectoryTreeToGraphConverter<string, MyInt>(fileReader, directoryReader);
            var analyzer2 = new DirectoryAnalyzer<string, MyInt>(converter2, path, MyInt.Default, fileInterpreter2);

            var stats2 = analyzer2.GetTotalStats().ToList();
            stats2.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));
            
            foreach (var (key, value) in stats)
            {
                Console.WriteLine(key + "\t" + value);
            }

            foreach (var (key, value) in stats2)
            {
                Console.WriteLine(key + "\t" + value);
            }
            
			//Console.ReadKey();
		}
	}

	public class ShannonAlgorithmBenchmark
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

	public class MultiplyBenchmark
	{
		// , 16, 32, 64, 128, 256, 512, 1024, 2048, 2048 * 2, 2048 * 2 * 2, 2048 * 2 * 2 * 2
		[Params(2, 96, 147, 3795, 99999)]
		public int MulA { get; set; }

		[Params(2, 96, 147, 3795, 99999)]
		public int MulB { get; set; }

		[Benchmark]
		public int TestMultiply()
		{
			return Multiplication.DoMultiply(MulA, MulB).result;
		}
	}
}