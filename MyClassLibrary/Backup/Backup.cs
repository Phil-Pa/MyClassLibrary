using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using MyClassLibrary.FileSystem;

namespace MyClassLibrary.Backup
{
    public static class Backup
    {
        // Token: 0x060000C3 RID: 195 RVA: 0x000048A8 File Offset: 0x00002AA8
        public static string DoBackup(IEnumerable<string> directories, string outputDirectory, string aesCryptPath, string password)
        {
            string timeString = DateTime.Now.ToShortDateString();
            string tempDir = outputDirectory + "/temp";
            Directory.CreateDirectory(tempDir);
            string[] array = (directories as string[]) ?? Enumerable.ToArray<string>(directories);
            Task[] tasks = new Task[array.Length];
            int i = 0;
            string[] array2 = array;
            for (int j = 0; j < array2.Length; j++)
            {
                string directory = array2[j];
                string inDir = new DirectoryInfo(directory).Name;
                Task task = Task.Run(delegate ()
                {
                    FileSystemUtils.Copy(directory, tempDir + "/" + inDir);
                });
                tasks[i++] = task;
            }
            Task.WaitAll(tasks);
            string outputZipFile = outputDirectory + "/" + timeString + "-backup.zip";
            ZipFile.CreateFromDirectory(tempDir, outputZipFile);
            Directory.Delete(tempDir, true);
            string backupFile = outputZipFile + ".aes";
            string args = string.Concat(new string[]
            {
                "-e -p ",
                password,
                " -o \"",
                backupFile,
                "\" \"",
                outputZipFile,
                "\""
            });
            string result;
            using (Process process = Process.Start(new ProcessStartInfo
            {
                Arguments = args,
                FileName = aesCryptPath
            }))
            {
                process.WaitForExit();
                File.Delete(outputZipFile);
                result = backupFile;
            }
            return result;
        }

        // Token: 0x060000C4 RID: 196 RVA: 0x00004A2C File Offset: 0x00002C2C
        public static void UnpackBackup(string backupFile, string aesCryptPath, string password)
        {
            string outputZipFile = backupFile.Substring(0, backupFile.Length - 4);
            string destinationDirectoryName = outputZipFile.Substring(0, outputZipFile.Length - 4);
            if (Directory.Exists(destinationDirectoryName))
            {
                return;
            }
            string args = string.Concat(new string[]
            {
                "-d -p ",
                password,
                " -o \"",
                outputZipFile,
                "\" \"",
                backupFile,
                "\""
            });
            using (Process process = Process.Start(new ProcessStartInfo
            {
                Arguments = args,
                FileName = aesCryptPath
            }))
            {
                process.WaitForExit();
                File.Delete(backupFile);
                ZipFile.ExtractToDirectory(outputZipFile, destinationDirectoryName);
                File.Delete(outputZipFile);
            }
        }
    }
}