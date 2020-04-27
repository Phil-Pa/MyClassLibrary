using System.Collections.Generic;
using MyClassLibrary.FileSystem.CodeCounter;

namespace MyClassLibrary.FileSystem
{
	public interface IFileInterpreter<T, TV>
	{
		(T, IAddable<TV>)? Interpret(string fileExtension, IList<string> lines);
		bool SupportsFileExtension(string fileExtension);
	}
}