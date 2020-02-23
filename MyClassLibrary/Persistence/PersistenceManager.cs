using MyClassLibrary.Encoding;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace MyClassLibrary.Persistence
{
	public class PersistenceManager<T>
	{

		public bool CanCompressData { get; }

		public string FileName { get; }

		public PersistenceManager(bool canCompressData = false, string? fileName = null)
		{
			CanCompressData = canCompressData;
			if (fileName == null)
				FileName = typeof(T).Name + ".bin";
			else
				FileName = fileName;
		}

		private bool AllowCompress(string output)
		{
			return CanCompressData && output.Length >= 10000;
		}

		private void InternalSave(Action<IPersistenceWriter<T>> action)
		{
			MemoryStream memoryStream = new MemoryStream();
			using BinaryPersistenceWriter<T> writer = new BinaryPersistenceWriter<T>(memoryStream);

			// writes to string stream
			action(writer);

			byte[] readBuffer = new byte[memoryStream.Length];
			memoryStream.Position = 0;

			int read = memoryStream.Read(readBuffer, 0, readBuffer.Length);
			Debug.Assert(read == readBuffer.Length);

			string output = System.Text.Encoding.ASCII.GetString(readBuffer);

			// only compress if the amount of data is high enough that it would be worth it

			if (AllowCompress(output))
				output = StringCompression.Compress(output);

			File.WriteAllText(FileName, output);

			writer.Close();
		}

		public void Save(T data)
		{
			InternalSave((writer) => writer.Save(data));
		}

		public void SaveAll(IList<T> items)
		{
			InternalSave((writer) => writer.SaveAll(items));
		}

		public T Load()
		{
			MemoryStream ss = new MemoryStream();
			ss.Write(System.Text.Encoding.ASCII.GetBytes(File.ReadAllText(FileName)));
			ss.Position = 0;

			using BinaryPersistenceReader<T> reader = new BinaryPersistenceReader<T>(ss);

			T result = reader.Load();
			reader.Close();
			return result;
		}

		public IEnumerable<T> LoadAll()
		{
			MemoryStream ss = new MemoryStream();
			ss.Write(System.Text.Encoding.ASCII.GetBytes(File.ReadAllText(FileName)));
			ss.Position = 0;

			using BinaryPersistenceReader<T> reader = new BinaryPersistenceReader<T>(ss);

			var result = reader.LoadAll();
			reader.Close();
			return result;
		}
	}
}
