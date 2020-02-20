using MyClassLibrary.Persistence;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace UnitTestProject.Persistence
{
	public class TestDummyData
	{

		private const string FileName = "data.bin";

		private void Setup()
		{
			if (File.Exists(FileName))
				File.Delete(FileName);
		}

		// TODO: refactor tests

		[Fact]
		public void TestOneDummyData()
		{
			Setup();

			using IPersistenceWriter writer = new BinaryPersistenceWriter(new FileStream(FileName, FileMode.CreateNew));

			DummyData data = new DummyData(1, "Hallo");
			writer.Save(data);

			writer.Close();

			using IPersistenceReader reader = new BinaryPersistenceReader(new FileStream(FileName, FileMode.Open));

			DummyData dummy = reader.Load<DummyData>();

			reader.Close();

			Assert.Equal(data.Number, dummy.Number);
			Assert.Equal(data.Text, dummy.Text);
		}

		[Fact]
		public void TestMultipleData()
		{
			Setup();

			using IPersistenceWriter writer = new BinaryPersistenceWriter(new FileStream(FileName, FileMode.CreateNew));

			List<DummyData> data = GenerateDummyData();

			int length = data.Count;

			DummyData[] expected = new DummyData[length];
			data.CopyTo(expected);

			writer.SaveAll(data);

			writer.Close();

			using IPersistenceReader reader = new BinaryPersistenceReader(new FileStream(FileName, FileMode.Open));

			data.Clear();

			data = reader.LoadAll<DummyData>().ToList();

			reader.Close();

			for (int i = 0; i < length; i++)
			{
				Assert.Equal(expected[i].Number, data[i].Number);
				Assert.Equal(expected[i].Text, data[i].Text);
			}
		}

		[Fact]
		public void TestEnumData()
		{
			Setup();

			using IPersistenceWriter writer = new BinaryPersistenceWriter(new FileStream(FileName, FileMode.CreateNew));

			DummyEnum data = DummyEnum.Value1;
			writer.Save(data);

			writer.Close();

			using IPersistenceReader reader = new BinaryPersistenceReader(new FileStream(FileName, FileMode.Open));

			DummyEnum dummy = reader.Load<DummyEnum>();

			reader.Close();

			Assert.Equal(data, dummy);
		}

		private List<DummyData> GenerateDummyData()
		{
			List<DummyData> result = new List<DummyData>();
			Random random = new Random();
			int length = random.Next(50, 100);
			for (int i = 0; i < length; i++)
				result.Add(new DummyData(i, i.ToString()));
			return result;
		}

	}
}
