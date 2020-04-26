namespace MyClassLibrary.Collections.Graph
{
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