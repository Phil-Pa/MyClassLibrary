using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace MyClassLibrary.Collections.Graph
{
	[SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
	public class Graph<T, TV> : IGraph<T, TV> where T : class where TV : class
	{
		
		private const int Black = 2, Gray = 1, White = 0;

		public bool IsDirected { get; private set; }
		public IEnumerable<IGraphNode<T>> Nodes { get; }
		public IEnumerable<IGraphEdge<T, TV>> Edges { get; }
		public bool IsCyclic { get; private set; }
		private List<List<int>> _adjacencyMatrix;
		public Graph(IEnumerable<IGraphNode<T>> nodes, IEnumerable<IGraphEdge<T, TV>> edges)
		{
			Nodes = nodes;
			Edges = edges;
			_adjacencyMatrix = new List<List<int>>();

			Validate();
			// TODO: Analyze();
		}

		private void Validate()
		{
			// edges contain only nodes in the nodes list

			var nodes = Nodes.ToList();

			foreach (var graphEdge in Edges)
			{
				if (nodes.Contains(graphEdge.StartNode) || nodes.Contains(graphEdge.EndNode))
				{

				}
				else
				{
					throw new ArgumentException();
				}
			}

			// unique ids
			if (Nodes.Select(node => node.Id).Distinct().Count() != Nodes.Count())
			{
				throw new ArgumentException();
			}
		}

		private void Analyze()
		{
			AnalyzeCycles();
			AnalyzeDirected();
		}

		private void AnalyzeCycles()
		{
			_adjacencyMatrix = CreateAdjacencyMatrix();

			int[] color = new int[Nodes.Count()];
			
			for (var i = 0; i < color.Length; i++)
				color[i] = White;

			if (color.Where((t, i) => t == White && IsCyclicUtil(i, color)).Any())
			{
				IsCyclic = true;
				return;
			}

			IsCyclic = false;
		}

		private bool IsCyclicUtil(in int i, IList<int> color)
		{
			color[i] = Gray;

			foreach (var k in _adjacencyMatrix[i])
			{
				switch (color[k])
				{
					case Gray:
					case White when IsCyclicUtil(k, color):
						return true;
				}
			}

			color[i] = Black;
			return false;
		}

		private List<List<int>> CreateAdjacencyMatrix()
		{
			var dimension = Edges.Count();
			List<List<int>> matrix = new List<List<int>>();

			for (var i = 0; i < dimension; i++)
			{
				matrix.Add(new List<int>());
			}

			foreach (var graphEdge in Edges)
			{
				matrix[graphEdge.EndNode.Id].Add(graphEdge.StartNode.Id);
			}

			return matrix;
		}

		private void AnalyzeDirected()
		{
			foreach (var graphEdge in Edges)
			{
				foreach (var reversedEdge in Edges)
				{
					if (graphEdge.StartNode == reversedEdge.EndNode && graphEdge.EndNode == reversedEdge.StartNode)
					{
						IsDirected = false;
					}
					else
					{
						IsDirected = true;
						return;
					}
				}
			}
		}
	}
}