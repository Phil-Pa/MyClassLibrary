﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using MyClassLibrary.Collections.Graph;
using MyClassLibrary.FileSystem.CodeCounter;

namespace MyClassLibrary.FileSystem
{
	public class DirectoryAnalyzer<T, TV> where TV : IAddable<TV>
	{

		// string is the path of the directory where a language has code stats
		
		private readonly IGraph<Tuple<string, IDictionary<T, IAddable<TV>>>, object> _graph;
		private readonly string _path;

		private readonly IAddable<TV> _defaultValue;
		
		public DirectoryAnalyzer(IDirectoryTreeToGraphConverter<T, TV> converter, string path, IAddable<TV> defaultValue, IFileInterpreter<T, TV> fileInterpreter)
		{
			// TODO: what if directory structure changes?
			// TODO: refactor
			_path = path;
			_defaultValue = defaultValue;
			converter.Convert(path, fileInterpreter);
			_graph = new Graph<Tuple<string, IDictionary<T, IAddable<TV>>>, object>(converter.Nodes, converter.Edges);
			
			// TODO: uncomment asserts when they are working
			// Debug.Assert(!_graph.IsCyclic, "directory graph must not be cyclic");
			// Debug.Assert(_graph.IsDirected, "directory graph must be directed");
		}

		public IDictionary<T, TV> GetTotalStats()
		{
			var map = new Dictionary<T, IAddable<TV>>();

			foreach (var node in _graph.Nodes)
			{
				Debug.Assert(node.NodeValue != null, "node value must not be null");
				var directoryStats = node.NodeValue!.Item2;
				foreach (var (languageKey, statsValue) in directoryStats)
				{
					if (!map.ContainsKey(languageKey))
						map.Add(languageKey, _defaultValue);

					var currentStats = map[languageKey];
					map[languageKey] = currentStats.Add(statsValue);
				}
			}

			var result = new Dictionary<T, TV>();

			foreach (var (key, value) in map)
			{
				result.Add(key, (TV)value);
			}
			
			return result;
		}

	}
}