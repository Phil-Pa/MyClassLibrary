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

        public Lazy<bool> IsCyclic { get; }

        public Graph(IEnumerable<IGraphNode<T>> nodes, IEnumerable<IGraphEdge<T, TV>> edges)
		{
			Nodes = nodes;
			Edges = edges;

            IsCyclic = new Lazy<bool>(AnalyzeCycles);

            Validate();
            Analyze();
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
					throw new ArgumentException("nodes in a graph edge must be in the nodes list");
				}
			}

			// unique ids
			if (Nodes.Select(node => node.Id).Distinct().Count() != Nodes.Count())
			{
				throw new ArgumentException("nodes must have unique ids");
			}
		}

		private void Analyze()
		{
            AnalyzeDirected();
		}

		private bool AnalyzeCycles()
		{ 
            var matrix = CreateAdjacencyMatrix();

			var tarjan = new Tarjan(matrix);
            return tarjan.countStronglyConnectedComponents() != Nodes.Count();
        }

        private bool[][] CreateAdjacencyMatrix()
		{
			var dimension = Nodes.Count();
			bool[][] adjacencyMatrix = new bool[dimension][];

            for (var i = 0; i < dimension; i++)
            {

                adjacencyMatrix[i] = new bool[dimension];
                
                for (var j = 0; j < dimension; j++)
                {
                    var connected = Edges.Any(edge => edge.HasConnectionBetweenNodes(i, j));

                    adjacencyMatrix[i][j] = connected;
                }
            }

            return adjacencyMatrix;
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

        private sealed class Tarjan
        {

            private int n, pre, count = 0;
            private int[] id, low;
            private bool[] marked;
            private bool[][] adj;
            private Stack<int> stack = new Stack<int>();

            // Tarjan input requires an NxN adjacency matrix
            public Tarjan(bool[][] adj) {
                n = adj.Length;
                this.adj = adj;
                marked = new bool[n];
                id = new int[n];
                low = new int[n];
                for (int u = 0; u < n; u++) if (!marked[u]) dfs(u);
            }

            private void dfs(int u) {
                marked[u] = true;
                low[u] = pre++;
                int min = low[u];
                stack.Push(u);
                int v;
                for (v = 0; v < n; v++) {
                    if (adj[u][v]) {
                        if (!marked[v]) dfs(v);
                        if (low[v] < min) min = low[v];
                    }
                }
                if (min < low[u]) {
                    low[u] = min;
                    return;
                }

                v = 0;
                do {
                    v = stack.Pop();
                    id[v] = count;
                    low[v] = n;
                } while (v != u);
                count++;
            }

            // Returns the id array with the strongly connected components.
            // If id[i] == id[j] then nodes i and j are part of the same strongly connected component.
            public int[] getStronglyConnectedComponents()
            {
                return id;
            }

            // Returns the number of strongly connected components in this graph
            public int countStronglyConnectedComponents() {
                return count;
            }
            
        }
	}
}