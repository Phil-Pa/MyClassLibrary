using MyClassLibrary.Time;
using System;
using System.Collections.Generic;
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
			// TODO: handle multiple workers

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
			var taskDepthMap = CreateTaskDepthMap(_tasks);
			// TODO: could be wrong
			if (taskDepthMap.Values.Distinct().Count() == taskDepthMap.Values.Count)
			{
				HandleSingleDependentTasks(taskDepthMap);
			}
			else
			{
				// TODO: good implementation
				// HandleMultiDependentTasks(taskDepthMap);

				HandleTasksUsingPermutations();
			}
		}

		private void HandleTasksUsingPermutations()
		{
			var listOfTasks = _tasks.GetPermutations(_tasks.Count);
			var ofTasks = listOfTasks as IEnumerable<Task>[] ?? listOfTasks.ToArray();
			int count = ofTasks.Count();

			TimeSpan duration = TimeSpan.MaxValue;
			IEnumerable<Task> tasksResult = _tasks;

			for (int i = 0; i < count; i++)
			{
				var list = ofTasks.ElementAt(i);
				var enumerable = list as Task[] ?? list.ToArray();
				if (!SatifiesDependencies(enumerable))
					continue;

				var listDuration = SimulateTaskExecution(enumerable);
				duration = TimeUtils.Min(duration, listDuration);
				tasksResult = enumerable;
			}

			if (Equals(tasksResult, _tasks))
			{
				// error, found nothing
				throw new Exception();
			}

			_tasks = tasksResult.ToList();
			_calculatedDuration = duration;
		}

		/// <summary>
		/// Note: does not check for correct task execution, for example depending tasks
		/// </summary>
		/// <param name="tasks"></param>
		/// <returns></returns>
		public static TimeSpan SimulateTaskExecution(IEnumerable<Task> tasks)
		{
			TimeSpan duration = TimeSpan.Zero;

			var enumerable = tasks.ToList();
			if (enumerable.Any(task => task.IsDependingOnOtherTasks))
				return duration;

			var array = enumerable.ToArray();

			Array.Sort(array, (a, b) => b.IsParallel.CompareTo(a.IsParallel));

			var parallelTasks = new List<Task>();

			foreach (var task in array)
			{
				// if we dont have any parallel task running

				// if we have a parallel task running

				// if we encounter a parallel task while we are doing a parallel task

				if (task.IsParallel)
				{
					parallelTasks.Add(task);
				}
				else
				{
					duration += task.Duration;
					for (int i = 0; i < parallelTasks.Count; i++)
					{
						var pt = parallelTasks[i];
						pt.Duration -= task.Duration;
						if (pt.Duration > TimeSpan.Zero)
							continue;

						parallelTasks.RemoveAt(i);
						i--;
					}
				}
			}

			parallelTasks.ForEach(task => duration += task.Duration);

			return duration;
		}

		private static bool SatifiesDependencies(IEnumerable<Task> tasks)
		{
			var enumerable = tasks.ToList();
			int count = enumerable.Count();
			List<Task> tasksDone = new List<Task>();
			for (int i = 0; i < count; i++)
			{
				Task task = enumerable.ElementAt(i);
				if (task.IsDependingOnOtherTasks)
				{
					if (!tasksDone.ContainsAll(task.DependingTasks!))
						return false;
				}
				tasksDone.Add(task);
			}

			return true;
		}

		private void HandleSingleDependentTasks(IDictionary<Task, int> map)
		{
			int deepestTaskValue = map.Values.Max();

			// check if multiple tasks have the deepest task value. usually, this should not be the case but if, then take the last task of them

			bool takeLastDeepestTask = map.Values.Count(depth => depth == deepestTaskValue) > 1;

			var deepestTask = takeLastDeepestTask ? map.Where((pair, _) => pair.Value == deepestTaskValue).Last().Key : map.Where((pair, _) => pair.Value == deepestTaskValue).First().Key;

			if (takeLastDeepestTask)
			{
				map[deepestTask]++;
			}

			var stack = new Stack<Task>();

			while (deepestTask.DependingTasks != null)
			{
				stack.Push(deepestTask);
				deepestTask = deepestTask.DependingTasks.First();
			}

			// calculate duration
			_tasks.ForEach(t => { _calculatedDuration += t.Duration; });
		}

		private void HandleMultiDependentTasks(IDictionary<Task, int> taskDepthMap)
		{
			var taskLists = JoinToTasksLists(taskDepthMap);

			var resultTaskStackListForTaskDepthMap = new List<Task>();
			var resultTaskList = new List<Task>();
			Task? generatedTask = null;
			bool generatedNewTask = false;

			foreach (var taskList in taskLists)
			{
				if (taskList.All(t => !t.IsDependingOnOtherTasks))
				{
					var tasksToAdd = taskList;
					if (generatedNewTask)
					{
						foreach (var task in tasksToAdd)
						{
							task.AddSingleDependentTask(generatedTask);
						}
						generatedNewTask = false;
					}

					resultTaskList.AddRange(tasksToAdd);
					resultTaskStackListForTaskDepthMap.AddRange(tasksToAdd);
				}
				else
				{
					// clear depending tasks so we don't have recursion
					var dependingTask = taskList.First().DependingTasks.First();

					var dependentTaskMap = new Dictionary<Task, IEnumerable<Task>?>();

					foreach (var task in taskList)
					{
						// cleared tasks are restored for later
						dependentTaskMap.Add(task, task.DependingTasks);

						task.ClearDependingTasks();
					}
					var scheduler = new TaskScheduler(taskList, _numberWorkers);
					var sequence = scheduler.CalculateTaskSequence() ?? throw new Exception("sequence should not be null");

					var list = sequence.ToList();

					foreach (var task in list)
					{
						task.RestoreDependingTasks(dependentTaskMap[task]);
					}

					resultTaskList.AddRange(list);

					if (generatedNewTask)
						dependingTask = resultTaskList.Last();

					var si = scheduler.GetSchedulingInformation();
					generatedTask = new Task("Generated Sub task of " + string.Join(", ", taskList.Select(task => task.Name)), "", si.Duration, false, (int)taskList.Average(t => t.Priority), dependingTask);
					resultTaskStackListForTaskDepthMap.Add(generatedTask);
					generatedNewTask = true;
				}
			}

			var newMap = CreateTaskDepthMap(resultTaskStackListForTaskDepthMap);
			//HandleSingleDependentTasks(newMap);
			_tasks = resultTaskList;

			// duration calculation has to be after HandleSingleDependentTasks, because we want to overwrite the calculated duration
			resultTaskStackListForTaskDepthMap.ForEach(t => { _calculatedDuration += t.Duration; });
		}

		private static IEnumerable<List<Task>> JoinToTasksLists(IDictionary<Task, int> map)
		{
			var taskLists = new List<List<Task>>();
			foreach (int depth in map.Values.Distinct())
			{
				var list = new List<Task>();
				var filteredList = map.Where((pair, i) => pair.Value == depth).Select(pair => pair.Key).ToList();
				list.AddRange(filteredList);
				taskLists.Add(list);
			}

			return taskLists;
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
				// split parallel and non parallel

				var parallelTasks = _tasks.Where(t => t.IsParallel);
				var maxParallelDuration = parallelTasks.Max(t => t.Duration);

				var notParallelTasks = _tasks.Where(t => !t.IsParallel);
				var notParallelDuration = new TimeSpan(notParallelTasks.Sum(t => t.Duration.Ticks));

				_calculatedDuration = TimeUtils.Max(maxParallelDuration, notParallelDuration);

				_tasks = _tasks.OrderBy(task => !task.IsParallel).ToList();
			}
		}

		public SchedulingInformation GetSchedulingInformation()
		{
			return new SchedulingInformation(_calculatedDuration);
		}
	}
}