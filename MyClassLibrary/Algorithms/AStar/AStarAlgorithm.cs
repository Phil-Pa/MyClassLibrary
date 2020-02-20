using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MyClassLibrary.Algorithms.AStar
{

	public class AStarAlgorithm
	{

		private readonly List<TileData> openSet = new List<TileData>();
		private readonly List<TileData> closedSet = new List<TileData>();

		private readonly List<List<TileData>> grid = new List<List<TileData>>();
		private readonly List<TileData> tiles = new List<TileData>();

		private readonly List<TileData> path = new List<TileData>();

		private bool diagonal = false;

		private float Heuristic(TileData start, TileData end)
		{
			if (!diagonal)
			{
				if (start.X == end.X)
					return MathF.Abs(start.Y - end.Y);
				if (start.Y == end.Y)
					return MathF.Abs(start.X - end.X);
				if (start.X == end.X && start.Y == end.Y)
					return 0.0f;
				else
					return MathF.Abs(start.X - end.X) + MathF.Abs(start.Y - end.Y);
			}
			else
			{
				return MathF.Sqrt(MathF.Pow(MathF.Abs(start.X - end.X), 2.0f) + MathF.Pow(MathF.Abs(start.Y - end.Y), 2.0f));
			}
		}

		private void CheckOneStartAndEndTile(List<List<TileType>> gridData)
		{
			Debug.Assert(gridData.Flatten().Where((type) => type == TileType.Start).Count() == 1);
			Debug.Assert(gridData.Flatten().Where((type) => type == TileType.End).Count() == 1);
		}

		private void CheckValidGridSize(List<List<TileType>> gridData)
		{
			Debug.Assert(gridData.IsNotEmpty());
			int size = gridData[0].Count;

			foreach (var list in gridData)
			{
				Debug.Assert(size == list.Count);
			}
		}

		public List<List<TileType>> CalculatePath(List<List<TileType>> gridData, bool diagonal = false)
		{
			this.diagonal = diagonal;

			CheckValidGridSize(gridData);
			CheckOneStartAndEndTile(gridData);

			int rows = gridData.Count;
			int cols = gridData[0].Count;

			for (int x = 0; x < rows; x++)
			{
				for (int y = 0; y < cols; y++)
				{
					tiles.Add(new TileData(null, x, y, gridData[y][x]));
				}
			}

			TileData startTile = tiles.Find((tileData) => tileData.Type == TileType.Start);
			TileData endTile = tiles.Find((tileData) => tileData.Type == TileType.End);

			for (int x = 0; x < cols; x++)
			{
				grid.Add(new List<TileData>());
				for (int y = 0; y < rows; y++)
				{
					var foundTiles = tiles.FindAll((tileData) => tileData.X == x && tileData.Y == y);
					grid[x].AddRange(foundTiles);
				}
			}

			for (int x = 0; x < cols; x++)
			{
				for (int y = 0; y < rows; y++)
				{
					grid[x][y].AddNeighbors(grid, rows, cols, diagonal);
				}
			}

			TileData start = grid[startTile.X][startTile.Y];
			TileData end = grid[endTile.X][endTile.Y];

			openSet.Add(start);

			while (true)
			{
				int bestIndex = 0;

				for (int i = 0; i < openSet.Count; i++)
				{
					if (openSet[i].F < openSet[bestIndex].F)
						bestIndex = i;
				}

				var current = openSet[bestIndex];

				if (current == end)
				{
					var temp = current;
					path.Add(temp);

					while (temp.Previos != null)
					{
						var prev = temp.Previos;
						if (prev == null)
							break;

						path.Add(prev);
						temp = prev;
					}

					break;
				}

				for (int i = 0; i < openSet.Count; i++)
				{
					if (openSet[i] == current)
						openSet.Remove(current);
				}

				closedSet.Add(current);

				for (int i = 0; i < current.Neighbors.Count; i++)
				{
					TileData neighbor = current.Neighbors[i];

					if (!closedSet.Contains(neighbor) && neighbor.Type != TileType.Wall)
					{
						float tempG = current.G + 1;
						bool newPath = false;
						
						if (openSet.Contains(neighbor))
						{
							if (tempG < neighbor.G)
							{
								neighbor.G = tempG;
								newPath = true;
							}
						}
						else
						{
							neighbor.G = tempG;
							openSet.Add(neighbor);
							newPath = true;
						}

						if (newPath)
						{
							neighbor.H = Heuristic(neighbor, end);
							neighbor.F = neighbor.G + neighbor.H;
							neighbor.Previos = current;
						}
					}
				}
			}

			foreach (var tile in path)
			{
				TileType tileType = gridData[tile.Y][tile.X];
				if (!(tileType == TileType.Start || tileType == TileType.End))
					gridData[tile.Y][tile.X] = TileType.Path;
			}

			return gridData;
		}

		public static List<List<TileType>> CreateRandomTileMap(in int size, in int percentWalls, (int, int)? start = null, (int, int)? end = null)
		{
			if (start == null)
				start = (0, size - 1);
			if (end == null)
				end = (size - 1, 0);

			var tiles = new List<List<TileType>>();

			Random random = new Random();

			for (int x = 0; x < size; x++)
			{
				tiles.Add(new List<TileType>());
				for (int y = 0; y < size; y++)
				{
					if (x == start.Value.Item1 && y == start.Value.Item2)
					{
						tiles[x].Add(TileType.Start);
						continue;
					}
					else if (x == end.Value.Item1 && y == end.Value.Item2)
					{
						tiles[x].Add(TileType.End);
						continue;
					}

					TileType tileType;
					if (random.Next(1, 100) <= percentWalls)
						tileType = TileType.Wall;
					else
						tileType = TileType.Walkable;

					tiles[x].Add(tileType);
				}
			}

			return tiles;
		}

	}
}
