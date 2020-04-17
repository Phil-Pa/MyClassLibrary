using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using MyClassLibrary.TaskScheduling;

namespace UnitTestProject.TaskScheduling
{
	public class TaskSimulationTest
	{

		[Fact]
		public void Test2()
		{
			var task1 = new Task(name: "Task1", description: "Task1 Test", duration: TimeSpan.FromMinutes(20), isParallel: true, priority: 3, dependingTasks: null);
			var task2 = new Task(name: "Task2", description: "Task2 Test", duration: TimeSpan.FromMinutes(10), isParallel: false, priority: 3, dependingTasks: null);

			var tasks = new List<Task> { task1, task2 };
			var duration = TaskScheduler.SimulateTaskExecution(tasks);
			Assert.Equal(TimeSpan.FromMinutes(20), duration);

			task1 = new Task(name: "Task1", description: "Task1 Test", duration: TimeSpan.FromMinutes(20), isParallel: true, priority: 3, dependingTasks: null);
			task2 = new Task(name: "Task2", description: "Task2 Test", duration: TimeSpan.FromMinutes(20), isParallel: false, priority: 3, dependingTasks: null);

			tasks = new List<Task> { task1, task2 };
			duration = TaskScheduler.SimulateTaskExecution(tasks);
			Assert.Equal(TimeSpan.FromMinutes(20), duration);

			task1 = new Task(name: "Task1", description: "Task1 Test", duration: TimeSpan.FromMinutes(20), isParallel: true, priority: 3, dependingTasks: null);
			task2 = new Task(name: "Task2", description: "Task2 Test", duration: TimeSpan.FromMinutes(30), isParallel: false, priority: 3, dependingTasks: null);

			tasks = new List<Task> { task1, task2 };
			duration = TaskScheduler.SimulateTaskExecution(tasks);
			Assert.Equal(TimeSpan.FromMinutes(30), duration);
		}

		[Fact]
		public void Test3()
		{
			var task1 = new Task(name: "Task1", description: "Task1 Test", duration: TimeSpan.FromMinutes(20), isParallel: true, priority: 3, dependingTasks: null);
			var task2 = new Task(name: "Task2", description: "Task2 Test", duration: TimeSpan.FromMinutes(10), isParallel: false, priority: 3, dependingTasks: null);
			var task3 = new Task(name: "Task3", description: "Task3 Test", duration: TimeSpan.FromMinutes(10), isParallel: true, priority: 3, dependingTasks: null);

			var tasks = new List<Task> { task1, task2, task3 };
			var duration = TaskScheduler.SimulateTaskExecution(tasks);
			Assert.Equal(TimeSpan.FromMinutes(20), duration);

			task1 = new Task(name: "Task1", description: "Task1 Test", duration: TimeSpan.FromMinutes(20), isParallel: true, priority: 3, dependingTasks: null);
			task2 = new Task(name: "Task2", description: "Task2 Test", duration: TimeSpan.FromMinutes(20), isParallel: false, priority: 3, dependingTasks: null);
			task3 = new Task(name: "Task3", description: "Task3 Test", duration: TimeSpan.FromMinutes(30), isParallel: true, priority: 3, dependingTasks: null);

			tasks = new List<Task> { task1, task2, task3 };
			duration = TaskScheduler.SimulateTaskExecution(tasks);
			Assert.Equal(TimeSpan.FromMinutes(30), duration);

			task1 = new Task(name: "Task1", description: "Task1 Test", duration: TimeSpan.FromMinutes(20), isParallel: true, priority: 3, dependingTasks: null);
			task2 = new Task(name: "Task2", description: "Task2 Test", duration: TimeSpan.FromMinutes(30), isParallel: false, priority: 3, dependingTasks: null);
			task3 = new Task(name: "Task3", description: "Task3 Test", duration: TimeSpan.FromMinutes(10), isParallel: true, priority: 3, dependingTasks: null);

			tasks = new List<Task> { task1, task2, task3 };
			duration = TaskScheduler.SimulateTaskExecution(tasks);
			Assert.Equal(TimeSpan.FromMinutes(30), duration);
		}

		[Fact]
		public void TestExecuteSimulationSimple()
		{
			var task1 = new Task(name: "Task1", description: "Task1 Test", duration: TimeSpan.FromMinutes(20), isParallel: true, priority: 3, dependingTasks: null);

			var task2 = new Task(name: "Task2", description: "Task2 Test", duration: TimeSpan.FromMinutes(10), isParallel: false, priority: 3, dependingTasks: task1);
			var task3 = new Task(name: "Task3", description: "Task3 Test", duration: TimeSpan.FromMinutes(10), isParallel: true, priority: 3, task1);
			var task4 = new Task(name: "Task4", description: "Task4 Test", duration: TimeSpan.FromMinutes(10), isParallel: false, priority: 3, task2, task3);

			var tasks = new List<Task> { task1, task2, task3, task4 };

			var duration = TaskScheduler.SimulateTaskExecution(tasks);

			Assert.Equal(TimeSpan.FromMinutes(40), duration);
		}

		[Fact]
		public void TestExecuteSimulationComplex()
		{
			var task1 = new Task(name: "Task1", description: "Task1 Test", duration: TimeSpan.FromMinutes(20), isParallel: true, priority: 3, dependingTasks: null);
			var task2 = new Task(name: "Task2", description: "Task2 Test", duration: TimeSpan.FromMinutes(15), isParallel: false, priority: 3, dependingTasks: null);
			var task3 = new Task(name: "Task3", description: "Task3 Test", duration: TimeSpan.FromMinutes(25), isParallel: true, priority: 3, task1);
			var task4 = new Task(name: "Task4", description: "Task4 Test", duration: TimeSpan.FromMinutes(10), isParallel: false, priority: 3, task2);
			var task5 = new Task(name: "Task5", description: "Task5 Test", duration: TimeSpan.FromMinutes(30), isParallel: true, priority: 3, task3);
			var task6 = new Task(name: "Task6", description: "Task6 Test", duration: TimeSpan.FromMinutes(20), isParallel: false, priority: 3, task4);
			var task7 = new Task(name: "Task7", description: "Task7 Test", duration: TimeSpan.FromMinutes(15), isParallel: true, priority: 3, task5, task6);

			var tasks = new List<Task> { task1, task2, task3, task4, task5, task6, task7 };

			var duration = TaskScheduler.SimulateTaskExecution(tasks);

			Assert.Equal(TimeSpan.FromMinutes(135), duration);

			tasks = new List<Task>() { task1, task2, task4, task3, task6, task5, task7 };

			duration = TaskScheduler.SimulateTaskExecution(tasks);

			Assert.Equal(TimeSpan.FromMinutes(95), duration);
		}

	}
}
