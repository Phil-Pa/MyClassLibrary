using MyClassLibrary.Encoding;
using Xunit;

namespace UnitTestProject.Encoding
{
	public class StringCompressionTest
	{
		[Fact]
		public void Test()
		{
			const string sampleText = "hallo hier ist mein beispiel text";

			var compressed = StringCompression.Compress(sampleText);

			var decompressed = StringCompression.Decompress(compressed);
			Assert.Equal(sampleText, decompressed);
		}
	}
}