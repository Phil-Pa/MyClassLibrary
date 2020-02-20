using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace MyClassLibrary.Persistence
{
	public class BinaryPersistenceReader : BinaryReader, IPersistenceReader
	{
		public BinaryPersistenceReader(FileStream input) : base(input)
		{
		}

		public T Load<T>()
		{

			Type type = typeof(T);

			T data = Activator.CreateInstance<T>();

			PropertyInfo[] propertyInfos = type.GetProperties();

			for (int i = 0; i < propertyInfos.Length; i++)
			{
				PropertyInfo info = propertyInfos[i];

				// TODO: use type in switch case, not any value

				var obj = info.GetValue(data);
				switch (obj)
				{
					case int value:
						info.SetValue(data, ReadInt32());
						break;
					case string value:
						info.SetValue(data, ReadString());
						break;
					case char value:
						info.SetValue(data, ReadChar());
						break;
					case bool value:
						info.SetValue(data, ReadBoolean());
						break;
					default:
						throw new ArgumentException($"data of type {type} is a type, where one of its properties is not int, string, char, or bool");
				}
			}

			return data;
		}

		public IEnumerable<T> LoadAll<T>()
		{
			List<T> items = new List<T>();

			while (BaseStream.Position < BaseStream.Length)
			{
				items.Add(Load<T>());
			}

			return items;
		}
	}
}
