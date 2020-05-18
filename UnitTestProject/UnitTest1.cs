using Xunit.Abstractions;

namespace UnitTestProject
{
	public class UnitTest1
	{
		private readonly ITestOutputHelper _testOutputHelper;

		public UnitTest1(ITestOutputHelper testOutputHelper)
		{
			_testOutputHelper = testOutputHelper;
		}
    }
}