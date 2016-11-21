using System.Collections.Generic;
using System.Linq;

namespace DerivedXmlSerializer.Graphs
{
    internal class GraphUnion<T> : IGraph<T>
    {
        private readonly IGraph<T> _a;
        private readonly IGraph<T> _b;

        public GraphUnion(IGraph<T> a, IGraph<T> b)
        {
            _a = a;
            _b = b;
        }

        public IEnumerable<T> GetAdjacent(T vertex)
        {
            return _a.GetAdjacent(vertex).Concat(_b.GetAdjacent(vertex)).Distinct();
        }
    }
}