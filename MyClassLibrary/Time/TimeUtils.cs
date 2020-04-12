using System;
using System.Diagnostics;

namespace MyClassLibrary.Time
{
	public static class TimeUtils
	{
		private static TimeSpan Max2(in TimeSpan t1, in TimeSpan t2)
		{
			return t1 > t2 ? t1 : t2;
		}

		public static TimeSpan Max(params TimeSpan[] timeSpans)
		{
			Debug.Assert(timeSpans.Length >= 2);
			var max = timeSpans[0];

			for (int i = 1; i < timeSpans.Length; i++)
			{
				max = Max2(max, timeSpans[i]);
			}

			return max;
		}

		private static TimeSpan Min2(in TimeSpan t1, in TimeSpan t2)
		{
			return t1 > t2 ? t2 : t1;
		}

		public static TimeSpan Min(params TimeSpan[] timeSpans)
		{
			Debug.Assert(timeSpans.Length >= 2);
			var min = timeSpans[0];

			for (int i = 1; i < timeSpans.Length; i++)
			{
				min = Min2(min, timeSpans[i]);
			}

			return min;
		}
	}
}