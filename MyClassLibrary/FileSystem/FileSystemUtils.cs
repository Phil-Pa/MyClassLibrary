using System;
using System.Diagnostics;
using System.Linq;

namespace MyClassLibrary.FileSystem
{
    public static class FileSystemUtils
    {
        private static readonly char[] DirectorySeperators = {'/', '\\'};
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
        
    }
}