using MyClassLibrary.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MyClassLibrary.Algorithms.AStar
{
	[DebuggerDisplay("{" + nameof(GridX) + "}, " + "{" + nameof(GridY) + "}, {" + nameof(Type) + "}")]
	public unsafe class Tile : IHeapItem<Tile>
	{
		public TileType Type { get; set; }

		public int FCost {
			get
			{
				return GCost + HCost;
			}
		}

		public int GCost { get; set; }
		public int HCost { get; set; }
		public int GridX { get; set; }
		public int GridY { get; set; }
		public Tile Parent { get; set; }

		public bool IsWalkable {
			get
			{
				return Type != TileType.Wall && Type != TileType.Path;
			}
		}

		public int HeapIndex { get; set; }

		public Tile(TileType type, int _gridX, int _gridY)
		{
			Type = type;
			GridX = _gridX;
			GridY = _gridY;
			Parent = null;
			HeapIndex = 0;
			GCost = 0;
			HCost = 0;
		}

		public int CompareTo(Tile nodeToCompare)
		{
			int compare = FCost.CompareTo(nodeToCompare.FCost);
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
			return GridX.GetHashCode() * 4 + GridY.GetHashCode() ^ Type.GetHashCode();
		}

		public static bool operator ==(Tile left, Tile right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(Tile left, Tile right)
		{
			return !(left == right);
		}
	}
}
