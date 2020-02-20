using System;

namespace MyClassLibrary.Persistence
{
	public interface IPersistenceWriter : IDisposable
	{
		void Write(int value);
		void Write(string value);
		void Close();
		void Save<T>(T data);
	}
}
