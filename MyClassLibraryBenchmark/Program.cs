using MyClassLibrary.Algorithms.AStar;
using System;
using System.Diagnostics;
using System.Linq;
using BenchmarkDotNet.Running;
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

        private static void RunBenchmarks()
        {
            BenchmarkRunner.Run<ShannonAlgorithmBenchmark>();
            BenchmarkRunner.Run<MultiplyBenchmark>();
        }
	}
}