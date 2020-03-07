using System;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MyClassLibrary.Encoding
{
	public static class StringExtensions
	{
		/// <summary>
		/// Reverses a string
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static string Reverse(this string s)
		{
			char[] charArray = s.ToCharArray();
			Array.Reverse(charArray);
			return new string(charArray);
		}

		/// <summary>
		/// Converts a character into a bit string
		/// for example: the character 'a' is 65 in ASCII, so it would be converted to 1000001, and to 01000001 with numBits set to 8
		/// </summary>
		/// <param name="c"></param>
		/// <param name="numBits"></param>
		/// <returns></returns>
		public static string AsBitString(this in char c, in int numBits = -1)
		{
			int number = c;
			var sb = new StringBuilder();

			int i = 0;

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

		/// <summary>
		/// Converts a string into a string containing only 0s and 1s using <see cref="AsBitString(in char, in int)"/>
		/// </summary>
		/// <param name="s"></param>
		/// <param name="numBits"></param>
		/// <returns></returns>
		public static string AsBitString(this string s, in int numBits = -1)
		{
			var bitStr = new BitString(s, numBits);
			return bitStr.Bits;
		}

		/// <summary>
		/// Only works with strings that only contain 0s and 1s. Makes the 0s to 1s and the 1s to 0s
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static string FlipBits(this string s)
		{
			var chars = s.ToCharArray().ToList();
			chars.ForEach(c => Debug.Assert(c == '0' || c == '1'));

			var sb = new StringBuilder();
			chars.ForEach(c => { sb.Append(c == '0' ? 1 : 0); });

			return sb.ToString();
		}
	}

	/// <summary>
	/// Wraps a string of 0s and 1s to a class with properties
	/// </summary>
	public class BitString
	{

		/// <summary>
		/// The number of bits this bit string holds
		/// </summary>
		public int NumBits { get; }

		/// <summary>
		/// The bits as string
		/// </summary>
		public string Bits { get; }

		public BitString(string asciiString, in int numBits = -1)
		{
			NumBits = numBits;
			Bits = AsciiToBitString(asciiString);
		}

		private string AsciiToBitString(string asciiString)
		{
			var sb = new StringBuilder();
			asciiString.ToCharArray().ToList().ForEach(c => sb.Append(c.AsBitString(NumBits)));
			return sb.ToString();
		}
	}

	/// <summary>
	/// TODO:
	/// </summary>
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