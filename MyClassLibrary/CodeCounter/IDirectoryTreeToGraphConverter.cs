using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using MyClassLibrary.Collections.Graph;

namespace MyClassLibrary.CodeCounter
{
	public interface IDirectoryTreeToGraphConverter<T, TV>
	{
		// TODO: should we pass the file interpreter via convert or with ctor of deriving classes?
		void Convert(string path, IFileInterpreter<TV> fileInterpreter);

		IEnumerable<IGraphNode<Tuple<string, IDictionary<T, IAddable<TV>>>>> Nodes { get; }
		IEnumerable<IGraphEdge<Tuple<string, IDictionary<T, IAddable<TV>>>, object>> Edges { get; }
	}

	public class DirectoryTreeToGraphConverter : IDirectoryTreeToGraphConverter<Language, CodeStats>
	{
		private readonly IFileReader _fileReader;
		private readonly IDirectoryReader _directoryReader;
		private readonly List<IGraphNode<Tuple<string, IDictionary<Language, IAddable<CodeStats>>>>> _nodes;
		private readonly List<IGraphEdge<Tuple<string, IDictionary<Language, IAddable<CodeStats>>>, object>> _edges;
		private IFileInterpreter<CodeStats> _fileInterpreter;

		public DirectoryTreeToGraphConverter(IFileReader fileReader, IDirectoryReader directoryReader)
		{
			_fileReader = fileReader;
			_directoryReader = directoryReader;
			_nodes = new List<IGraphNode<Tuple<string, IDictionary<Language, IAddable<CodeStats>>>>>();
			_edges = new List<IGraphEdge<Tuple<string, IDictionary<Language, IAddable<CodeStats>>>, object>>();
		}

		public void Convert(string path, IFileInterpreter<CodeStats> fileInterpreter)
		{
			_fileInterpreter = fileInterpreter;
			ConvertInternal(path);
			CreateEdges();
		}

		private void CreateEdges()
		{
			
		}

		private void ConvertInternal(string path)
		{
			var dirs = _directoryReader.GetDirectories(path);

			if (dirs == null)
				return;

			foreach (var dir in dirs)
			{
				ConvertInternal(dir);
				ProcessDirectory(dir);
			}
		}

		private void ProcessDirectory(string dir)
		{
			var files = _directoryReader.GetFiles(dir).Where(file => file.GetFileExtension() != null);
			if (files == null)
				return;
			
			var map = new Dictionary<Language, IAddable<CodeStats>>();

			foreach (var file in files)
			{
				ProcessFile(file, map);
			}

			var tuple = new Tuple<string, IDictionary<Language, IAddable<CodeStats>>>(dir, map);
			_nodes.Add(new GraphNode<Tuple<string, IDictionary<Language, IAddable<CodeStats>>>>(tuple));
		}

		private void ProcessFile(string file, IDictionary<Language, IAddable<CodeStats>> map)
		{
			// TODO: extract dependency
			Language language;
			var fileExtension = file.GetFileExtension();
			Debug.Assert(fileExtension != null);

			switch (fileExtension)
			{
				case ".c":
					language = Language.C;
					break;
				case ".h":
				case ".hpp":
					language = Language.CCppHeader;
					break;
				case ".java":
					language = Language.Java;
					break;
				case ".cs":
					language = Language.CSharp;
					break;
				case ".cpp":
					language = Language.Cpp;
					break;
				default:
					throw new Exception();
			}

			var lines = _fileReader.ReadLines(file);
			IAddable<CodeStats> stats = _fileInterpreter.Interpret(lines);
			map.Add(language, stats);
		}

		public IEnumerable<IGraphNode<Tuple<string, IDictionary<Language, IAddable<CodeStats>>>>> Nodes => _nodes;

		public IEnumerable<IGraphEdge<Tuple<string, IDictionary<Language, IAddable<CodeStats>>>, object>> Edges =>
			_edges;
	}
}