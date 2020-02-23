using MyClassLibrary.Algorithms.AStar;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xunit;

namespace UnitTestProject.Algorithms.AStar
{
	public class AStarAlgorithmTest
	{

		private readonly List<List<TileType>> correctWay = new List<List<TileType>>()
		{
			new List<TileType>
			{
				TileType.Path, TileType.Path, TileType.Path, TileType.Path, TileType.End
			},
			new List<TileType>
			{
				TileType.Path, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Walkable
			},
			new List<TileType>
			{
				TileType.Path, TileType.Wall, TileType.Start, TileType.Wall, TileType.Wall
			},
			new List<TileType>
			{
				TileType.Path, TileType.Wall, TileType.Path, TileType.Walkable, TileType.Walkable
			},
			new List<TileType>
			{
				TileType.Path, TileType.Path, TileType.Path, TileType.Walkable, TileType.Walkable
			}
		};

		[Fact]
		public void BasicTest()
		{
			List<List<TileType>> input = new List<List<TileType>>()
			{
				new List<TileType>
				{
					TileType.Walkable, TileType.Walkable, TileType.Walkable, TileType.Walkable, TileType.End
				},
				new List<TileType>
				{
					TileType.Walkable, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Walkable
				},
				new List<TileType>
				{
					TileType.Walkable, TileType.Wall, TileType.Start, TileType.Wall, TileType.Wall
				},
				new List<TileType>
				{
					TileType.Walkable, TileType.Wall, TileType.Walkable, TileType.Walkable, TileType.Walkable
				},
				new List<TileType>
				{
					TileType.Walkable, TileType.Walkable, TileType.Walkable, TileType.Walkable, TileType.Walkable
				}
			};

			AStarAlgorithm aStarAlgorithm = new AStarAlgorithm();
			var result = aStarAlgorithm.CalculatePath(input);

			Assert.Equal(correctWay, result);
		}

		[Fact]
		public void TestPerformance()
		{

			var input = AStarAlgorithm.CreateRandomTileMap(100, 30);
			AStarAlgorithm aStarAlgorithm = new AStarAlgorithm();
			var watch = Stopwatch.StartNew();
			var result = aStarAlgorithm.CalculatePath(input);
			watch.Stop();

			Assert.True(result.Count > 0);
			Assert.True(watch.ElapsedMilliseconds < 5000);
		}
	}
}