using System;
using System.IO;
using System.IO.Compression;

namespace MyClassLibrary.Encoding
{
	/// <summary>
	/// Useful for string compression to minimize the amount of characters in a string
	/// </summary>
	public static class StringCompression
	{
		/// <summary>
		/// Compresses a string using the zip format. use <see cref="Decompress(string)"/> to decompress
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string Compress(string str)
		{
			var buffer = System.Text.Encoding.UTF8.GetBytes(str);
			MemoryStream memoryStream = new MemoryStream();
			using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
			{
				gZipStream.Write(buffer, 0, buffer.Length);
			}

			memoryStream.Position = 0;

			var compressedData = new byte[memoryStream.Length];
			memoryStream.Read(compressedData, 0, compressedData.Length);

			var gZipBuffer = new byte[compressedData.Length + 4];
			Buffer.BlockCopy(compressedData, 0, gZipBuffer, 4, compressedData.Length);
			Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gZipBuffer, 0, 4);
			return Convert.ToBase64String(gZipBuffer);
		}

		/// <summary>
		/// Decompresses a string compressed with <see cref="Compress(string)"/>
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string Decompress(string str)
		{
			var gZipBuffer = Convert.FromBase64String(str);
			using MemoryStream memoryStream = new MemoryStream();
			var dataLength = BitConverter.ToInt32(gZipBuffer, 0);
			memoryStream.Write(gZipBuffer, 4, gZipBuffer.Length - 4);

			var buffer = new byte[dataLength];

			memoryStream.Position = 0;
			using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
			{
				gZipStream.Read(buffer, 0, buffer.Length);
			}

			return System.Text.Encoding.UTF8.GetString(buffer);
		}

	}
}
