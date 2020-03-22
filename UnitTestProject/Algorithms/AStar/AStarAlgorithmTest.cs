using MyClassLibrary.Algorithms.AStar;
using System.Collections.Generic;
using System.Diagnostics;
using Xunit;

namespace UnitTestProject.Algorithms.AStar
{
	public class AStarAlgorithmTest
	{

		[Fact]
		public void BasicTest()
		{
			
		}

		[Fact]
		public void TestPerformance()
		{
			var grid = AStarAlgorithm.CreateGrid(10, 5);

			AStarAlgorithm myAStarAlgorithm = new AStarAlgorithm(grid);

			var result = myAStarAlgorithm.FindPath(false);
		}

	}
}