using System.Collections.Generic;
using MyClassLibrary.Collections;
using Xunit;

namespace UnitTestProject.Collections
{

	public class Int
	{
		public int Value { get; set; }

		public Int(int value)
		{
			Value = value;
		}
	}

	public class GraphTest
	{

		[Fact]
		public void TestCyclic()
		{
			GraphUtils.ResetIdCounter();
			
			var node1 = new GraphNode<Int>(new Int(10));
			var node2 = new GraphNode<Int>(new Int(20));
			var node3 = new GraphNode<Int>(new Int(30));
			var node4 = new GraphNode<Int>(new Int(40));
			var node5 = new GraphNode<Int>(new Int(50));

			var nodes = new List<GraphNode<Int>>
			{
				node1, node2, node3, node4, node5
			};

			var edges = new List<IGraphEdge<Int, Int>>
			{
				new GraphEdge<Int, Int>(node1, node2),
				new GraphEdge<Int, Int>(node2, node3),
				new GraphEdge<Int, Int>(node3, node5),
				new GraphEdge<Int, Int>(node5, node2),
				new GraphEdge<Int, Int>(node3, node4),
				new GraphEdge<Int, Int>(node4, node1)
			};

			IGraph<Int, Int> graph = new Graph<Int, Int>(nodes, edges);

			Assert.True(graph.IsDirected);
			Assert.True(graph.IsCyclic);
		}
		
		[Fact]
		public void TestNotCyclic()
		{
			GraphUtils.ResetIdCounter();

			var node1 = new GraphNode<Int>(new Int(10));
			var node2 = new GraphNode<Int>(new Int(20));
			var node3 = new GraphNode<Int>(new Int(30));
			var node4 = new GraphNode<Int>(new Int(40));
			var node5 = new GraphNode<Int>(new Int(50));

			var nodes = new List<GraphNode<Int>>
			{
				node1, node2, node3, node4, node5
			};

			var edges = new List<IGraphEdge<Int, Int>>
			{
				new GraphEdge<Int, Int>(node1, node2),
				new GraphEdge<Int, Int>(node2, node3),
				new GraphEdge<Int, Int>(node3, node5),
				new GraphEdge<Int, Int>(node3, node4),
				new GraphEdge<Int, Int>(node4, node5)
			};

			IGraph<Int, Int> graph = new Graph<Int, Int>(nodes, edges);

			Assert.True(graph.IsDirected);
			Assert.False(graph.IsCyclic);
		}

	}
}
