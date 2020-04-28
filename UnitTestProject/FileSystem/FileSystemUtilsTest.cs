using System;
using MyClassLibrary.FileSystem;
using Xunit;

namespace UnitTestProject.FileSystem
{
    public class FileSystemUtilsTest
    {
        [Fact]
        public void TestIsSubDirectory()
        {
            
            Assert.True(FileSystemUtils.IsSubDirectory("E:/Test", "E:/Test/a"));
            Assert.True(FileSystemUtils.IsSubDirectory("E:/Test/a", "E:/Test/a/b"));
            Assert.True(FileSystemUtils.IsSubDirectory("E:/", "E:/Test"));
            
            Assert.True(FileSystemUtils.IsSubDirectory("E:\\Test", "E:\\Test\\a"));
            Assert.True(FileSystemUtils.IsSubDirectory("E:\\Test\\a", "E:\\Test\\a\\b"));
            Assert.True(FileSystemUtils.IsSubDirectory("E:\\", "E:\\Test"));
            
            Assert.False(FileSystemUtils.IsSubDirectory("E:/", "E:/"));
            Assert.False(FileSystemUtils.IsSubDirectory("E:/Test", "E:/Test"));
            Assert.False(FileSystemUtils.IsSubDirectory("E:/Test", "E:/a"));
            Assert.False(FileSystemUtils.IsSubDirectory("E:/Test", "E:/b"));
            Assert.False(FileSystemUtils.IsSubDirectory("E:/Test", "E:/b/a"));
            Assert.False(FileSystemUtils.IsSubDirectory("E:/Test/a", "E:/b/a"));

            Assert.Throws<ArgumentException>(() =>
            {
                FileSystemUtils.IsSubDirectory("E:\\Test/a", "E:\\Test\\a\\b");
            });
        }

        [Fact]
        public void TestGetDirectorySeperator()
        {
            Assert.Equal('/', FileSystemUtils.GetDirectorySeperator("E:/fds"));
            Assert.Equal('\\', FileSystemUtils.GetDirectorySeperator("E:\\fds"));

            Assert.Throws<ArgumentException>(() =>
            {
                FileSystemUtils.GetDirectorySeperator("E:/foo\\bar");
            });
        }
    }
}