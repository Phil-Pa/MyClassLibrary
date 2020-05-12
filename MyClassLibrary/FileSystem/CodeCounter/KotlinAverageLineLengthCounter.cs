using System;
using System.Collections.Generic;
using System.Text;

namespace MyClassLibrary.FileSystem.CodeCounter
{
    public class KotlinAverageLineLengthCounter : IFileInterpreter<Language, MyInt>
    {

        private int numFiles = 0;
        private int totalSum = 0;

        public int Result => totalSum / numFiles;

        public (Language, IAddable<MyInt>)? Interpret(string fileExtension, IList<string> lines)
        {
            var sum = 0;

            foreach (var line in lines)
            {
                var trimmedLine = line.TrimStart();
                if (trimmedLine.StartsWith("/") || trimmedLine.StartsWith("*"))
                    continue;
                sum += line.Length;
            }

            totalSum += sum / lines.Count;
            numFiles++;

            return (Language.Kotlin, new MyInt(sum / lines.Count));
        }

        public bool SupportsFileExtension(string fileExtension)
        {
            return fileExtension.EndsWith("kt");
        }
    }
}
