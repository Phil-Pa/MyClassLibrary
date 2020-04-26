namespace MyClassLibrary.Collections.Graph
{
	public interface IGraphNode<out T> where T : class
	{
		int Id { get; }
		T? NodeValue { get; }

	}
}