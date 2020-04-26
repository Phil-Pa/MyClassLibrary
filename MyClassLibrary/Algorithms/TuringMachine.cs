using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace MyClassLibrary.Algorithms
{

	public enum StateMoveOption
	{
		Left,
		Right,
		None,
		Stop
	}

	public class Transition : IEquatable<Transition>
	{
		public int StartState { get; }
		public char ReadingChar { get; }
		public char WritingChar { get; }
		public StateMoveOption MoveOption { get; }
		public int EndState { get; }

		public Transition(int startState, char readingChar, char writingChar, StateMoveOption moveOption, int endState)
		{
			StartState = startState;
			ReadingChar = readingChar;
			WritingChar = writingChar;
			MoveOption = moveOption;
			EndState = endState;
		}

		private static readonly Regex CreateRegex = new Regex(@"(\d+) (\w) (\w) ([!lnr]) (\d+)");
		public static Transition Create(string str)
		{
			Match match = CreateRegex.Match(str);

			if (!match.Success)
				throw new Exception("str does not match regex for creation");

			var startState = int.Parse(match.Groups[1].Value);
			var readingChar = match.Groups[2].Value[0];
			var writingChar = match.Groups[3].Value[0];

			var optionChar = match.Groups[4].Value[0];

			var option = optionChar switch {
				'!' => StateMoveOption.Stop,
				'l' => StateMoveOption.Left,
				'r' => StateMoveOption.Right,
				_ => throw new Exception("invalid state move option")
			};

			var endState = int.Parse(match.Groups[5].Value);

			return new Transition(startState, readingChar, writingChar, option, endState);
		}

		/// <summary>
		/// Creates a list of transitions using a string
		/// </summary>
		/// <param name="str">str has to match the format "transition,transition,...,transition" where transition matches the regex "(\d+) (\w) (\w) ([!lnr]) (\d+)"</param>
		/// <returns></returns>
		public static IReadOnlyList<Transition> CreateTransitions(string str)
		{
			var stringTransitions = str.Split(',', StringSplitOptions.RemoveEmptyEntries);

			return stringTransitions.Select(Create).ToList();
		}

		#region Equals/==
		public static bool operator ==(Transition a, Transition b)
		{
			// ReSharper disable once PossibleNullReferenceException
			return a.Equals(b);
		}

		public static bool operator !=(Transition a, Transition b)
		{
			return !(a == b);
		}

		public bool Equals(Transition? other)
		{
			if (other is null) return false;
			if (ReferenceEquals(this, other)) return true;
			return StartState == other.StartState && ReadingChar == other.ReadingChar && WritingChar == other.WritingChar && MoveOption == other.MoveOption && EndState == other.EndState;
		}

		public override bool Equals(object? obj)
		{
			if (obj is null) return false;
			if (ReferenceEquals(this, obj)) return true;
			return obj.GetType() == GetType() && Equals((Transition) obj);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(StartState, ReadingChar, WritingChar, (int) MoveOption, EndState);
		}
		#endregion
	}

	public class TuringMachine
	{

		private readonly IReadOnlyList<int> _states;
		private readonly IReadOnlyList<int> _endStates;
		private readonly List<char> _band;
		private readonly IReadOnlyList<Transition> _transitions;

		private int _currentState;
		private Transition? _currentTransition;
		private int _position;
		private bool _stopped;

		private const char Blank = '_';

		public TuringMachine(
			List<char> band,
			IReadOnlyList<int> states,
			int startState,
			IReadOnlyList<int> endStates,
			IReadOnlyList<Transition> transitions)
		{
			_band = band;
			_states = states ?? throw new ArgumentNullException(nameof(states));
			_endStates = endStates ?? throw new ArgumentNullException(nameof(endStates));
			_transitions = transitions ?? throw new ArgumentNullException(nameof(transitions));

			_currentTransition = null;
			_currentState = startState;

			Validate();
		}

		private void Validate()
		{
			// only different states
			Debug.Assert(_states.Distinct().Count() == _states.Count);

			// verify that end states is a subset of states
			Debug.Assert(_endStates.All(state => _states.Contains(state)));
		}

		private void Move()
		{
			switch (_currentTransition?.MoveOption)
			{
				// move on band
				case StateMoveOption.Left:
					_position--;
					break;
				case StateMoveOption.Right:
					_position++;
					break;
				case StateMoveOption.None:
					// do nothing
					break;
				case StateMoveOption.Stop:
					_stopped = true;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public string Step()
		{
			if (_stopped)
				throw new Exception();

			if (_position >= _band.Count)
			{
				_band.Add(Blank);
			}
			else if (_position < 0)
			{
				_band.Insert(0, Blank);
				_position++;
			}

			// read char
			var readChar = _band[_position];

			// get current transition
			_currentTransition = _transitions.First(tr => tr.StartState == _currentState && tr.ReadingChar == readChar);

			// write char
			_band[_position] = _currentTransition.WritingChar;

			// make move
			Move();

			// set new current state
			_currentState = _currentTransition.EndState;

			if (_endStates.Contains(_currentState))
				_stopped = true;

			return string.Join(string.Empty, _band).Trim(Blank);
		}

		public string DoSteps()
		{
			while (!_stopped)
				Step();
			return string.Join(string.Empty, _band).Trim(Blank);
		}
	}
}
