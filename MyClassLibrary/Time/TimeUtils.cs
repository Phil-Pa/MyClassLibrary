using System;
using System.Collections.Generic;
using System.Text;

namespace MyClassLibrary.Time
{
	public class TimeUtils
	{
		public static TimeSpan Max(in TimeSpan t1, in TimeSpan t2)
		{
			return t1 > t2 ? t1 : t2;
		}

		public static TimeSpan Min(in TimeSpan t1, in TimeSpan t2)
		{
			return t1 > t2 ? t2 : t1;
		}
	}
}
