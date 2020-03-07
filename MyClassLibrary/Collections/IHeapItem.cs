using System;

namespace MyClassLibrary.Collections
{
	public interface IHeapItem<T> : IComparable<T>
	{
		int HeapIndex {
			get;
			set;
		}
	}
}