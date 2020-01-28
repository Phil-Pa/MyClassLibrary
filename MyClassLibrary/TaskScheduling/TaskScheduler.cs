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

			// check if multiple tasks have the deepest task value. usually, this should not be the case but if, then take the last task of them

			var takeLastDeepestTask = map.Values.Count(depth => depth == deepestTaskValue) > 1;

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

		private void HandleMultiDependentTasks(IDictionary<Task, int> map)
		{
			var taskLists = JoinToTasksLists(map);

			var resultTaskStackListForTaskDepthMap = new List<Task>();
			var resultTaskList = new List<Task>();
			Task? generatedTask = null;
			var generatedNewTask = false;

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
					Task dependingTask = taskList.First().DependingTasks.First();

					var dependentTaskMap = new Dictionary<Task, IEnumerable<Task>?>();

					foreach (Task task in taskList)
					{
						// cleared tasks are restored for later
						dependentTaskMap.Add(task, task.DependingTasks);

						task.ClearDependingTasks();
					}
					TaskScheduler scheduler = new TaskScheduler(taskList, _numberWorkers);
					var sequence = scheduler.CalculateTaskSequence() ?? throw new Exception("sequence should not be null");

					var list = sequence.ToList();

					foreach (Task task in list)
					{
						task.RestoreDependingTasks(dependentTaskMap[task]);
					}

					resultTaskList.AddRange(list);

					if (generatedNewTask)
						dependingTask = resultTaskList.Last();

					SchedulingInformation si = scheduler.GetSchedulingInformation();
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
			foreach (var depth in map.Values.Distinct())
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
