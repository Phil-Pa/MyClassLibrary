using System;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MyClassLibrary.Encoding
{

	public static class StringExtensions
	{

		public static string Reverse(this string s)
		{
			var charArray = s.ToCharArray();
			Array.Reverse(charArray);
			return new string(charArray);
		}

		public static string AsBitString(this in char c, in int numBits = -1)
		{
			int number = c;
			StringBuilder sb = new StringBuilder();

			var i = 0;

			while (number >= 1)
			{
				sb.Append(number % 2);
				number /= 2;
				++i;
			}

			if (numBits != -1)
			{
				while (i++ < numBits)
					sb.Append(0);
			}

			return sb.ToString().Reverse();
		}

		public static string AsBitString(this string s, in int numBits = -1)
		{
			BitString bitStr = new BitString(s, numBits);
			return bitStr.Bits;
		}

		public static string FlipBits(this string s)
		{
			var chars = s.ToCharArray().ToList();
			chars.ForEach(c => Debug.Assert(c == '0' || c == '1'));

			StringBuilder sb = new StringBuilder();
			chars.ForEach(c => { sb.Append(c == '0' ? 1 : 0); });

			return sb.ToString();
		}
	}

	public class BitString
	{

		public int NumBits { get; }
		public string Bits { get; set; }

		public BitString(string asciiString, in int numBits = -1)
		{
			NumBits = numBits;
			Bits = AsciiToBitString(asciiString);
		}

		private string AsciiToBitString(string asciiString)
		{
			StringBuilder sb = new StringBuilder();
			asciiString.ToCharArray().ToList().ForEach(c => sb.Append(c.AsBitString(NumBits)));
			return sb.ToString();
		}
	}

	public readonly ref struct FastBitString
	{

		public int NumBits { get; }
		public Span<byte> Bits { get; }

		public FastBitString(in Span<char> asciiData, in int numBits = -1)
		{
			int length = asciiData.Length;

			for (int i = 0; i < length; i++)
			{
				char character = asciiData[i];
				byte b = character.ToByte();
			}

			throw new NotImplementedException();
		}

	}
}
