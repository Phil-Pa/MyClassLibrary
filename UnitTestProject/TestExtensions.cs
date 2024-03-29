﻿using System;
using System.Collections.Generic;
using Xunit;

using MyClassLibrary;
using Xunit.Abstractions;
using System.Linq;

namespace UnitTestProject
{
	public class TestExtensions
	{

		private readonly ITestOutputHelper _output;

		public TestExtensions(ITestOutputHelper output)
		{
			_output = output;
		}

		[Fact]
		public void TestPermutations()
		{

			var list = new List<int>
			{
				1, 2, 3, 4
			};

			for (var i = 2; i < 10; i++)
			{
				var permutations = list.GetPermutations(list.Count);
				_output.WriteLine(permutations.Count().ToString());
			}
			
		}

		[Fact]
		public void TestContainsOnly()
		{

			var list = new List<int>
			{
				2, 2, 2, 2
			};

			Assert.True(list.ContainsOnly(2));
			
			list.Add(3);
			
			Assert.False(list.ContainsOnly(2));
		}

        class ClonableObject : ICloneable
        {
            public object Value { get; set; }
            public object Clone()
            {
                return new ClonableObject
                {
                    Value = Value
                };
            }
        }

        [Fact]
        public void TestClone()
        {
            var list = new List<ClonableObject>();
            for (var i = 0; i < 10; i++)
                list.Add(new ClonableObject
                {
                    Value = i
                });

            var clonedList = list.Clone();
            
            Assert.Equal(list.Count, clonedList.Count);

            for (var i = 0; i < list.Count; i++)
            {
                var obj1 = list[i];
                var obj2 = clonedList[i];

                Assert.Equal(obj1.Value, obj2.Value);
            }
        }

		[Fact]
		public void TestToInt()
		{
			const string str = "1232";
			Assert.Equal(1232, str.ToInt());

			Assert.Throws<ArgumentException>(() =>
			{
				"283ha34".ToInt();
			});
		}

		[Fact]
		public void TestGetFileExtension()
		{
			Assert.Null("fdskfds".GetFileExtension());
			Assert.Equal(".cs", "fds.cs".GetFileExtension());
			Assert.Equal(".gitignore", ".gitignore".GetFileExtension());
			Assert.Equal(".cs", "fjdks.xaml.fmds.cs".GetFileExtension());
		}
	}
}
