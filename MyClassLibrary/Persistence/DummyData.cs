using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyClassLibrary.Persistence
{
	public class DummyData : IBinaryData<DummyData>
	{

		public int Number { get; set; }
		public string Text { get; set; }

		public DummyData()
		{
			Number = 0;
			Text = string.Empty;
		}

		public DummyData(int number, string text)
		{
			Number = number;
			Text = text ?? throw new ArgumentNullException(nameof(text));
		}

		public void Load(IPersistenceReader reader)
		{
			Number = reader.ReadInt32();
			Text = reader.ReadString();
		}

		public void Save(IPersistenceWriter writer)
		{
			writer.Write(Number);
			writer.Write(Text);
		}
	}
}
