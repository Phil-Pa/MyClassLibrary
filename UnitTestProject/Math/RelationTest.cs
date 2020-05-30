using System;
using System.Collections.Generic;
using System.Text;
using MyClassLibrary.Math;
using Xunit;

namespace UnitTestProject.Math
{
    public class RelationTest
    {

        [Fact]
        public void TestIsSymmetric()
        {
            var relation = new Relation();

            relation.AddPair(1, 2);
            Assert.False(relation.IsSymmetric);

            relation.AddPair(2, 1);
            Assert.True(relation.IsSymmetric);

            relation.AddPair(2, 3);
            Assert.False(relation.IsSymmetric);

            relation.AddPair(1, 3);
            Assert.False(relation.IsSymmetric);

            relation.AddPair(3, 2);
            Assert.False(relation.IsSymmetric);


        }

        [Fact]
        public void TestIsReflexive()
        {
            var relation = new Relation();

            relation.AddPair(1, 2);
            Assert.False(relation.IsReflexive);

            relation.AddPair(1, 1);
            Assert.False(relation.IsReflexive);

            relation.AddPair(2, 2);
            Assert.True(relation.IsReflexive);

            relation.AddPair(3, 2);
            Assert.False(relation.IsReflexive);

            relation.AddPair(3, 3);
            Assert.True(relation.IsReflexive);
        }

        [Fact]
        public void TestIsAntiSymmetric()
        {
            var relation = new Relation();

            relation.AddPair(1, 2);
            Assert.True(relation.IsAntiSymmetric);

            relation.AddPair(1, 1);
            Assert.True(relation.IsAntiSymmetric);

            relation.AddPair(2, 1);
            Assert.False(relation.IsAntiSymmetric);
        }

        [Fact]
        public void TestIsTransitive1()
        {
            var relation = new Relation();

            relation.AddPair(1, 1);
            Assert.True(relation.IsTransitive);

            relation.AddPair(1, 2);
            Assert.False(relation.IsTransitive);
        }

        [Fact]
        public void TestIsTransitive2()
        {
            var relation = new Relation();

            relation.AddPair(0, 1);
            relation.AddPair(1, 2);
            relation.AddPair(2, 0);

            Assert.True(relation.IsTransitive);
        }

        [Fact]
        public void TestIsTotal()
        {
            var random = new Random();
            var relation = new Relation();

            relation.AddPair(1, 1);
            Assert.True(relation.IsTotal);

            relation.AddPair(1, 2);
            Assert.True(relation.IsTotal);

            relation.AddPair(2, 2);
            Assert.True(relation.IsTotal);

            for (var i = 0; i < 100; i++)
            {
                relation.AddPair(random.Next(), random.Next());
                Assert.True(relation.IsTotal);
            }
        }

    }
}
