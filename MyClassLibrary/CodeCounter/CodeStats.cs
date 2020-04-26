using System;

namespace MyClassLibrary.CodeCounter
{
	public readonly struct CodeStats : IAddable<CodeStats>
	{
		public int CodeLines { get; }
		public int CommentLines { get; }
		public int BlankLines { get; }

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


		public CodeStats Add(IAddable<CodeStats> other)
		{
			if (other is CodeStats stats)
				return this + stats;
			
			throw new ArgumentException(other.ToString());
		}
	}
}