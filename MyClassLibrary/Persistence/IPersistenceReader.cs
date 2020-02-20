using System;
using System.Collections.Generic;

namespace MyClassLibrary.Persistence
{
	public interface IPersistenceReader : IDisposable
	{
		int ReadInt32();
		string ReadString();
		void Close();
		T Load<T>();
		IEnumerable<T> LoadAll<T>();
	}
}
