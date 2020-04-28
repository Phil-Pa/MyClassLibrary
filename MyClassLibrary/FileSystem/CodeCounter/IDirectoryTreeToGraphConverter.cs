using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using MyClassLibrary.Collections.Graph;

namespace MyClassLibrary.FileSystem.CodeCounter
{
	public interface IDirectoryTreeToGraphConverter<T, TV>
	{
		// TODO: should we pass the file interpreter via convert or with ctor of deriving classes?
		void Convert(string path, IFileInterpreter<T, TV> fileInterpreter, TextWriter? fileOutput);

		IEnumerable<IGraphNode<Tuple<string, IDictionary<T, IAddable<TV>>>>> Nodes { get; }
		IEnumerable<IGraphEdge<Tuple<string, IDictionary<T, IAddable<TV>>>, object>> Edges { get; }
	}

	public class DirectoryTreeToGraphConverter<T, TV> : IDirectoryTreeToGraphConverter<T, TV>
	{
		private readonly IFileReader _fileReader;
		private readonly IDirectoryReader _directoryReader;
		private readonly List<IGraphNode<Tuple<string, IDictionary<T, IAddable<TV>>>>> _nodes;
		private readonly List<IGraphEdge<Tuple<string, IDictionary<T, IAddable<TV>>>, object>> _edges;
		private IFileInterpreter<T, TV>? _fileInterpreter;
        private TextWriter? _fileOutput;

		public DirectoryTreeToGraphConverter(IFileReader fileReader, IDirectoryReader directoryReader)
		{
			_fileInterpreter = null;
			_fileReader = fileReader;
			_directoryReader = directoryReader;
			_nodes = new List<IGraphNode<Tuple<string, IDictionary<T, IAddable<TV>>>>>();
			_edges = new List<IGraphEdge<Tuple<string, IDictionary<T, IAddable<TV>>>, object>>();
		}

		public void Convert(string path, IFileInterpreter<T, TV> fileInterpreter, TextWriter? fileOutput)
		{
            _fileInterpreter = fileInterpreter;
            _fileOutput = fileOutput;
			ConvertInternal(path);
			CreateEdges();
		}

		[SuppressMessage("ReSharper", "ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator")]
        private void CreateEdges()
        {

            foreach (var root in _nodes)
            {
                foreach (var graphNode in _nodes)
                {
                    var directory = root.NodeValue!.Item1;
                    var subDirectory = graphNode.NodeValue!.Item1;
                    if (!FileSystemUtils.IsSubDirectory(directory, subDirectory))
                        continue;
                    
                    var edge = new GraphEdge<Tuple<string, IDictionary<T, IAddable<TV>>>, object>(root, graphNode);
                    _edges.Add(edge);
                }
            }
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
            var files = _directoryReader.GetFiles(dir);

            if (files == null)
                return;

            files = files.Where(file => file?.GetFileExtension() != null);

            var map = new Dictionary<T, IAddable<TV>>();

            foreach (var file in files)
            {
                ProcessFile(file, map);
            }

            var tuple = new Tuple<string, IDictionary<T, IAddable<TV>>>(dir, map);
            _nodes.Add(new GraphNode<Tuple<string, IDictionary<T, IAddable<TV>>>>(tuple));
        }

		private void ProcessFile(string file, IDictionary<T, IAddable<TV>> map)
		{
			var fileExtension = file.GetFileExtension();

			if (fileExtension == null)
				return;

			if (_fileInterpreter == null)
				throw new Exception("interpreter must not be null, should have been set before in this class");

			if (!_fileInterpreter.SupportsFileExtension(fileExtension))
				return;

            _fileOutput?.WriteLine("processing:\t" + file);

            var lines = _fileReader.ReadLines(file);

			var res = _fileInterpreter.Interpret(fileExtension, lines);
			if (!res.HasValue)
			{
			}
			else
			{
				if (map.ContainsKey(res.Value.Item1))
				{
					map[res.Value.Item1] = map[res.Value.Item1].Add(res.Value.Item2);
				}
				else
				{
					map.Add(res.Value.Item1, res.Value.Item2);						
				}
			}
		}

		public IEnumerable<IGraphNode<Tuple<string, IDictionary<T, IAddable<TV>>>>> Nodes => _nodes;

		public IEnumerable<IGraphEdge<Tuple<string, IDictionary<T, IAddable<TV>>>, object>> Edges =>
			_edges;
	}
}