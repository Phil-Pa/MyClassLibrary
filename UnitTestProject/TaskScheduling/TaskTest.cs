using MyClassLibrary.TaskScheduling;
using System;
using Xunit;
// ReSharper disable UnusedVariable

namespace UnitTestProject.TaskScheduling
{
	public class TaskTest
	{
		[Fact]
		public void TestConstructor()
		{
			var task = new Task("", "", TimeSpan.FromMinutes(5), false, 3, null);

			Assert.Equal("", task.Name);
			Assert.Equal("", task.Description);
			Assert.Equal(TimeSpan.FromMinutes(5), task.Duration);
			Assert.False(task.IsParallel);
			Assert.Null(task.DependingTasks);
			Assert.Equal(3, task.Priority);
		}

		[Fact]
		public void TestPriorityBetween1And10()
		{
			Assert.Throws<ArgumentException>(() =>
			{
				var task = new Task("", "", TimeSpan.MaxValue, false, 0, null);
			});


			Assert.Throws<ArgumentException>(() =>
			{
				var task = new Task("", "", TimeSpan.MaxValue, false, 11, null);
			});

			Assert.Throws<ArgumentException>(() =>
			{
				var task = new Task("", "", TimeSpan.MaxValue, false, -23, null);
			});

			Assert.Throws<ArgumentException>(() =>
			{
				var task = new Task("", "", TimeSpan.MaxValue, false, 432, null);
			});
		}

		[Fact]
		public void TestTaskDepth()
		{
			var task1 = new Task(name: "Task1", description: "Task1 Test", duration: TimeSpan.FromMinutes(20), isParallel: false, priority: 3, dependingTasks: null);
			var task2 = new Task(name: "Task2", description: "Task2 Test", duration: TimeSpan.FromMinutes(15), isParallel: false, priority: 3, dependingTasks: null);
			var task3 = new Task(name: "Task3", description: "Task3 Test", duration: TimeSpan.FromMinutes(25), isParallel: true, priority: 3, task1);
			var task4 = new Task(name: "Task4", description: "Task4 Test", duration: TimeSpan.FromMinutes(10), isParallel: false, priority: 3, task2);
			var task5 = new Task(name: "Task5", description: "Task5 Test", duration: TimeSpan.FromMinutes(30), isParallel: true, priority: 3, task1);
			var task6 = new Task(name: "Task6", description: "Task6 Test", duration: TimeSpan.FromMinutes(20), isParallel: false, priority: 3, task2, task5);

			Assert.Equal(2, task6.DependingTasksDepth);
		}
	}
}