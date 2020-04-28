using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace MyClassLibrary.FileSystem
{
    public static class FileSystemUtils
    {
        public static bool IsSubDirectory(string directory, string subDirectory)
        {
            if (directory.Length > subDirectory.Length)
                return false;

            if (directory == subDirectory)
                return false;
            
            if (subDirectory.Contains(directory))
                return true;

            var directorySeperator = GetDirectorySeperator(directory);
            var subDirectorySeperator = GetDirectorySeperator(subDirectory);
            
            if (directorySeperator != subDirectorySeperator)
                throw new ArgumentException("directory and subdirectory dont have the same directory seperator");

            for (var i = 0; i <= directory.Length; i++)
            {
                if (i < directory.Length)
                {
                    if (directory[i] != subDirectory[i])
                        return false;
                }
                else
                {
                    return subDirectory[i] == directorySeperator;
                }
            }
            
            throw new Exception("should never have reached this exception");
        }

        public static char GetDirectorySeperator(string directory)
        {
            var containsSlash = directory.Contains('/');
            var containsBackSlash = directory.Contains('\\');
            
            if (containsSlash && containsBackSlash)
                throw new ArgumentException("directory: " + directory + " must only contains one type of slash");

            if (containsSlash)
                return '/';
            if (containsBackSlash)
                return '\\';
            throw new ArgumentException("directory contains no directory seperator like / or \\");
        }

        public static void Copy(string sourceDirectory, string targetDirectory)
        {
            DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
            DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);

            CopyAll(diSource, diTarget);
        }

        private static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            // Copy each file into the new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }
    }
}