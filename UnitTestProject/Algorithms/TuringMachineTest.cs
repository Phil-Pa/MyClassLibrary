using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyClassLibrary.Algorithms;
using Xunit;

namespace UnitTestProject.Algorithms
{
	public class TuringMachineTest
	{

		[Fact]
		public void Test()
		{

			List<int> states = new List<int> {1, 2, 3};
			List<int> endStates = new List<int> {3};

			var band = "aababaa".ToList();

			List<Transition> transitions = new List<Transition> {
				new Transition(1, '_', '_', StateMoveOption.Stop, 1),
				new Transition(1, 'a', 'a', StateMoveOption.Right, 1),
				new Transition(1, 'b', 'a', StateMoveOption.Right, 2),
				new Transition(2, 'a', 'b', StateMoveOption.Right, 2),
				new Transition(2, 'b', 'b', StateMoveOption.Right, 3)
			};

			TuringMachine turingMachine = new TuringMachine(band, states, 1, endStates, transitions);

			string output = turingMachine.DoSteps();

			Assert.Equal("aaabbaa", output);
		}

		[Fact]
		public void TestDouble()
		{
			List<int> states = new List<int> { 0, 1, 2, 3, 4 };
			List<int> endStates = new List<int>();

			List<Transition> transitions = new List<Transition> {
				new Transition(0, '_', '_', StateMoveOption.Stop, 0),
				new Transition(0, 'a', 'b', StateMoveOption.Right, 1),
				new Transition(1, 'a', 'a', StateMoveOption.Right, 1),
				new Transition(1, 'c', 'c', StateMoveOption.Right, 1),
				new Transition(1, '_', 'c', StateMoveOption.None, 2),
				new Transition(2, 'c', 'c', StateMoveOption.Left, 2),
				new Transition(2, 'a', 'a', StateMoveOption.Left, 2),
				new Transition(2, 'b', 'b', StateMoveOption.Right, 0),
				new Transition(0, 'c', 'c', StateMoveOption.Right, 3),
				new Transition(3, 'c', 'c', StateMoveOption.Right, 3),
				new Transition(3, '_', '_', StateMoveOption.Left, 4),
				new Transition(4, 'c', 'a', StateMoveOption.Left, 4),
				new Transition(4, 'b', 'a', StateMoveOption.Left, 4),
				new Transition(4, '_', '_', StateMoveOption.Stop, 4)
			};

			for (int i = 1; i <= 10; i++)
			{
				var sb = new StringBuilder();
				for (int j = 0; j < i; j++)
					sb.Append('a');

				var band = sb.ToString().ToList();
				TuringMachine turingMachine = new TuringMachine(band, states, 0, endStates, transitions);

				for (int j = 0; j < i; j++)
					sb.Append('a');

				var result = sb.ToString();

				string output = turingMachine.DoSteps();

				Assert.Equal(result, output);
			}
		}

		[Fact]
		public void TestCreateTransitionWithString()
		{
			Transition tran = Transition.Create("1 a a l 1");
			Transition result = new Transition(1, 'a', 'a', StateMoveOption.Left, 1);
			Assert.True(result == tran);

			var transitions = Transition.CreateTransitions("1 a a l 1,1 a a l 1,1 a a l 1,1 a a l 1");
			Assert.Equal(4, transitions.Count);

			for (int i = 0; i < 4; i++)
				Assert.Equal(tran, transitions[i]);

			tran = Transition.Create("11 h d r 34");
			result = new Transition(11, 'h', 'd', StateMoveOption.Right, 34);
			Assert.True(result == tran);
		}
	}
}
