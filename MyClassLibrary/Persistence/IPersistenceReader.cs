using System;
using System.Collections.Generic;

namespace MyClassLibrary.Persistence
{
	public interface IPersistenceReader<T> : IDisposable
	{
		int ReadInt32();
		string ReadString();
		void Close();
		T Load();
		IEnumerable<T> LoadAll();
	}
}
