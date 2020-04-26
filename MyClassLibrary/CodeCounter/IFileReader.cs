using System.Collections.Generic;

namespace MyClassLibrary.CodeCounter
{
	public interface IFileReader
	{

		IEnumerable<string> ReadLines(string path);

	}
}