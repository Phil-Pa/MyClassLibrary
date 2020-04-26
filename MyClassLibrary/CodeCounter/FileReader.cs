using System.Collections.Generic;
using System.IO;

namespace MyClassLibrary.CodeCounter
{
	public class FileReader : IFileReader
	{
		public IEnumerable<string> ReadLines(string path)
		{
			return File.ReadLines(path);
		}
	}
}