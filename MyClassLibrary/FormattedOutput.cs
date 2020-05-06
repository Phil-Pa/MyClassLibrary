using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MyClassLibrary
{
    public class FormattedOutput
    {

        private readonly StringBuilder sb = new StringBuilder();
        private readonly List<string> rows = new List<string>();
        private const string Space = " ";
        private readonly int numSpaces;

        public FormattedOutput(int numSpaces)
        {
            this.numSpaces = numSpaces;
        }

        public void Clear() => sb.Clear();

        public void AddRow(params string[] values)
        {
            // reserve space
            var filledValues = CheckAndFillValues(values);

            var capacity = values.Length * numSpaces;
            var rowBuilder = new StringBuilder(capacity);

            FillRowBuilder(capacity, rowBuilder);

            var offset = 0;

            for (var i = 0; i < filledValues.Count; i++)
            {
                var value = filledValues[i];
                for (var j = 0; j < value.Length; j++)
                {
                    var ch = value[j];
                    rowBuilder[offset + j] = ch;
                }

                offset += numSpaces;
            }

            rows.Add(rowBuilder.ToString());
        }

        public void AddEmptyRow()
        {
            rows.Add(string.Empty);
        }

        private static void FillRowBuilder(int capacity, StringBuilder rowBuilder)
        {
            for (var i = 0; i < capacity; i++)
            {
                rowBuilder.Append(Space);
            }
        }

        private List<string> CheckAndFillValues(string[] values)
        {
            var list = new List<string>();
            for (var i = 0; i < values.Length; i++)
            {
                var value = values[i];
                if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                {
                    list.Add(new string(' ', numSpaces));
                }
                else
                {
                    var sb = new StringBuilder(numSpaces);
                    sb.Append(value);
                    var length = value.Length;
                    for (var j = length; j < numSpaces; j++)
                    {
                        sb.Append(Space);
                    }

                    list.Add(sb.ToString());
                }
            }

            return list;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var row in rows)
            {
                sb.Append(row).Append('\n');
            }

            return sb.ToString();
        }
    }
}
