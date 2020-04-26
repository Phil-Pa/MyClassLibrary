using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MyClassLibrary.CodeCounter
{
	internal class CodeReader : IFileInterpreter<CodeStats>
	{
		private readonly Language _language;
		private readonly IEnumerable<string> _lines;

		public CodeReader(Language language, IEnumerable<string> lines)
		{
			_language = language;
			_lines = lines;
		}

		public IAddable<CodeStats> Interpret(IEnumerable<string> lines)
		{
			int commentLines = 0, blankLines = 0, codeLines = 0;

			bool isInMultiCommentLine = false;

			var enumerable = lines.ToList();
			foreach (var line in enumerable)
			{
				if (string.IsNullOrEmpty(line))
				{
					blankLines++;
					continue;
				}

				if (IsSingleCommentLine(line))
				{
					commentLines++;
					continue;
				}

				bool isCommentStart = IsCommentStart(line);
				bool isCommentEnd = IsCommentEnd(line);

				if (isCommentStart && !isCommentEnd)
				{
					isInMultiCommentLine = true;
				}
				else if (!isCommentStart && isCommentEnd)
				{
					isInMultiCommentLine = false;
				}

				if (IsEmptyLine(line))
					blankLines++;
				else if (isInMultiCommentLine || (isCommentStart && isCommentEnd))
					commentLines++;
				else
					codeLines++;
			}

			Debug.Assert(enumerable.Count() == commentLines + blankLines + codeLines);

			return new CodeStats(codeLines, commentLines, blankLines);
		}

		private bool IsEmptyLine(string line)
		{
			throw new System.NotImplementedException();
		}

		private bool IsCommentEnd(string line)
		{
			throw new System.NotImplementedException();
		}

		private bool IsCommentStart(string line)
		{
			throw new System.NotImplementedException();
		}

		private bool IsSingleCommentLine(string line)
		{
			throw new System.NotImplementedException();
		}
	}
}