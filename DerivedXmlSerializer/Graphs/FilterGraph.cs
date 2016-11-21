using System;
using System.Collections.Generic;
using System.Linq;

namespace DerivedXmlSerializer.Graphs
{
    /// <summary>
    /// Filters vertices of the given graph by predicate.
    /// </summary>
    /// <typeparam name="T">Type of vertices.</typeparam>
    class FilterGraph<T> : IGraph<T>
    {
        private readonly IGraph<T> _inner;
        private readonly Func<T, bool> _predicate;

        public FilterGraph(IGraph<T> inner, Func<T, bool> predicate)
        {
            if (inner == null)
                throw new ArgumentNullException("inner");
            if (predicate == null)
                throw new ArgumentNullException("predicate");
            _inner = inner;
            _predicate = predicate;
        }

        public IEnumerable<T> GetAdjacent(T vertex)
        {
            if (vertex == null || !_predicate(vertex))
                return Enumerable.Empty<T>();
            return _inner.GetAdjacent(vertex).Where(_predicate);
        }
    }
}