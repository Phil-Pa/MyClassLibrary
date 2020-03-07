using MyClassLibrary.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyClassLibrary.Algorithms.AStar
{
	public class AStarAlgorithm
	{

		private readonly Tile[,] grid;

		private List<Tile> path = new List<Tile>();

		private readonly int size;

		private static readonly Random random = new Random();

		public AStarAlgorithm(Tile[,] grid)
		{
			this.grid = grid;
			size = (int)System.Math.Sqrt(grid.Length);
		}

		public List<Tile> GetNeighbours(Tile node, bool diagonal)
		{
			List<Tile> neighbours = new List<Tile>();

			for (int x = -1; x <= 1; x++)
			{
				for (int y = -1; y <= 1; y++)
				{
					if (x == 0 && y == 0)
						continue;

					int checkX = node.GridX + x;
					int checkY = node.GridY + y;

					if (checkX >= 0 && checkX < size && checkY >= 0 && checkY < size)
					{
						if (diagonal)
						{
							neighbours.Add(grid[checkX, checkY]);
						}
						else
						{

							var distX = System.Math.Abs(node.GridX - checkX);
							var distY = System.Math.Abs(node.GridY - checkY);

							if (Math.Math.Hypot(distX, distY) <= 1)
								neighbours.Add(grid[checkX, checkY]);
						}
					}
				}
			}

			return neighbours;
		}

		public static Tile[,] CreateGrid(int size, int percentWalls)
		{
			Tile[,] grid = new Tile[size, size];
			TileType type = TileType.Walkable;

			for (int x = 0; x < size; x++)
			{
				for (int y = 0; y < size; y++)
				{
					type = random.Next(0, 101) <= percentWalls ? TileType.Wall : TileType.Walkable;
					grid[x, y] = new Tile(type, x, y);
				}
			}

			grid[0, 0].Type = TileType.Start;
			grid[size - 1, size - 1].Type = TileType.End;

			return grid;
		}

		private Tile FindNode(Tile[,] nodes, TileType type)
		{
			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					if (nodes[i, j].Type == type)
						return nodes[i, j];
				}
			}
			throw new Exception();
		}

		private static int GetDistance(Tile nodeA, Tile nodeB)
		{
			int dstX = System.Math.Abs(nodeA.GridX - nodeB.GridX);
			int dstY = System.Math.Abs(nodeA.GridY - nodeB.GridY);

			if (dstX > dstY)
				return 14 * dstY + 10 * (dstX - dstY);
			return 14 * dstX + 10 * (dstY - dstX);
		}

		public unsafe List<Tile> FindPath(bool diagonal = true)
		{
			Tile startNode = FindNode(grid, TileType.Start);
			Tile targetNode = FindNode(grid, TileType.End);

			Heap<Tile> openSet = new Heap<Tile>(grid.Length);
			HashSet<Tile> closedSet = new HashSet<Tile>();
			openSet.Add(startNode);

			while (openSet.Count > 0)
			{
				Tile currentNode = openSet.RemoveFirst();
				closedSet.Add(currentNode);

				if (currentNode == targetNode)
				{
					RetracePath(startNode, targetNode);
					return path;
				}

				var neighbours = GetNeighbours(currentNode, diagonal);

				for (int i = 0; i < neighbours.Count; i++)
				{
					Tile neighbour = neighbours[i];

					if (!neighbour.IsWalkable || closedSet.Contains(neighbour))
					{
						continue;
					}

					int newMovementCostToNeighbour = currentNode.GCost + GetDistance(currentNode, neighbour);
					if (newMovementCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour))
					{
						neighbours[i].GCost = newMovementCostToNeighbour;
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
			}

			throw new Exception();
		}

		private unsafe void RetracePath(Tile startNode, Tile endNode)
		{
			List<Tile> path = new List<Tile>();
			Tile currentNode = endNode;

			while (currentNode != startNode)
			{
				if (currentNode.Type != TileType.Start && currentNode.Type != TileType.End)
					currentNode.Type = TileType.Path;

				path.Add(currentNode);
				currentNode = currentNode.Parent;
			}

			path.Reverse();

			this.path = path;
		}

	}
}
