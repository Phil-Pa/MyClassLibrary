using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MyClassLibrary.CodeCounter
{
	internal class CodeReader : IFileInterpreter<Language, CodeStats>
	{

		private Language _language;
		public CodeReader()
		{
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

		public (Language, IAddable<CodeStats>) Interpret(string fileExtension, IEnumerable<string> lines)
		{
			switch (fileExtension)
			{
				case ".c":
					_language = Language.C;
					break;
				case ".h":
				case ".hpp":
					_language = Language.CCppHeader;
					break;
				case ".java":
					_language = Language.Java;
					break;
				case ".cs":
					_language = Language.CSharp;
					break;
				case ".cpp":
					_language = Language.Cpp;
					break;
				default:
					throw new Exception();
			}
			
			
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

			return (language, new CodeStats(codeLines, commentLines, blankLines));
		}
	}
}