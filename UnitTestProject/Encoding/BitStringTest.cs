using MyClassLibrary.Encoding;
using Xunit;

namespace UnitTestProject.Encoding
{
	public class BitStringTest
	{
		[Fact]
		public void TestReverse()
		{
			const string str = "101010100011";

			Assert.Equal("110001010101", str.Reverse());
		}

		[Fact]
		public void TestBitString()
		{
			var bitString = new BitString("A");
			Assert.Equal("1000001", bitString.Bits);

			bitString = new BitString("AB");
			Assert.Equal("10000011000010", bitString.Bits);

			bitString = new BitString("A", 8);
			Assert.Equal("01000001", bitString.Bits);
		}

		[Fact]
		public void TestAsBitString()
		{
			string str = "hallo".AsBitString();
			Assert.Equal(new BitString("hallo").Bits, str);
		}

		[Fact]
		public void TestFlipBits()
		{
			string bits = "A".AsBitString().FlipBits();
			Assert.Equal("0111110", bits);
		}
	}
}