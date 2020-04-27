using System.Collections.Generic;

namespace MyClassLibrary.FileSystem
{
	public interface IFileReader
	{

		IList<string> ReadLines(string path);

	}
}