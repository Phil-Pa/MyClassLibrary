using System;
using System.Collections.Generic;
using System.Linq;
using MyClassLibrary.FileSystem.CodeCounter;

namespace MyClassLibrary.FileSystem
{

	public readonly struct MyInt : IAddable<MyInt>
	{
		private int Value { get; }
		public static readonly IAddable<MyInt> Default = new MyInt(0);

		public MyInt(int value)
		{
			Value = value;
		}
		public IAddable<MyInt> Add(IAddable<MyInt> other)
		{
			if (other is MyInt i)
				return new MyInt(Value + i.Value);
			
			throw new ArgumentException(other.ToString());
		}

		public override string ToString()
		{
			return Value + "B, " + Value / 1024 + "KB, " + Value / 1024 / 1024 + "MB";
		}
	}
	
	public class FileSizeInterpreter : IFileInterpreter<string, MyInt>
	{

		public (string, IAddable<MyInt>)? Interpret(string fileExtension, IEnumerable<string> lines)
		{
			var sum = lines.Sum(line => line.Length);

			return (fileExtension, new MyInt(sum));
		}

		public bool SupportsFileExtension(string fileExtension)
		{
			return true;
		}
	}
}