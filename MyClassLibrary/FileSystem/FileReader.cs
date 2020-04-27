using System.Collections.Generic;
using System.IO;

namespace MyClassLibrary.FileSystem
{
	public class FileReader : IFileReader
	{
		public IList<string> ReadLines(string path)
        {
            var lines = new List<string>();
            using StreamReader reader = new StreamReader(path);
            while (!reader.EndOfStream)
            {
                lines.Add(reader.ReadLine());
            }
            
            return lines;
        }
	}
}