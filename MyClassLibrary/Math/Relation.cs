using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyClassLibrary.Math
{
    public class Relation : HashSet<(int, int)>
    {

        public bool IsReflexive
        {
            get
            {
                return GetDistinctElements().All(element => Contains((element, element)));
            }
        }

        public bool IsIrreflexive => !IsReflexive;

        public bool IsSymmetric
        {
            get
            {
                return GetDistinctPairs().All(pair => 
                    Logic.Implication(Contains((pair.Item1, pair.Item2)), Contains((pair.Item2, pair.Item1))));
            }
        }

        public bool IsAntiSymmetric
        {
            get
            {
                return GetDistinctPairs().All(pair =>
                    Logic.Implication(Contains((pair.Item1, pair.Item2)) && Contains((pair.Item2, pair.Item1)),
                        pair.Item1 == pair.Item2));
            }
        }

        public bool IsTransitive
        {
            get
            {
                return GetDistinctTriples().All(triple =>
                    Logic.Implication(Contains((triple.Item1, triple.Item2)) &&
                                      Contains((triple.Item2, triple.Item3)), triple.Item1 == triple.Item3));
            }
        }

        // total, if every element in R is in relation with every element in R
        /// <summary>
        /// Is always true, because every added pair satifies a relation aRb or bRa
        /// </summary>
        public bool IsTotal
        {
            get
            {
                return GetDistinctPairs().All(pair =>
                    Logic.Implication(pair.Item1 != pair.Item2, Contains((pair.Item1, pair.Item2)) || Contains((pair.Item2, pair.Item1))));
            }
        }

        private IEnumerable<int> GetDistinctElements()
        {
            var aSet = this.Select(pair => pair.Item1).Distinct().ToList();
            var bSet = this.Select(pair => pair.Item2).Distinct().ToList();

            var elements = new List<int>();
            elements.AddRange(aSet);
            elements.AddRange(bSet);

            return elements.Distinct();
        }

        private IEnumerable<(int, int)> GetDistinctPairs()
        {
            return this.Distinct();
        }

        private IEnumerable<(int, int, int)> GetDistinctTriples()
        {
            var triples = new List<(int, int, int)>();

            var pairs1 = GetDistinctPairs();
            var pairs2 = GetDistinctPairs();

            foreach (var pair1 in pairs1)
            {
                foreach (var pair2 in pairs2)
                {
                    if (pair1.Item2 == pair2.Item2)
                    {
                        triples.Add((pair1.Item1, pair2.Item1, pair2.Item2));
                    }
                }
            }

            return triples;
        }

        public void AddPair(int a, int b) => Add((a, b));

    }
}
