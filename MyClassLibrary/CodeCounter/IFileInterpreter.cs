using System.Collections.Generic;

namespace MyClassLibrary.CodeCounter
{
	public interface IFileInterpreter<T>
	{
		IAddable<T> Interpret(IEnumerable<string> lines);
	}
}