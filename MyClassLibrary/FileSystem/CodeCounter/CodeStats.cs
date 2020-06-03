using MyClassLibrary.Math;
using System;

namespace MyClassLibrary.FileSystem.CodeCounter
{
	public readonly struct CodeStats : IAddable<CodeStats>
	{
		public int CodeLines { get; }
		public int CommentLines { get; }
		public int BlankLines { get; }
		public static IAddable<CodeStats> Default { get; } = new CodeStats(0, 0, 0);

		public CodeStats(int codeLines, int commentLines, int blankLines)
		{
			CodeLines = codeLines;
			CommentLines = commentLines;
			BlankLines = blankLines;
		}

		public static CodeStats operator +(CodeStats a, CodeStats b)
		{
			return new CodeStats(a.CodeLines + b.CodeLines, a.CommentLines + b.CommentLines, a.BlankLines + b.BlankLines);
		}


		public IAddable<CodeStats> Add(IAddable<CodeStats> other)
		{
			if (other is CodeStats stats)
				return this + stats;
			
			throw new ArgumentException(other.ToString() + "is not of type CodeStats");
		}

		public override string ToString()
		{
			return CodeLines + ", " + CommentLines + ", " + BlankLines;
		}
	}
}