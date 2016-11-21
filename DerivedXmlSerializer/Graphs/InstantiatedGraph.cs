using System.Collections.Generic;
using System.Linq;

namespace DerivedXmlSerializer.Graphs
{
    class InstantiatedGraph<T> : IGraph<T>
    {
        private readonly ILookup<T, T> _items;

        public InstantiatedGraph(ILookup<T, T> items)
        {
            _items = items;
        }

        public IEnumerable<T> GetAdjacent(T vertex)
        {
            return _items[vertex];
        }
    }
}