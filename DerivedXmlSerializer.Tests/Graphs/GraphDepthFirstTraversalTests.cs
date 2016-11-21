using System;
using System.Collections.Generic;
using System.Linq;
using DerivedXmlSerializer.Graphs;
using NUnit.Framework;

namespace DerivedXmlSerializer
{
    public class GraphDepthFirstTraversalTests
    {
        private class TestGraph : IGraph<string>
        {
            private readonly ILookup<string, string> _storage;

            public TestGraph()
            {
                var connections = new[]
                {
                    "fruit, apple",
                    "fruit, banana",
                    "car, wheel",
                    "car, cargo",
                    "cargo, fruit",
                    "ship, cargo"
                };
                _storage = connections
                    .Select(s => s.Split(new [] {", "}, StringSplitOptions.RemoveEmptyEntries))
                    .Select(i => new {x = i[0], y = i[1]})
                    .ToLookup(i => i.x, i => i.y);
            }

            public IEnumerable<string> GetAdjacent(string vertex)
            {
                return _storage[vertex];
            }
        }

        [Test]
        public void TestGraphExploration()
        {
            CollectionAssert.AreEquivalent(
                new [] {"car", "cargo", "wheel", "fruit", "apple", "banana"},
                Algorithms.DepthFirstSearch(new TestGraph(), "car", MarkerFactory.CreateMarker<string>));
        }
    }
}
