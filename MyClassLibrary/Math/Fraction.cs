using System;
using System.Diagnostics;

namespace MyClassLibrary.Math
{
	/// <summary>
	/// Represents a mathematical fraction
	/// </summary>
	[DebuggerDisplay("{" + nameof(Numerator) + "}/{" + nameof(Denominator) + "}")]
	public readonly struct Fraction : IComparable<Fraction>
	{
		public int Numerator { get; }
		public int Denominator { get; }

		public Fraction(in int numerator, in int denominator)
		{
            if (denominator == 0)
                throw new ArgumentException("arguments must not be 0");

            Numerator = numerator;
			Denominator = denominator;
		}

		public static Fraction Of(int num)
		{
			return new Fraction(num, 1);
		}

		public Fraction Simplify()
		{
			var gdc = Math.GCD(Numerator, Denominator);
			var numerator = Numerator / gdc;
			var denominator = Denominator / gdc;

			if (numerator < 0 && denominator < 0)
			{
				numerator = System.Math.Abs(numerator);
				denominator = System.Math.Abs(denominator);
			}

			return new Fraction(numerator, denominator);
		}

		public float ToFloat()
		{
			return (float)Numerator / Denominator;
		}

        public bool IsInteger()
        {
            return Numerator % Denominator == 0;
        }

        public int ToInt()
        {
            if (!IsInteger())
                throw new Exception("fraction must be integer to be converted to an integer");
            return Numerator / Denominator;
        }

		public static bool operator ==(in Fraction a, in Fraction b)
		{
			var simpleA = a.Simplify();
			var simpleB = b.Simplify();
			return System.Math.Abs(simpleA.Numerator) == System.Math.Abs(simpleB.Numerator) &&
				System.Math.Abs(simpleA.Denominator) == System.Math.Abs(simpleB.Denominator);
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

        public static Fraction operator+(in Fraction a, in Fraction b)
        {
            var newDenominator = a.Denominator * b.Denominator;
            var newNumerator = a.Numerator * b.Denominator + b.Numerator * a.Denominator;
            return new Fraction(newNumerator, newDenominator).Simplify();
        }

        public static Fraction operator-(in Fraction a, in Fraction b)
        {
            // TODO: test
            var newDenominator = a.Denominator * b.Denominator;
            var newNumerator = a.Numerator * b.Denominator - b.Numerator * a.Denominator;
            return new Fraction(newNumerator, newDenominator).Simplify();
        }

        public static Fraction operator *(in Fraction a, in Fraction b)
        {
            var newNumerator = a.Numerator * b.Numerator;
            var newDenominator = a.Denominator * b.Denominator;
            return new Fraction(newNumerator, newDenominator).Simplify();
        }

        public static Fraction operator /(in Fraction a, in Fraction b)
        {
            return a * new Fraction(b.Denominator, b.Numerator);
        }

        private bool Equals(in Fraction other)
		{
			return this == other;
		}

		public int CompareTo(Fraction other)
		{
			if (this < other)
				return -1;
			return this == other ? 0 : 1;
		}

		public override bool Equals(object? obj)
		{
			return obj is Fraction other && Equals(other);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Numerator, Denominator);
		}

        public override string ToString()
        {
            if (IsInteger())
                return Numerator.ToString();
            else
                return Numerator + "/" + Denominator;
        }

        public static Fraction Random(in int min = 1, in int max = 10, bool canBeNegative = true)
        {
            var random = new Random();
            var numerator = random.Next(min, max);
            var denominator = random.Next(min, max);

            if (canBeNegative)
            {
                if (random.NextBool())
                    numerator = -numerator;
                if (random.NextBool())
                    denominator = -denominator;
            }

            return new Fraction(numerator, denominator).Simplify();
        }
    }
}