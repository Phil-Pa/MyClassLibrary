using System;
using System.Collections.Generic;
using System.Linq;
using MyClassLibrary.Collections;
// ReSharper disable All

namespace MyClassLibrary.TaskScheduling
{

	public class Duration
	{
		public TimeSpan Time { get; }

		public Duration(TimeSpan time)
		{
			Time = time;
		}
	}

	public class TaskScheduler
	{
		private readonly IGraph<Task, Duration> taskGraph;
		private readonly int _numberWorkers;

		// scheduling information
		private TimeSpan _calculatedDuration = TimeSpan.Zero;

		public TaskScheduler(IEnumerable<Task> tasks, in int numberWorkers)
		{
			_numberWorkers = numberWorkers;

			var nodes = tasks.Select(task => new GraphNode<Task>(task));
			List<IGraphEdge<Task, Duration>> edges = new List<IGraphEdge<Task, Duration>>();

			foreach (var task in tasks)
			{
				foreach (var dependingTask in task.DependingTasks)
				{
					var startNode = nodes.First(node => node.NodeValue!.Equals(task));
					var endNode = nodes.First(node => node.NodeValue!.Equals(dependingTask));
					edges.Add(new GraphEdge<Task, Duration>(startNode, endNode, new Duration(task.Duration)));
				}
			}

			taskGraph = new Graph<Task, Duration>(nodes, edges);
		}

		public IEnumerable<Task> CalculateTaskSequence()
		{
			throw new NotImplementedException();
		}

		public SchedulingInformation GetSchedulingInformation()
		{
			return new SchedulingInformation(_calculatedDuration);
		}
	}
}