using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MyClassLibrary
{
    public class FormattedOutput
    {

        private readonly StringBuilder _sb = new StringBuilder();
        private List<string> _rows = new List<string>();
        private const char Space = ' ';
        private int _numSpaces;
        private readonly int _newOverflowSpaces;
        private int _numMaxWordsInRow;

        public FormattedOutput(in int numSpaces, in int newOverflowSpaces = 4)
        {
            _numSpaces = numSpaces;
            _newOverflowSpaces = newOverflowSpaces;
        }

        public void Clear() => _sb.Clear();

        public void AddRow(params string[] values)
        {

            var maxValueWordLength = values.Max(str => str.Length);

            if (maxValueWordLength >= _numSpaces)
            {
                RecalculateAllRows(maxValueWordLength + _newOverflowSpaces);
                // ReSharper disable once TailRecursiveCall
                AddRow(values);
                return;
            }

            _numMaxWordsInRow = System.Math.Max(_numMaxWordsInRow, values.Length);

            var capacity = _numMaxWordsInRow * _numSpaces;
            var rowBuilder = new StringBuilder(capacity);

            FillRowBuilder(capacity, rowBuilder);

            for (var i = 0; i < capacity; i += _numSpaces)
            {
                Debug.Assert(i % _numSpaces == 0);

                var index = i / _numSpaces;

                var outOfValueSize = index >= values.Length;

                if (AppendEmptySpaces(outOfValueSize, rowBuilder, i))
                    continue;

                var value = values[index];

                if (string.IsNullOrEmpty(value))
                    AppendEmptySpaces(outOfValueSize, rowBuilder, i);

                for (var j = 0; j < value.Length; j++)
                {
                    var ch = value[j];
                    rowBuilder[i + j] = ch;
                }

                for (var j = value.Length; j < _numSpaces; j++)
                {
                    rowBuilder[i + j] = Space;
                }
            }

            _rows.Add(rowBuilder.ToString());
        }

        private void RecalculateAllRows(in int newNumSpaces)
        {
            var newRows = new List<string>();

            var diffSpaces = newNumSpaces - _numSpaces;
            var rowCount = _rows.Count;

            for (var i = 0; i < rowCount; i++)
            {
                // insert every _numSpaces the missing spaces

                var tempSb = new StringBuilder(_rows[i], newNumSpaces);
                tempSb.Append(Space, diffSpaces);

                var numWords = _rows[i].Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;

                var count = 0;

                for (var j = _numSpaces;; j += _numSpaces)
                {

                    for (var k = 0; k < diffSpaces; k++)
                        tempSb.Insert(j, Space);

                    j += diffSpaces;

                    count++;

                    if (count + 1 >= numWords)
                        break;
                }

                newRows.Add(tempSb.ToString());
            }

            _rows.Clear();
            _rows = newRows;
            _numSpaces = newNumSpaces;
        }

        public void AddEmptyRow()
        {
            _rows.Add(string.Empty);
        }

        private bool AppendEmptySpaces(in bool outOfValueSize, StringBuilder stringBuilder, in int i)
        {
            if (!outOfValueSize)
                return false;

            for (var j = 0; j < _numSpaces; j++)
                stringBuilder[i + j] = Space;

            return true;

        }

        private static void FillRowBuilder(in int capacity, StringBuilder rowBuilder)
        {
            for (var i = 0; i < capacity; i++)
            {
                rowBuilder.Append(Space);
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder(_rows.Count * _numSpaces * _numMaxWordsInRow);

            foreach (var row in _rows)
            {
                sb.Append(row).Append('\n');
            }

            return sb.ToString();
        }
    }
}