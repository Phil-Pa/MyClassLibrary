using System.Collections.Generic;

namespace MyClassLibrary.CodeCounter
{
	public interface IFileInterpreter<T, TV>
	{
		(T, IAddable<TV>)? Interpret(string fileExtension, IEnumerable<string> lines);
		bool SupportsFileExtension(string fileExtension);
	}
}