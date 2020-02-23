using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace MyClassLibrary.Persistence
{
	public class BinaryPersistenceReader<T> : BinaryReader, IPersistenceReader<T>
	{
		public BinaryPersistenceReader(Stream input) : base(input)
		{
		}

		public T Load()
		{

			// TODO: load enums, and lists of lists...

			Type type = typeof(T);

			T data = Activator.CreateInstance<T>();

			if (type.IsEnum)
			{
				return (T)Enum.Parse(type, ReadString());
			}
			else
			{
				return LoadNonEnum(type, data);
			}
		}

		private T LoadNonEnum(Type type, T data)
		{
			PropertyInfo[] propertyInfos = type.GetProperties();

			for (int i = 0; i < propertyInfos.Length; i++)
			{
				PropertyInfo info = propertyInfos[i];

				// TODO: use type in switch case, not any value

				var obj = info.GetValue(data);
				switch (obj)
				{
					case int _:
						//if (BaseStream is StringStream ss1)
						//	info.SetValue(data, ss1.Int32());
						//else
						//	info.SetValue(data, ReadInt32());
						info.SetValue(data, ReadInt32());
						break;
					case string _:
						info.SetValue(data, ReadString());
						break;
					case char _:
						info.SetValue(data, ReadChar());
						break;
					case bool _:
						info.SetValue(data, ReadBoolean());
						break;
					default:
						throw new ArgumentException($"data of type {type} is a type, where one of its properties is not int, string, char, or bool");
				}
			}

			return data;
		}

		public IEnumerable<T> LoadAll()
		{
			List<T> items = new List<T>();

			while (BaseStream.Position < BaseStream.Length)
			{
				items.Add(Load());
			}

			return items;
		}
	}
}
