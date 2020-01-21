using System;
using System.Collections.Generic;
using System.Text;
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

			StringCompression stringCompression = new StringCompression();
			var compressed = stringCompression.Compress(sampleText);

			var decompressed = stringCompression.Decompress(compressed);
			Assert.Equal(sampleText, decompressed);
		}
	}
}
