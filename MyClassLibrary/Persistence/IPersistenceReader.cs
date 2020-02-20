using System;

namespace MyClassLibrary.Persistence
{
	public interface IPersistenceReader : IDisposable
	{
		int ReadInt32();
		string ReadString();
		void Close();
		T Load<T>();
	}
}
