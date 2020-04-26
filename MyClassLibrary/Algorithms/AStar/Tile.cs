using MyClassLibrary.Collections;
using System.Diagnostics;

namespace MyClassLibrary.Algorithms.AStar
{
	[DebuggerDisplay("{" + nameof(GridX) + "}, " + "{" + nameof(GridY) + "}, {" + nameof(Type) + "}")]
	public class Tile : IHeapItem<Tile>
	{
		public TileType Type { get; set; }

		private int FCost => GCost + HCost;

		public int GCost { get; set; }
		public int HCost { get; set; }
		public int GridX { get; }
		public int GridY { get; }
		public Tile? Parent { get; set; }

		public bool IsWalkable => Type != TileType.Wall && Type != TileType.Path;

		public int HeapIndex { get; set; }

		public Tile(TileType type, int gridX, int gridY)
		{
			Type = type;
			GridX = gridX;
			GridY = gridY;
			Parent = null;
			HeapIndex = 0;
			GCost = 0;
			HCost = 0;
		}

		public int CompareTo(Tile nodeToCompare)
		{
			var compare = FCost.CompareTo(nodeToCompare.FCost);
			if (compare == 0)
			{
				compare = HCost.CompareTo(nodeToCompare.HCost);
			}
			return -compare;
		}

		public override bool Equals(object obj)
		{
			if (obj is Tile node)
			{
				// assume no node has the same coordinates
				return GridX == node.GridX && GridY == node.GridY;
			}

			return false;
		}

		public override int GetHashCode()
		{
			return GridX.GetHashCode() * 4 + GridY.GetHashCode() ^ FCost.GetHashCode();
		}

		public static bool operator ==(Tile left, Tile right)
		{
			// ReSharper disable once PossibleNullReferenceException
			return left.Equals(right);
		}

		public static bool operator !=(Tile left, Tile right)
		{
			return !(left == right);
		}

		public static bool operator <(Tile left, Tile right)
		{
			return left is null ? !(right is null) : left.CompareTo(right) < 0;
		}

		public static bool operator <=(Tile left, Tile right)
		{
			return left is null ? !(right is null) : left.CompareTo(right) <= 0;
		}

		public static bool operator >(Tile left, Tile right)
		{
			return left is null ? !(right is null) : left.CompareTo(right) > 0;
		}

		public static bool operator >=(Tile left, Tile right)
		{
			return left is null ? !(right is null) : left.CompareTo(right) >= 0;
		}
	}
}
