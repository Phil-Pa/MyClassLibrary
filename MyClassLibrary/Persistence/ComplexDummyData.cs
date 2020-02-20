using System;
using System.Collections.Generic;
using System.Text;

namespace MyClassLibrary.Persistence
{
	public class ComplexDummyData
	{

		public DummyData Dummy { get; set; }

		public bool MyCondition { get; set; }

		public ComplexDummyData(DummyData dummy, bool myCondition)
		{
			Dummy = dummy ?? throw new ArgumentNullException(nameof(dummy));
			MyCondition = myCondition;
		}
	}
}
