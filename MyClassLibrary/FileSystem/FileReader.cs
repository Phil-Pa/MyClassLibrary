using System;
using System.Collections.Generic;
using System.IO;

namespace MyClassLibrary.FileSystem
{
	public class FileReader : IFileReader
	{
		public IList<string>? ReadLines(string path)
        {
            try
            {
                var lines = new List<string>();
                using var reader = new StreamReader(path);
                while (!reader.EndOfStream)
                {
                    lines.Add(reader.ReadLine());
                }

                return lines;
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
	}
}