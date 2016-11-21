using System.Collections.Generic;

namespace DerivedXmlSerializer.Graphs
{
    public interface IGraph<TVertex>
    {
        IEnumerable<TVertex> GetAdjacent(TVertex vertex);
    }
}