using System;
using System.IO;
using Xunit;

namespace UnitTestProject
{
	public class FileStreamTest
	{
		private const string FileName = "testfile.txt";

		public FileStreamTest()
		{
			if (File.Exists(FileName))
				File.Delete(FileName);
		}

		[Fact]
		public void Test()
		{
			using Stream fs = new FileStream(FileName, FileMode.Create);

			Assert.Equal(0, fs.Position);
			Assert.Equal(0, fs.Length);

			ReadOnlySpan<byte> buffer = stackalloc byte[] { 1 };

			// simple write
			fs.Write(buffer);
			Assert.Equal(1, fs.Position);
			Assert.Equal(1, fs.Length);

			// goto begin
			fs.Seek(0, SeekOrigin.Begin);

			Assert.Equal(0, fs.Position);
			Assert.Equal(1, fs.Length);

			// over write the only written value
			fs.Write(buffer);
			Assert.Equal(1, fs.Position);
			Assert.Equal(1, fs.Length);

			// write a second value
			fs.Write(buffer);
			Assert.Equal(2, fs.Position);
			Assert.Equal(2, fs.Length);

			// try to read both written values

			// create buffer
			byte[] readBuffer = new byte[fs.Length];

			// goto begin
			//fs.Seek(0, SeekOrigin.Begin);
			fs.Position = 0;

			// read all from begin to end
			int read = fs.Read(readBuffer, 0, readBuffer.Length);

			// verify results
			Assert.Equal(2, read);
			Assert.Equal(1, readBuffer[0]);
			Assert.Equal(1, readBuffer[1]);

			fs.Seek(2, SeekOrigin.End);

			// with this, the length gets set to the position value and the gap is filled with zeros
			fs.Write(buffer);

			readBuffer = new byte[fs.Position];
			fs.Position = 0;
			read = fs.Read(readBuffer, 0, readBuffer.Length);

			Assert.Equal(5, read);

			fs.Close();
		}
	}
}