using System.Diagnostics;

namespace MyClassLibrary.Collections.Graph
{
	public class GraphEdge<T, TV> : IGraphEdge<T, TV> where T : class where TV : class
	{
		public IGraphNode<T> StartNode { get; }
		public IGraphNode<T> EndNode { get; }
		public TV? EdgeValue { get; }
        public bool HasConnectionBetweenNodes(int idNode1, int idNode2)
        {
            return StartNode.Id == idNode1 && EndNode.Id == idNode2;
        }

        public GraphEdge(IGraphNode<T> startNode, IGraphNode<T> endNode, TV? edgeValue = null)
		{
			Debug.WriteLine("Created edge with nodes with ids: " + startNode.Id + ", " + endNode.Id);

			StartNode = startNode;
			EndNode = endNode;
			EdgeValue = edgeValue;
		}
	}
}