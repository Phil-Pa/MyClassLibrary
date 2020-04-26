using MyClassLibrary.Math;
using Xunit;

namespace UnitTestProject.Encoding
{
	public class DefaultNumberConverterTest
	{
		private readonly INumberConverter _numberConverter = new DefaultNumberConverter();

		[Fact]
		public void TestBase64Conversion()
		{
			var result = _numberConverter.Convert(10, 2, "128");
			Assert.Equal("10000000", result);

			result = _numberConverter.Convert(64, 10, "+-aA");
			Assert.Equal((10 + 36 * 64 + 63 * 64 * 64 + 62 * 64 * 64 * 64).ToString(), result);
		}
	}
}