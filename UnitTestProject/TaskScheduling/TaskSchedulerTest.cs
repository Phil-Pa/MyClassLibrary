using MyClassLibrary.TaskScheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace UnitTestProject.TaskScheduling
{
	public class TaskSchedulerTest
	{
		private const int SingleWorker = 1;

		[Fact]
		public void TestEmptyTaskList()
		{
			// ReSharper disable once CollectionNeverUpdated.Local
			var tasks = new List<Task>();

			var scheduler = new TaskScheduler(tasks, SingleWorker);
			var sequence = scheduler.CalculateTaskSequence();

			var si = scheduler.GetSchedulingInformation();

			Assert.Null(sequence);
			Assert.Equal(TimeSpan.Zero, si.Duration);
		}

		[Fact]
		public void TestOneTask()
		{
			var task = new Task("Task1", "Task1 Test", TimeSpan.FromMinutes(10), false, 3, null);
			var tasks = new List<Task> { task };

			var scheduler = new TaskScheduler(tasks, SingleWorker);
			var sequence = scheduler.CalculateTaskSequence();

			var list = sequence.ToList();

			var si = scheduler.GetSchedulingInformation();

			Assert.Equal(task, list.First());
			Assert.Single(list.ToList());
			Assert.Equal(TimeSpan.FromMinutes(10), si.Duration);
		}

		[Fact]
		public void TestTwoNotParallelTasks()
		{
			var task1 = new Task(name: "Task1", description: "Task1 Test", duration: TimeSpan.FromMinutes(10), isParallel: false, priority: 3, dependingTasks: null);

			var task2 = new Task(name: "Task1", description: "Task1 Test", duration: TimeSpan.FromMinutes(10), isParallel: false, priority: 3, dependingTasks: null);

			var tasks = new List<Task> { task1, task2 };

			var scheduler = new TaskScheduler(tasks, SingleWorker);
			var sequence = scheduler.CalculateTaskSequence();

			var list = sequence.ToList();

			var si = scheduler.GetSchedulingInformation();

			Assert.Equal(task1, list[0]);
			Assert.Equal(task2, list[1]);

			Assert.Equal(2, list.Count);

			Assert.Equal(TimeSpan.FromMinutes(20), si.Duration);
		}

		[Fact]
		public void TestTwoParallelTasks()
		{
			var task1 = new Task(name: "Task1", description: "Task1 Test", duration: TimeSpan.FromMinutes(10), isParallel: true, priority: 3, dependingTasks: null);

			var task2 = new Task(name: "Task1", description: "Task1 Test", duration: TimeSpan.FromMinutes(10), isParallel: true, priority: 3, dependingTasks: null);

			var tasks = new List<Task> { task1, task2 };

			var scheduler = new TaskScheduler(tasks, SingleWorker);
			var sequence = scheduler.CalculateTaskSequence();

			var list = sequence.ToList();

			var si = scheduler.GetSchedulingInformation();

			Assert.Equal(task1, list[0]);
			Assert.Equal(task2, list[1]);

			Assert.Equal(2, list.Count);

			Assert.Equal(TimeSpan.FromMinutes(10), si.Duration);
		}

		[Fact]
		public void TestMixedTasks1()
		{
			var task1 = new Task(name: "Task1", description: "Task1 Test", duration: TimeSpan.FromMinutes(20), isParallel: false, priority: 3, dependingTasks: null);

			var task2 = new Task(name: "Task1", description: "Task1 Test", duration: TimeSpan.FromMinutes(10), isParallel: true, priority: 3, dependingTasks: null);

			var tasks = new List<Task> { task1, task2 };

			var scheduler = new TaskScheduler(tasks, SingleWorker);
			var sequence = scheduler.CalculateTaskSequence();

			var list = sequence.ToList();

			var si = scheduler.GetSchedulingInformation();

			Assert.Equal(task2, list[0]);
			Assert.Equal(task1, list[1]);

			Assert.Equal(2, list.Count);

			Assert.Equal(TimeSpan.FromMinutes(20), si.Duration);
		}

		[Fact]
		public void TestMixedTasks2()
		{
			var task1 = new Task(name: "Task1", description: "Task1 Test", duration: TimeSpan.FromMinutes(20), isParallel: false, priority: 3, dependingTasks: null);

			var task2 = new Task(name: "Task1", description: "Task1 Test", duration: TimeSpan.FromMinutes(10), isParallel: true, priority: 3, dependingTasks: null);

			var tasks = new List<Task> { task1, task2 };

			var scheduler = new TaskScheduler(tasks, SingleWorker);
			var sequence = scheduler.CalculateTaskSequence();

			var list = sequence.ToList();

			var si = scheduler.GetSchedulingInformation();

			Assert.Equal(task1, list[1]);
			Assert.Equal(task2, list[0]);

			Assert.Equal(2, list.Count);

			Assert.Equal(TimeSpan.FromMinutes(20), si.Duration);
		}

		[Fact]
		public void TestSingleDependentTask()
		{
			var task1 = new Task(name: "Task1", description: "Task1 Test", duration: TimeSpan.FromMinutes(20), isParallel: true, priority: 3, dependingTasks: null);

			var task2 = new Task(name: "Task2", description: "Task2 Test", duration: TimeSpan.FromMinutes(10), isParallel: false, priority: 3, dependingTasks: task1);
			var task3 = new Task(name: "Task3", description: "Task3 Test", duration: TimeSpan.FromMinutes(10), isParallel: true, priority: 3, task2);
			var task4 = new Task(name: "Task4", description: "Task4 Test", duration: TimeSpan.FromMinutes(10), isParallel: false, priority: 3, task3);

			var tasks = new List<Task> { task1, task2, task3, task4 };

			var scheduler = new TaskScheduler(tasks, SingleWorker);
			var sequence = scheduler.CalculateTaskSequence();

			var list = sequence.ToList();

			var si = scheduler.GetSchedulingInformation();

			Assert.Equal(task1, list[0]);
			Assert.Equal(task2, list[1]);
			Assert.Equal(task3, list[2]);
			Assert.Equal(task4, list[3]);

			Assert.Equal(4, list.Count);

			Assert.Equal(TimeSpan.FromMinutes(50), si.Duration);
		}

		[Fact]
		public void TestComplexDependentTask()
		{
			var task1 = new Task(name: "Task1", description: "Task1 Test", duration: TimeSpan.FromMinutes(20), isParallel: true, priority: 3, dependingTasks: null);

			var task2 = new Task(name: "Task2", description: "Task2 Test", duration: TimeSpan.FromMinutes(10), isParallel: false, priority: 3, dependingTasks: task1);
			var task3 = new Task(name: "Task3", description: "Task3 Test", duration: TimeSpan.FromMinutes(10), isParallel: true, priority: 3, task1);
			var task4 = new Task(name: "Task4", description: "Task4 Test", duration: TimeSpan.FromMinutes(10), isParallel: false, priority: 3, task2, task3);

			var tasks = new List<Task> { task1, task2, task3, task4 };

			var scheduler = new TaskScheduler(tasks, SingleWorker);
			var sequence = scheduler.CalculateTaskSequence();

			var list = sequence.ToList();

			var si = scheduler.GetSchedulingInformation();

			Assert.Equal(task1, list[0]);
			Assert.Equal(task3, list[1]);
			Assert.Equal(task2, list[2]);
			Assert.Equal(task4, list[3]);

			Assert.Equal(4, list.Count);

			Assert.Equal(TimeSpan.FromMinutes(40), si.Duration);
		}

		[Fact]
		public void TestMoreComplexDependentTask()
		{
			var task1 = new Task(name: "Task1", description: "Task1 Test", duration: TimeSpan.FromMinutes(20), isParallel: true, priority: 3, dependingTasks: null);
			var task2 = new Task(name: "Task2", description: "Task2 Test", duration: TimeSpan.FromMinutes(15), isParallel: false, priority: 3, dependingTasks: null);
			var task3 = new Task(name: "Task3", description: "Task3 Test", duration: TimeSpan.FromMinutes(25), isParallel: true, priority: 3, task1);
			var task4 = new Task(name: "Task4", description: "Task4 Test", duration: TimeSpan.FromMinutes(10), isParallel: false, priority: 3, task2);
			var task5 = new Task(name: "Task5", description: "Task5 Test", duration: TimeSpan.FromMinutes(30), isParallel: true, priority: 3, task3);
			var task6 = new Task(name: "Task6", description: "Task6 Test", duration: TimeSpan.FromMinutes(20), isParallel: false, priority: 3, task4);
			var task7 = new Task(name: "Task7", description: "Task7 Test", duration: TimeSpan.FromMinutes(15), isParallel: true, priority: 3, task5, task6);

			var tasks = new List<Task> { task1, task2, task3, task4, task5, task6, task7 };

			var scheduler = new TaskScheduler(tasks, SingleWorker);
			var sequence = scheduler.CalculateTaskSequence();

			var list = sequence.ToList();

			var si = scheduler.GetSchedulingInformation();

			Assert.Equal(task1, list[0]);
			Assert.Equal(task2, list[1]);
			Assert.Equal(task3, list[2]);
			Assert.Equal(task4, list[3]);
			Assert.Equal(task5, list[4]);
			Assert.Equal(task6, list[5]);
			Assert.Equal(task7, list[6]);

			int minutesToCompleteTaskList = System.Math.Min(15 + 10 + 15 + 5 + 30 + 15, 15 + 5 + 10 + 20 + 30 + 15); // = 90 15+5+10+20+30+15 = 95

			Assert.Equal(7, list.Count);

			Assert.Equal(TimeSpan.FromMinutes(minutesToCompleteTaskList), si.Duration);
		}
	}
}