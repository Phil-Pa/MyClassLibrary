using System;
using System.Collections.Generic;
using System.Linq;
using MyClassLibrary.FileSystem.CodeCounter;

namespace MyClassLibrary.FileSystem
{

	public readonly struct MyInt : IAddable<MyInt>, IComparable<MyInt>
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
			
			throw new ArgumentException(other.ToString() + " is not of type CodeStats");
		}

		public override string ToString()
		{
			return Value + "B, " + Value / 1024 + "KB, " + Value / 1024 / 1024 + "MB";
		}

        public int CompareTo(MyInt other)
        {
            return Value.CompareTo(other.Value);
        }
    }
	
	public class FileSizeInterpreter : IFileInterpreter<string, MyInt>
	{

		public (string, IAddable<MyInt>)? Interpret(string fileExtension, IList<string> lines)
        {
            if (!fileExtension.Contains('.'))
                fileExtension = "no_extension";

            if (fileExtension.StartsWith(".git"))
                fileExtension = ".gitfile";

            var sum = lines.Sum(t => t.Length);

            // var sum = lines.Sum(line => line.Length);

			return (fileExtension, new MyInt(sum));
		}

		public bool SupportsFileExtension(string fileExtension)
		{
			return true;
		}
	}
}