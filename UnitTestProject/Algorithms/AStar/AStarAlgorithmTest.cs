using MyClassLibrary.Algorithms.AStar;
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

			var myAStarAlgorithm = new AStarAlgorithm(grid);

			var result = myAStarAlgorithm.FindPath(false);

			Assert.True(result.Count > 0);
		}

	}
}