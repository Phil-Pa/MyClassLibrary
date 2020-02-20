using System.Collections.Generic;

namespace MyClassLibrary.Algorithms.AStar
{
	public class TileData
	{
		public TileData? Previos { get; set; }

		public int X { get; }
		public int Y { get; }

		public TileType Type { get; }

		public float F { get; set; }
		public float G { get; set; }
		public float H { get; set; }

		public List<TileData> Neighbors { get; private set; } = new List<TileData>();

		public TileData(TileData? previos, in int x, in int y, in TileType type, in float f = 0.0f, in float g = 0.0f, in float h = 0.0f)
		{
			Previos = previos;
			X = x;
			Y = y;
			Type = type;
			F = f;
			G = g;
			H = h;
		}

		public void AddNeighbors(List<List<TileData>> grid, in int rows, in int cols, in bool diagonal)
		{
			if (X < cols - 1)
				Neighbors.Add(grid[X + 1][Y]);
			if (X > 0)
				Neighbors.Add(grid[X - 1][Y]);
			if (Y < rows - 1)
				Neighbors.Add(grid[X][Y + 1]);
			if (Y > 0)
				Neighbors.Add(grid[X][Y - 1]);

			if (diagonal)
			{
				if (X > 0 && Y > 0)
					Neighbors.Add(grid[X - 1][Y - 1]);

				if (X < cols - 1 && Y > 0)
					Neighbors.Add(grid[X + 1][Y - 1]);

				if (X > 0 && Y < rows - 1)
					Neighbors.Add(grid[X - 1][Y + 1]);

				if (X < cols - 1 && Y < rows - 1)
					Neighbors.Add(grid[X + 1][Y + 1]);
			}
		}

		public override string ToString()
		{
			return $"Tile ({X}, {Y})";
		}
	}
}
