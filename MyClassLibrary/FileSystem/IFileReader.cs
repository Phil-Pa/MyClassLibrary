using System.Collections.Generic;

namespace MyClassLibrary.FileSystem
{
	public interface IFileReader
	{

		IEnumerable<string> ReadLines(string path);

	}
}