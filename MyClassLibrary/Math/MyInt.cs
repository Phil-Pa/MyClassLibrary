using System;
using System.Collections.Generic;
using System.Text;

namespace MyClassLibrary.Math
{
    public class MyInt : IAddable<MyInt>, IComparable<MyInt>
    {
        private int Value { get; }
        public static readonly IAddable<MyInt> Default = new MyInt(0);

        public MyInt(int value)
        {
            Value = value;
        }
        public IAddable<MyInt> Add(IAddable<MyInt> other)
        {
            if (other is MyInt i)
                return new MyInt(Value + i.Value);

            throw new ArgumentException(other.ToString() + " is not of type CodeStats");
        }

        public override string ToString()
        {
            return Value + "B, " + Value / 1024 + "KB, " + Value / 1024 / 1024 + "MB";
        }

        public int CompareTo(MyInt other)
        {
            return Value.CompareTo(other.Value);
        }
    }
}
