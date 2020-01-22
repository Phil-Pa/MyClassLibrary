using System;

namespace MyClassLibrary.Math
{
	public readonly struct Fraction : IComparable<Fraction>
	{

		public int Numerator { get; }
		public int Denominator { get; }

		public Fraction(in int numerator, in int denominator)
		{
			if (numerator == 0 || denominator == 0)
				throw new ArgumentException("arguments must not be 0");

			Numerator = numerator;
			Denominator = denominator;
		}

		public float ToFloat()
		{
			return (float) Numerator / Denominator;
		}

		public static bool operator==(in Fraction a, in Fraction b)
		{
			return System.Math.Abs(System.Math.Abs(a.ToFloat()) - System.Math.Abs(b.ToFloat())) < float.Epsilon;
		}

		public static bool operator !=(in Fraction a, in Fraction b)
		{
			return !(a == b);
		}

		public static bool operator <(in Fraction a, in Fraction b)
		{
			return a.ToFloat() < b.ToFloat();
		}

		public static bool operator >(in Fraction a, in Fraction b)
		{
			return a.ToFloat() > b.ToFloat();
		}

		public bool Equals(in Fraction other)
		{
			return Numerator == other.Numerator && Denominator == other.Denominator;
		}

		public int CompareTo(Fraction other)
		{
			if (this < other)
				return -1;
			else if (this == other)
				return 0;
			else // (this > other)
				return 1;
		}

		public override bool Equals(object? obj)
		{
			return obj is Fraction other && Equals(other);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Numerator, Denominator);
		}

	}
}
