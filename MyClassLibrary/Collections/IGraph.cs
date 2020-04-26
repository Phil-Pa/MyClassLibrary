using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MyClassLibrary.Collections
{
	public interface IGraph<out T, out TV> where T : class where TV : class
	{
		bool IsDirected { get; }
		IEnumerable<IGraphNode<T>> Nodes { get; }
		IEnumerable<IGraphEdge<T, TV>> Edges { get; }
		bool IsCyclic { get; }
	}

	public interface IGraphEdge<out T, out TV> where T : class where TV : class
	{
		IGraphNode<T> StartNode { get; }
		IGraphNode<T> EndNode { get; }
		TV? EdgeValue { get; }
	}

	public interface IGraphNode<out T> where T : class
	{
		int Id { get; }
		T? NodeValue { get; }

	}

	public class GraphEdge<T, TV> : IGraphEdge<T, TV> where T : class where TV : class
	{
		public IGraphNode<T> StartNode { get; }
		public IGraphNode<T> EndNode { get; }
		public TV? EdgeValue { get; }

		public GraphEdge(IGraphNode<T> startNode, IGraphNode<T> endNode, TV? edgeValue = null)
		{

			Debug.WriteLine("Created edge with nodes with ids: " + startNode.Id + ", " + endNode.Id);

			StartNode = startNode;
			EndNode = endNode;
			EdgeValue = edgeValue;
		}
	}

	public static class GraphUtils
	{
		private static readonly Dictionary<Type, int> IdCounterMap = new Dictionary<Type, int>();

		/// <summary>
		/// Generates an id for an object typ <paramref name="type"/> starting with id 0
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static int GenerateId(Type type)
		{
			if (!IdCounterMap.ContainsKey(type))
			{
				IdCounterMap.Add(type, 0);
			}

			return IdCounterMap[type]++;
		}

		/// <summary>
		/// Resets the id counter for every type
		/// </summary>
		public static void ResetIdCounter()
		{
			IdCounterMap.Clear();
		}
	}

	public class GraphNode<T> : IGraphNode<T> where T : class
	{
		public int Id { get; }
		public T? NodeValue { get; }

		public GraphNode(T? nodeValue = null)
		{
			Id = GraphUtils.GenerateId(typeof(T));
			NodeValue = nodeValue;
		}
	}
}
