using MyClassLibrary.Time;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace UnitTestProject.Time
{
	public class TimeUtilsTest
	{

		[Fact]
		public void TestMinMax()
		{

			TimeSpan t1 = TimeSpan.FromSeconds(1);
			TimeSpan t2 = TimeSpan.FromSeconds(22);
			TimeSpan t3 = TimeSpan.FromSeconds(32);

			var result = TimeUtils.Max(t1, t2);
			Assert.Equal(t2, result);

			result = TimeUtils.Min(t1, t2);
			Assert.Equal(t1, result);

			result = TimeUtils.Min(t1, t3);
			Assert.Equal(t1, result);

			result = TimeUtils.Max(t2, t3);
			Assert.Equal(t3, result);
		}

	}
}
