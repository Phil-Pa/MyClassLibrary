using System.Collections.Generic;
using Xunit;

using MyClassLibrary;
using Xunit.Abstractions;
using System.Linq;

namespace UnitTestProject
{
	public class TestExtensions
	{

		private readonly ITestOutputHelper _output;

		public TestExtensions(ITestOutputHelper output)
		{
			_output = output;
		}

		[Fact]
		public void TestPermutations()
		{

			var list = new List<int>
			{
				1, 2, 3, 4
			};

			for (int i = 2; i < 10; i++)
			{
				var permutations = list.GetPermutations(list.Count);
				_output.WriteLine(permutations.Count().ToString());
			}
			
		}

	}
}
