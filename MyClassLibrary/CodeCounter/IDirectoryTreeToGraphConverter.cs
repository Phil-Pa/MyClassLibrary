using System;
using System.Collections.Generic;
using MyClassLibrary.Collections;

namespace MyClassLibrary.CodeCounter
{
	public interface IDirectoryTreeToGraphConverter<T, TV>
	{
		void Convert(string path);

		IEnumerable<IGraphNode<Tuple<string, IDictionary<T, IAddable<TV>>>>> Nodes { get; }
		IEnumerable<IGraphEdge<Tuple<string, IDictionary<T, IAddable<TV>>>, object>> Edges { get; }
	}
}