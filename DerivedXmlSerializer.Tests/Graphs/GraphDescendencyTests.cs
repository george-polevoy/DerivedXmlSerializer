using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace DerivedXmlSerializer.Graphs
{
    public class GraphDescendencyTests
    {
        // a => b
        // a => c
        // b => d
        // d => e
        // f => g

        private class TestAncestry : IGraph<string>
        {
            private readonly Dictionary<string, string> _ancestry = new Dictionary<string, string>
            {
                {"b", "a"},
                {"c", "a"},
                {"d", "b"},
                {"e", "d"},
                {"g", "f"}
            };

            public IEnumerable<string> GetAdjacent(string v)
            {
                string w;
                if (_ancestry.TryGetValue(v, out w))
                    yield return w;
            }
        }

        [Test]
        [TestCase("a", "b")]
        [TestCase("b", "d")]
        [TestCase("d", "e")]
        [TestCase("f", "g")]
        public void BuildsDescendencyGivenAncestry(string ancestor, string descendant)
        {
            var graph = new TestAncestry().ToDescendancy(new[] {"g", "e"}, StringComparer.InvariantCulture);

            var collection =
                graph.GetAdjacent(ancestor);

            CollectionAssert.Contains(collection, descendant);
        }

        [Test]
        public void WalksAncestryWithDepthFirstSearch()
        {
            var items = new TestAncestry().DepthFirstSearch("e", MarkerFactory.CreateMarker<string>).ToList();

            CollectionAssert.AreEqual(new[] { "e", "d", "b", "a" }, items);
        }
    }
}