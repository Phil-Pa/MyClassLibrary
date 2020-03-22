namespace MyClassLibrary.Collections
{
	/// <summary>
	/// A class for a binary heap. Useful for algorithms like the <see cref="Algorithms.AStar.AStarAlgorithm"/>.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Heap<T> where T : IHeapItem<T>
	{
		private readonly T[] items;

		/// <summary>
		/// The Number of the items in the Heap
		/// </summary>
		public int Count { get; private set; }

		public Heap(int maxHeapSize)
		{
			items = new T[maxHeapSize];
		}

		/// <summary>
		/// Adds a new <paramref name="item"/> to the Heap.
		/// </summary>
		/// <param name="item"></param>
		public void Add(T item)
		{
			item.HeapIndex = Count;
			items[Count] = item;
			SortUp(item);
			Count++;
		}

		public T RemoveFirst()
		{
			var firstItem = items[0];
			Count--;
			items[0] = items[Count];
			items[0].HeapIndex = 0;
			SortDown(items[0]);
			return firstItem;
		}


		public void UpdateItem(T item)
		{
			SortUp(item);
		}

		/// <summary>
		/// Returns if the Heap contains the <paramref name="item"/>.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Contains(T item)
		{
			return Equals(items[item.HeapIndex], item);
		}

		private void SortDown(T item)
		{
			while (true)
			{
				int childIndexLeft = item.HeapIndex * 2 + 1;
				int childIndexRight = item.HeapIndex * 2 + 2;
				if (childIndexLeft < Count)
				{
					int swapIndex = childIndexLeft;
					if (childIndexRight < Count)
					{
						if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
						{
							swapIndex = childIndexRight;
						}
					}

					if (item.CompareTo(items[swapIndex]) < 0)
					{
						Swap(item, items[swapIndex]);
					}
					else
					{
						return;
					}
				}
				else
				{
					return;
				}
			}
		}

		private void SortUp(T item)
		{
			int parentIndex = (item.HeapIndex - 1) / 2;

			while (true)
			{
				var parentItem = items[parentIndex];
				if (item.CompareTo(parentItem) > 0)
				{
					Swap(item, parentItem);
				}
				else
				{
					break;
				}

				parentIndex = (item.HeapIndex - 1) / 2;
			}
		}

		private void Swap(T itemA, T itemB)
		{
			items[itemA.HeapIndex] = itemB;
			items[itemB.HeapIndex] = itemA;
			int itemAIndex = itemA.HeapIndex;
			itemA.HeapIndex = itemB.HeapIndex;
			itemB.HeapIndex = itemAIndex;
		}
	}
}