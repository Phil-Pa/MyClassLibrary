namespace MyClassLibrary.Collections.Graph
{
	public interface IGraphEdge<out T, out TV> where T : class where TV : class
	{
		IGraphNode<T> StartNode { get; }
		IGraphNode<T> EndNode { get; }
		TV? EdgeValue { get; }

        bool HasConnectionBetweenNodes(int idNode1, int idNode2);
    }
}