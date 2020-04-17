using System;

namespace MyClassLibrary.Collections
{
	public interface IHeapItem<in T> : IComparable<T>
	{
		int HeapIndex {
			get;
			set;
		}
	}
}