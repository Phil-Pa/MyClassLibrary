using System;
using System.Collections.Generic;
using System.Text;
using MyClassLibrary.TaskScheduling;
using Xunit;

namespace UnitTestProject.TaskScheduling
{
	public class TaskTest
	{

		[Fact]
		public void TestConstructor()
		{
			Task task = new Task("", "", TimeSpan.FromMinutes(5), false, 3, null);

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
				Task task = new Task("", "", TimeSpan.MaxValue, false, 0, null); });

			Assert.Throws<ArgumentException>(() =>
			{
				Task task = new Task("", "", TimeSpan.MaxValue, false, 11, null);
			});

			Assert.Throws<ArgumentException>(() =>
			{
				Task task = new Task("", "", TimeSpan.MaxValue, false, -23, null);
			});

			Assert.Throws<ArgumentException>(() =>
			{
				Task task = new Task("", "", TimeSpan.MaxValue, false, 432, null);
			});
		}

	}
}
