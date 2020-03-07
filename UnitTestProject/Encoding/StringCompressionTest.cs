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

			string compressed = StringCompression.Compress(sampleText);

			string decompressed = StringCompression.Decompress(compressed);
			Assert.Equal(sampleText, decompressed);
		}
	}
}