using System;

namespace MyClassLibrary.TaskScheduling
{
	public readonly ref struct SchedulingInformation
	{

		public TimeSpan Duration { get; }

		public SchedulingInformation(in TimeSpan duration)
		{
			Duration = duration;
		}

	}
}
