using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MyClassLibrary.TaskScheduling
{

	public class TaskScheduler
	{

		private List<Task> _tasks;
		private readonly int _numberWorkers;

		// scheduling information
		private TimeSpan _calculatedDuration = TimeSpan.Zero;

		private bool HasDependentTasks { get; }

		public TaskScheduler(IEnumerable<Task> tasks, in int numberWorkers)
		{
			_tasks = tasks.ToList();
			_numberWorkers = numberWorkers;
			HasDependentTasks = _tasks.Where(t => t.DependingTasks != null).IsNotEmpty();
		}

		public IEnumerable<Task>? CalculateTaskSequence()
		{
			if (_tasks.IsEmpty())
				return null;

			if (HasDependentTasks)
			{
				HandleDependentTasks();
			}
			else
			{
				HandleNotDependentTasks();
			}

			return _tasks;
		}

		private static IDictionary<Task, int> CreateTaskDepthMap(IEnumerable<Task> tasks)
		{
			return tasks.ToDictionary(task => task, task => task.DependingTasksDepth);
		}

		private void HandleDependentTasks()
		{

			var map = CreateTaskDepthMap(_tasks);

			if (map.Values.Distinct().Count() == map.Values.Count)
				HandleSingleDependentTasks(map);
			else
				HandleMultiDependentTasks(map);
		}

		private void HandleSingleDependentTasks(IDictionary<Task, int> map)
		{
			var deepestTaskValue = map.Values.Max();
			Task deepestTask = map.Where((pair, _) => pair.Value == deepestTaskValue).First().Key;

			var stack = new Stack<Task>();

			while (deepestTask.DependingTasks != null)
			{
				stack.Push(deepestTask);
				deepestTask = deepestTask.DependingTasks.First();
			}

			stack.ToList().ForEach(task => { _calculatedDuration += task.Duration; });
			_calculatedDuration += deepestTask.Duration;
		}

		private void HandleMultiDependentTasks(IDictionary<Task, int> map)
		{
			var taskLists = new List<List<Task>>();
			foreach (var depth in map.Values.Distinct())
			{
				var list = new List<Task>();
				var filteredList = map.Where((pair, i) => pair.Value == depth).Select(pair => pair.Key).ToList();
				list.AddRange(filteredList);
				taskLists.Add(list);
			}

			var resultTaskStackListForTaskDepthMap = new List<Task>();
			var resultTaskList = new List<Task>();
			Task? generatedTask = null;
			var generatedNewTask = false;

			foreach (var taskList in taskLists)
			{
				if (taskList.Count == 1)
				{
					Task taskToAdd = taskList.First();
					if (generatedNewTask)
					{
						taskToAdd.AddSingleDependentTask(generatedTask);
						generatedNewTask = false;
					}

					resultTaskList.Add(taskToAdd);
					resultTaskStackListForTaskDepthMap.Add(taskToAdd);
				}
				else
				{
					// clear depending tasks so we dont have recursion
					Task dependingTask = taskList.First().DependingTasks.First();
					foreach (Task task in taskList)
					{
						task.ClearDependingTasks();
					}
					TaskScheduler scheduler = new TaskScheduler(taskList, _numberWorkers);
					var sequence = scheduler.CalculateTaskSequence();

					resultTaskList.AddRange(sequence);

					SchedulingInformation si = scheduler.GetSchedulingInformation();
					generatedTask = new Task("Generated Sub task of " + string.Join(", ", taskList.Select(task => task.Name)), "", si.Duration, false, (int)taskList.Average(t => t.Priority), dependingTask);
					resultTaskStackListForTaskDepthMap.Add(generatedTask);
					generatedNewTask = true;
				}
			}

			var newMap = CreateTaskDepthMap(resultTaskStackListForTaskDepthMap);
			HandleSingleDependentTasks(newMap);

			_tasks = resultTaskList;
		}

		private void HandleNotDependentTasks()
		{
			if (_tasks.All(t => t.IsParallel))
			{
				_calculatedDuration = _tasks.Max(t => t.Duration);
			}
			else if (_tasks.All(t => !t.IsParallel))
			{
				_tasks.ForEach(task => { _calculatedDuration += task.Duration; });
			}
			else
			{
				var parallelTasks = _tasks.Where(t => t.IsParallel);
				TimeSpan maxParallelDuration = parallelTasks.Max(t => t.Duration);
				var notParallelTasks = _tasks.Where(t => !t.IsParallel);
				TimeSpan notParallelDuration = new TimeSpan(notParallelTasks.Sum(t => t.Duration.Ticks));
				if (maxParallelDuration >
				    notParallelDuration)
				{
					_calculatedDuration = maxParallelDuration;
				}
				else if (notParallelDuration >= maxParallelDuration)
				{
					_calculatedDuration = notParallelDuration;
				}

				_tasks = _tasks.OrderBy(task => !task.IsParallel).ToList();
			}
		}

		public SchedulingInformation GetSchedulingInformation()
		{
			return new SchedulingInformation(_calculatedDuration);
		}
	}
}
