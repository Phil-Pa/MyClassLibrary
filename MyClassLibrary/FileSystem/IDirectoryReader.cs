﻿using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MyClassLibrary.FileSystem
{
	public interface IDirectoryReader
	{

		// path -> E:/test or path -> E:/test/
		// returns list of directories in the path, not recursive
		IEnumerable<string>? GetDirectories(string path);

		IEnumerable<string>? GetFiles(string path);
	}

	public class DirectoryReader : IDirectoryReader
	{
		public IEnumerable<string>? GetDirectories(string path)
		{
			var dirs = Directory.EnumerateDirectories(path, "*", SearchOption.TopDirectoryOnly);
			var directories = dirs.ToList();
			return directories.Any() ? directories : null;
		}

		public IEnumerable<string>? GetFiles(string path)
		{
			var fs = Directory.EnumerateFiles(path, "*", SearchOption.TopDirectoryOnly);
			var files = fs.Where(file => file != null).ToList();
			return files.Any() ? files : null;
		}
	}
}