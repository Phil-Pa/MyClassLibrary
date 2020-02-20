using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace MyClassLibrary.Persistence
{
	public class BinaryPersistenceWriter : BinaryWriter, IPersistenceWriter
	{
		public BinaryPersistenceWriter(FileStream input) : base(input)
		{
		}

		public void Save<T>(T data)
		{
			// save all public properties

			Type type = typeof(T);

			PropertyInfo[] propertyInfos = type.GetProperties();

			for (int i = 0; i < propertyInfos.Length; i++)
			{
				PropertyInfo info = propertyInfos[i];
				var obj = info.GetValue(data);
				switch (obj)
				{
					case int value:
						Write(value);
						break;
					case string value:
						Write(value);
						break;
					case char value:
						Write(value);
						break;
					case bool value:
						Write(value);
						break;
					default:
						throw new ArgumentException($"data of type {type} is a type, where one of its properties is not int, string, char, or bool");
				}
			}
		}

		public void SaveAll<T>(IList<T> items)
		{
			int length = items.Count;

			for (int i = 0; i < length; i++)
			{
				Save(items[i]);
			}
		}
	}
}
