namespace MyClassLibrary.Collections
{
	/// <summary>
	/// A class for a binary heap. Useful for algorithms like the <see cref="Algorithms.AStar.AStarAlgorithm"/>.
	/// The <see cref="Contains"/> method has time complexity of O(1)
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Heap<T> where T : IHeapItem<T>
	{
		private readonly T[] _items;

		/// <summary>
		/// The number of the items currently stored in the heap
		/// </summary>
		public int Count { get; private set; }

		/// <summary>
		/// Initializes the heap data structure. <paramref name="maxHeapSize"/> is the maximum amount of items in the heap and
		/// the maximum number of items can't be changed later
		/// </summary>
		/// <param name="maxHeapSize"></param>
		public Heap(int maxHeapSize)
		{
			_items = new T[maxHeapSize];
		}

		/// <summary>
		/// Adds a new item to the heap.
		/// </summary>
		/// <param name="item"></param>
		public void Add(T item)
		{
			item.HeapIndex = Count;
			_items[Count] = item;
			SortUp(item);
			Count++;
		}

		/// <summary>
		/// Removes the root of the heap binary tree and re-structures the heap
		/// </summary>
		/// <returns>Returns the removed root item</returns>
		public T RemoveFirst()
		{
			var firstItem = _items[0];
			Count--;
			_items[0] = _items[Count];
			_items[0].HeapIndex = 0;
			SortDown(_items[0]);
			return firstItem;
		}

		/// <summary>
		/// If necessary the item is rearanged upward the binary heap tree
		/// </summary>
		/// <param name="item"></param>
		public void UpdateItem(T item)
		{
			SortUp(item);
		}

		/// <summary>
		/// Returns if the Heap contains the item
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Contains(T item)
		{
			return Equals(_items[item.HeapIndex], item);
		}

		private void SortDown(T item)
		{
			while (true)
			{
				var childIndexLeft = item.HeapIndex * 2 + 1;
				var childIndexRight = item.HeapIndex * 2 + 2;
				if (childIndexLeft < Count)
				{
					var swapIndex = childIndexLeft;
					if (childIndexRight < Count)
					{
						if (_items[childIndexLeft].CompareTo(_items[childIndexRight]) < 0)
						{
							swapIndex = childIndexRight;
						}
					}

					if (item.CompareTo(_items[swapIndex]) < 0)
					{
						Swap(item, _items[swapIndex]);
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
			var parentIndex = (item.HeapIndex - 1) / 2;

			while (true)
			{
				var parentItem = _items[parentIndex];
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
			_items[itemA.HeapIndex] = itemB;
			_items[itemB.HeapIndex] = itemA;
			var itemAIndex = itemA.HeapIndex;
			itemA.HeapIndex = itemB.HeapIndex;
			itemB.HeapIndex = itemAIndex;
		}
	}
}