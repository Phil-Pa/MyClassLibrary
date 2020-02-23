using System;
using System.Collections.Generic;

namespace MyClassLibrary.Persistence
{
	public interface IPersistenceWriter<T> : IDisposable
	{
		void Write(int value);
		void Write(string value);
		void Close();
		void Save(T data);
		void SaveAll(IList<T> items);
	}
}
