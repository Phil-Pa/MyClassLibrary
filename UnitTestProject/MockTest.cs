using System;
using Moq;
using Xunit;

namespace UnitTestProject
{
	public interface IFoo
	{
		Bar Bar { get; set; }
		string Name { get; set; }
		int Value { get; set; }
		bool DoSomething(string value);
		bool DoSomething(int number, string value);
		string DoSomethingStringy(string value);
		bool TryParse(string value, out string outputValue);
		bool Submit(ref Bar bar);
		int GetCount();
		bool Add(int value);
	}

	public class Bar
	{
		public virtual Baz Baz { get; set; }

		public virtual bool Submit()
		{
			return false;
		}
	}

	public class Baz
	{
		public virtual string Name { get; set; }
	}

	public class MockTest
	{
		[Fact]
		public void TestMock()
		{
			var mock = new Mock<IFoo>();
			mock.Setup(foo => foo.DoSomething("ping")).Returns(true);

			Assert.True(mock.Object.DoSomething("ping"));


		}
	}
}