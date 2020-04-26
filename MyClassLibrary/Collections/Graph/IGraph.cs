using System.Collections.Generic;

namespace MyClassLibrary.Collections.Graph
{
	public interface IGraph<out T, out TV> where T : class where TV : class
	{
		bool IsDirected { get; }
		IEnumerable<IGraphNode<T>> Nodes { get; }
		IEnumerable<IGraphEdge<T, TV>> Edges { get; }
		bool IsCyclic { get; }
	}
}
