using MyClassLibrary.Persistence;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace UnitTestProject.Persistence
{
	public class PersistenceManagerTest
	{

		private const string FileName = "data.save";

		public PersistenceManagerTest()
		{
			if (File.Exists(FileName))
				File.Delete(FileName);
		}

		[Fact]
		public void Test()
		{
			PersistenceManager<DummyData> persistenceManager = new PersistenceManager<DummyData>(true);

			persistenceManager.Save(new DummyData(1, "World"));

			DummyData data = persistenceManager.Load();

			Assert.Equal(1, data.Number);
			Assert.Equal("World", data.Text);
		}

		[Fact]
		public void TestMultiple()
		{
			PersistenceManager<DummyData> persistenceManager = new PersistenceManager<DummyData>(true);

			persistenceManager.SaveAll(new[] { new DummyData(1, "World"), new DummyData(2, "Hello") });

			var list  = persistenceManager.LoadAll().ToList();

			var data = list.First();

			Assert.Equal(1, data.Number);
			Assert.Equal("World", data.Text);

			data = list.Last();

			Assert.Equal(2, data.Number);
			Assert.Equal("Hello", data.Text);
		}

	}
}
