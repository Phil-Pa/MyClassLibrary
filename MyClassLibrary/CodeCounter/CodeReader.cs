using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MyClassLibrary.CodeCounter
{
	public class CodeReader : IFileInterpreter<Language, CodeStats>
	{

		private Language _language;

		private static bool IsEmptyLine(string line)
		{
			return line.ContainsOnly(' ') || line == string.Empty;
		}

		private static bool IsCommentEnd(string line)
		{
			return line.TrimEnd().EndsWith("*/");
		}

		private static bool IsCommentStart(string line)
		{
			return line.TrimStart().StartsWith("/*");
		}

		private static bool IsSingleCommentLine(string line)
		{
			return line.TrimStart().StartsWith("//");
		}

		public (Language, IAddable<CodeStats>)? Interpret(string fileExtension, IEnumerable<string> lines)
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
				case ".kt":
					_language = Language.Kotlin;
					break;
				case ".cs":
					_language = Language.CSharp;
					break;
				case ".cpp":
					_language = Language.Cpp;
					break;
				default:
					return null;
			}
			
			
			int commentLines = 0, blankLines = 0, codeLines = 0;

			var isInMultiCommentLine = false;

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

			Debug.Assert(enumerable.Count == commentLines + blankLines + codeLines);

			return (_language, new CodeStats(codeLines, commentLines, blankLines));
		}

		private readonly string[] fileExtensions = new[] {".c", ".cpp", ".h", ".hpp", ".java", ".kt", ".cs"};

		public bool SupportsFileExtension(string fileExtension)
		{
			return fileExtensions.Contains(fileExtension);
		}
	}
}