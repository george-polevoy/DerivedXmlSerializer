using System;
using System.Collections.Generic;

namespace DerivedXmlSerializer.Graphs
{
    interface IGraphSearch<TVertex>
    {
        IEnumerable<TVertex> DepthFirstSearch(IGraph<TVertex> graph, TVertex vertex, Func<IMarker<TVertex>> createMarker);
    }
}