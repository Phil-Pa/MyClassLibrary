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

		[Fact]
		public void TestGetNeighbor()
		{

			var type = typeof(AStarAlgorithm);
			var method = type.GetMethod("GetNeighbours");

			Assert.NotNull(method);

			int size = 5;

			Tile[,] grid = new Tile[size, size];
			for (int x = 0; x < size; x++)
			{
				for (int y = 0; y < size; y++)
				{
					grid[x, y] = new Tile(TileType.Walkable, x, y);
				}
			}

			grid[0, 0].Type = TileType.Start;
			grid[size - 1, size - 1].Type = TileType.End;

			for (int i = 1; i < size - 1; i++)
			{
				grid[i, i].Type = TileType.Wall;
			}

			AStarAlgorithm algorithm = new AStarAlgorithm(grid);

			List<Tile> tiles = (List<Tile>)method.Invoke(algorithm, new object[] { grid[0, 0], false });
			Assert.Equal(2, tiles.Count);

			//tiles = (List<Tile>)method.Invoke(algorithm, new object[] { grid[0, 1], false });
			//Assert.Assert(tiles);

			//tiles = (List<Tile>)method.Invoke(algorithm, new object[] { grid[0, 1], false });
			//Assert.Single(tiles);
		}
	}
}