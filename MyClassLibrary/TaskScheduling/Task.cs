using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MyClassLibrary.TaskScheduling
{
	[DebuggerDisplay("{" + nameof(Name) + "}, " + ("{" + nameof(DependingTasksNames) + "}, " + ("{" + nameof(Duration) + "}, IsParallel=" + ("{" + nameof(IsParallel) + "}"))))]
	public class Task : ICloneable
	{
		public string Name { get; }
		public string Description { get; }
		public TimeSpan Duration { get; set; }
		public bool IsParallel { get; }
		public IEnumerable<Task>? DependingTasks { get; private set; }
		public int Priority { get; }

		public bool IsDependingOnOtherTasks {
			get
			{
				if (DependingTasks == null)
					return false;

				return !DependingTasks.IsEmpty();
			}
		}

		private string DependingTasksNames {
			// ReSharper disable once UnusedMember.Local
			get
			{
				if (!IsDependingOnOtherTasks)
					return "No Depending Tasks";

				return "Task depends on " + string.Join(", ", DependingTasks.Select(t => t.Name));
			}
		}

		public int DependingTasksDepth {
			get
			{
				if (DependingTasks == null)
					return 0;

				int depth = 0;
				var task = this;
				while (true)
				{
					task = task.DependingTasks.First();
					depth++;

					if (task.DependingTasks == null)
						break;
				}

				return depth;
			}
		}

		public Task(string name, string description, in TimeSpan duration, in bool isParallel,
			in int priority, params Task[] dependingTasks)
		{
			CheckPriorityRange(priority);

			Name = name;
			Description = description;
			Duration = duration;
			IsParallel = isParallel;
			Priority = priority;
			if (dependingTasks == null)
				DependingTasks = null;
			else
				DependingTasks = dependingTasks.Length == 0 ? null : dependingTasks;
		}

		public void ClearDependingTasks()
		{
			DependingTasks = null;
		}

		private static void CheckPriorityRange(in int priority)
		{
			if (priority <= 0 || priority >= 11)
				throw new ArgumentException("priority must be between 1 and 10, but it is " + priority);
		}

		public void AddSingleDependentTask(Task? task)
		{
			if (task == null)
				return;

			DependingTasks = new List<Task> { task };
		}

		public void RestoreDependingTasks(IEnumerable<Task>? dependentTasks)
		{
			DependingTasks = dependentTasks;
		}

		public object Clone()
		{
			if (DependingTasks == null)
				return new Task(Name, Description, Duration, IsParallel, Priority);
			else
				return new Task(Name, Description, Duration, IsParallel, Priority, new List<Task>(DependingTasks).ToArray());
		}
	}
}