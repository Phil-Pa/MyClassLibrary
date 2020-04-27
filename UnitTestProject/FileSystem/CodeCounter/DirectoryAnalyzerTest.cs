using System;
using System.Collections.Generic;
using Moq;
using MyClassLibrary.Collections.Graph;
using MyClassLibrary.FileSystem;
using MyClassLibrary.FileSystem.CodeCounter;
using Xunit;
using Xunit.Abstractions;

namespace UnitTestProject.FileSystem.CodeCounter
{
	public class AnalyzerTest
	{
		private readonly ITestOutputHelper _outputHelper;
		private const string Path = "E:/Test";


		public AnalyzerTest(ITestOutputHelper outputHelper)
		{
			_outputHelper = outputHelper;
		}
		
		[Fact]
		public void TestTotalCodeStats()
		{
			var nodesResult = new List<IGraphNode<Tuple<string, IDictionary<Language, IAddable<CodeStats>>>>>();
			
			var node1ValueMap = new Dictionary<Language, IAddable<CodeStats>>
			{
				[Language.C] = new CodeStats(131, 51, 13),
				[Language.CCppHeader] = new CodeStats(32, 2, 4)
			};
			var node1Value = new Tuple<string, IDictionary<Language, IAddable<CodeStats>>>("E:/Test", node1ValueMap);
			var node1 = new GraphNode<Tuple<string, IDictionary<Language, IAddable<CodeStats>>>>(node1Value);
			nodesResult.Add(node1);
			
			var node2ValueMap = new Dictionary<Language, IAddable<CodeStats>>
			{
				[Language.C] = new CodeStats(361, 71, 73),
				[Language.CCppHeader] = new CodeStats(252, 32, 44)
			};
			var node2Value = new Tuple<string, IDictionary<Language, IAddable<CodeStats>>>("E:/Test/SubDir", node2ValueMap);
			var node2 = new GraphNode<Tuple<string, IDictionary<Language, IAddable<CodeStats>>>>(node2Value);
			nodesResult.Add(node2);
			
			var edgesResult = new List<IGraphEdge<Tuple<string, IDictionary<Language, IAddable<CodeStats>>>, object>>();
			var edge = new GraphEdge<Tuple<string, IDictionary<Language, IAddable<CodeStats>>>, object>(node1, node2);
			edgesResult.Add(edge);

			var converterMock = new Mock<IDirectoryTreeToGraphConverter<Language, CodeStats>>();
			converterMock.SetupGet(it => it.Nodes).Returns(nodesResult);
			converterMock.SetupGet(it => it.Edges).Returns(edgesResult);

			var analyzer = new DirectoryAnalyzer<Language, CodeStats>(converterMock.Object, Path, new CodeStats(0, 0, 0), null);
			var stats = analyzer.GetTotalStats();

			Assert.Equal(492, stats[Language.C].CodeLines);
			Assert.Equal(122, stats[Language.C].CommentLines);
			Assert.Equal(86, stats[Language.C].BlankLines);
		}

		[Fact]
		public void Test()
		{
			var fileInterpreter = new CodeReader();
			var fileReader = new FileReader();
			var directoryReader = new DirectoryReader();
			var converter = new DirectoryTreeToGraphConverter<Language, CodeStats>(fileReader, directoryReader);
			var analyzer = new DirectoryAnalyzer<Language, CodeStats>(converter, "C:\\UE4\\UnrealEngine-release\\Engine\\Source", CodeStats.Default, fileInterpreter);

			var stats = analyzer.GetTotalStats();

			foreach (var (key, value) in stats)
			{
				_outputHelper.WriteLine(key + "\t" + value);
			}
			
			
		}
	}
}