using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MyClassLibrary.TaskScheduling
{
	[DebuggerDisplay("Id=" + "{" + nameof(Id) + "}, " + "{" + nameof(Name) + "}, " + "{" + nameof(DependingTasksNames) + "}, " + "{" + nameof(Duration) + "}, IsParallel=" + "{" + nameof(IsParallel) + "}")]
	public class Task : ICloneable, IEquatable<Task>
	{
		private static int _idCounter;
		public int Id { get; }
		public string Name { get; }
		public string Description { get; }
		public TimeSpan Duration { get; set; }
		public bool IsParallel { get; }
		public IEnumerable<Task> DependingTasks { get; private set; }
		public int Priority { get; }

		public bool IsDependingOnOtherTasks {
			get
			{
				return DependingTasks.IsNotEmpty();
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

		public Task(string name, string description, in TimeSpan duration, in bool isParallel,
			in int priority, IEnumerable<Task>? dependingTasks)
		{
			CheckPriorityRange(priority);
			Id = _idCounter++;
			Debug.WriteLine("Task Id = " + Id);
			Name = name;
			Description = description;
			Duration = duration;
			IsParallel = isParallel;
			Priority = priority;
			DependingTasks = dependingTasks ?? new List<Task>();
		}

		public void ClearDependingTasks()
		{
			DependingTasks = new List<Task>();
		}

		private static void CheckPriorityRange(in int priority)
		{
			if (priority <= 0 || priority >= 11)
				throw new ArgumentException("priority must be between 1 and 10, but it is " + priority);
		}

		public void AddSingleDependentTask(Task task)
		{

			DependingTasks = new List<Task> { task };
		}

		public void SetDependingTasks(IEnumerable<Task> dependentTasks)
		{
			DependingTasks = dependentTasks;
		}

		public object Clone()
		{
			return DependingTasks == null
				? new Task(Name, Description, Duration, IsParallel, Priority, null)
				: new Task(Name, Description, Duration, IsParallel, Priority, new List<Task>(DependingTasks).ToArray());
		}

		public bool Equals(Task? other)
		{
			if (other is null) return false;
			if (ReferenceEquals(this, other)) return true;
			return Id == other.Id;
		}

		public override bool Equals(object? obj)
		{
			if (obj is null) return false;
			if (ReferenceEquals(this, obj)) return true;
			return obj.GetType() == GetType() && Equals((Task) obj);
		}

		public override int GetHashCode()
		{
			return Id;
		}
	}
}