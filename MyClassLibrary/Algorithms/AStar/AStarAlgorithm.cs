using System;
using System.Collections.Generic;
using MyClassLibrary.Collections;

namespace MyClassLibrary.Algorithms.AStar
{
	/// <summary>
	/// Class that implements the a star algorithm and only works for quadratic grids
	/// </summary>
	public class AStarAlgorithm
	{

		private readonly Tile[,] _grid;

		private readonly Tile[] _neighBourBuffer = new Tile[8];

		private List<Tile> _path = new List<Tile>();

		private readonly int _size;

		private static readonly Random Random = new Random();

		/// <summary>
		/// Initializes the object with a grid to use the algorithm on to
		/// </summary>
		/// <param name="grid">The grid must have a length so that the square root of the length is an integer</param>
		public AStarAlgorithm(Tile[,] grid)
		{
			_grid = grid;
			_size = (int)System.Math.Sqrt(grid.Length);
		}

		private int GetNeighbours(Tile node, bool diagonal)
		{
			var index = 0;

			for (var x = -1; x <= 1; x++)
			{
				for (var y = -1; y <= 1; y++)
				{
					if (x == 0 && y == 0)
						continue;

					var checkX = node.GridX + x;
					var checkY = node.GridY + y;

					if (checkX < 0 || checkX >= _size || checkY < 0 || checkY >= _size)
						continue;

					if (diagonal)
					{
						_neighBourBuffer[index++] = _grid[checkX, checkY];
					}
					else
					{

						var distX = System.Math.Abs(node.GridX - checkX);
						var distY = System.Math.Abs(node.GridY - checkY);

						if (Math.Math.Hypot(distX, distY) <= 1)
							_neighBourBuffer[index++] = _grid[checkX, checkY];
					}
				}
			}

			return index;
		}

		/// <summary>
		/// Creates a randomized example grid with size of size * size
		/// </summary>
		/// <param name="size"></param>
		/// <param name="percentWalls"></param>
		/// <returns></returns>
		public static Tile[,] CreateGrid(int size, int percentWalls)
		{
			var grid = new Tile[size, size];

			for (var x = 0; x < size; x++)
			{
				for (var y = 0; y < size; y++)
				{
					var type = Random.Next(0, 101) <= percentWalls ? TileType.Wall : TileType.Walkable;
					grid[x, y] = new Tile(type, x, y);
				}
			}

			grid[0, 0].Type = TileType.Start;
			grid[size - 1, size - 1].Type = TileType.End;

			return grid;
		}

		private Tile FindNode(Tile[,] nodes, TileType type)
		{
			for (var i = 0; i < _size; i++)
			{
				for (var j = 0; j < _size; j++)
				{
					if (nodes[i, j].Type == type)
						return nodes[i, j];
				}
			}
			throw new Exception("could not find node");
		}

		private static int GetDistance(Tile nodeA, Tile nodeB)
		{
			var dstX = System.Math.Abs(nodeA.GridX - nodeB.GridX);
			var dstY = System.Math.Abs(nodeA.GridY - nodeB.GridY);

			if (dstX > dstY)
				return 14 * dstY + 10 * (dstX - dstY);
			return 14 * dstX + 10 * (dstY - dstX);
		}

		/// <summary>
		/// Finds a path applying the a star algorithm. Throws an exception if there is no valid path
		/// </summary>
		/// <param name="diagonal"></param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		public List<Tile> FindPath(bool diagonal = true)
		{
			var startNode = FindNode(_grid, TileType.Start);
			var targetNode = FindNode(_grid, TileType.End);

			var gridLength = _grid.Length;

			var openSet = new Heap<Tile>(gridLength);
			var closedSet = new Heap<Tile>(gridLength);
			openSet.Add(startNode);

			while (openSet.Count > 0)
			{
				var currentNode = openSet.RemoveFirst();
				closedSet.Add(currentNode);

				if (currentNode == targetNode)
				{
					RetracePath(startNode, targetNode);
					return _path;
				}

				var neighBoursLength = GetNeighbours(currentNode, diagonal);

				for (var i = 0; i < neighBoursLength; i++)
				{
					var neighbour = _neighBourBuffer[i];

					if (!neighbour.IsWalkable || closedSet.Contains(neighbour))
					{
						continue;
					}

					var newMovementCostToNeighbour = currentNode.GCost + GetDistance(currentNode, neighbour);
					if (newMovementCostToNeighbour >= neighbour.GCost && openSet.Contains(neighbour))
						continue;

					_neighBourBuffer[i].GCost = newMovementCostToNeighbour;
					neighbour.HCost = GetDistance(neighbour, targetNode);
					neighbour.Parent = currentNode;

					if (!openSet.Contains(neighbour))
					{
						openSet.Add(neighbour);
					}
					else
					{
						openSet.UpdateItem(neighbour);
					}
				}
			}

			throw new Exception("could not find path");
		}

		private void RetracePath(Tile startNode, Tile endNode)
		{
			var path = new List<Tile>(_size);
			var currentNode = endNode;

			while (currentNode! != startNode)
			{
				if (currentNode!.Type != TileType.Start && currentNode.Type != TileType.End)
					currentNode.Type = TileType.Path;

				path.Add(currentNode);
				currentNode = currentNode.Parent!;
			}

			path.Reverse();

			_path = path;
		}

	}
}
