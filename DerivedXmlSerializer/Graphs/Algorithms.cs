using System;
using System.Collections.Generic;
using System.Linq;

namespace DerivedXmlSerializer.Graphs
{
    public static class Algorithms
    {
        public static IGraph<T> Where<T>(this IGraph<T> graph, Func<T, bool> predicate)
        {
            return new FilterGraph<T>(graph, predicate);
        }

        public static IEnumerable<T> DepthFirstSearch<T>(this IGraph<T> graph, T vertex, Func<IMarker<T>> createMarker)
        {
            return NonRecursiveDepthFirstSearch(graph, vertex, createMarker);
        }

        public static IEnumerable<T> DepthFirstSearchAcyclic<T>(IGraph<T> graph, T vertex)
        {
            return NonRecursiveDepthFirstSearch(graph, vertex, null);
        }

        /// <summary>
        /// Performs Depth First Search of a graph.
        /// </summary>
        /// <param name="graph">Graph.</param>
        /// <param name="vertex"></param>
        /// <param name="createMarker"></param>
        /// <returns>Returns a lazy enumeration of all of the found vertices.</returns>
        internal static IEnumerable<TVertex> NonRecursiveDepthFirstSearch<TVertex>(IGraph<TVertex> graph, TVertex vertex, Func<IMarker<TVertex>> createMarker)
        {
            var context = createMarker == null ? null : createMarker();
            var s = new Stack<TVertex>();
            s.Push(vertex);
            while (s.Count > 0)
            {
                var current = s.Pop();
                if (context != null)
                {
                    if (context.IsMarked(current)) continue;
                    context.Mark(current);
                }
                yield return current;
                foreach (var w in graph.GetAdjacent(current)) s.Push(w);
            }
        }

        /// <summary>
        /// Returns a graph with all of the vertices and edges from both graphs.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a">One of the graphs in the union operation.</param>
        /// <param name="b">Another graph in the union operation.</param>
        /// <returns></returns>
        public static IGraph<T> Concat<T>(this IGraph<T> a, IGraph<T> b)
        {
            return new GraphUnion<T>(a, b);
        }
        
        public static IEnumerable<T> WalkAncestry<T>(this IGraph<T> g, T v)
        {
            var current = v;
            do
            {
                using (var i = g.GetAdjacent(current).GetEnumerator())
                {
                    if (!i.MoveNext())
                        yield break;
                    if (i.MoveNext())
                        throw new InvalidOperationException("Ancestry graph lead to multiple parents, which is not allowed");
                    current = i.Current;
                    yield return current;
                }
            } while (true);
        }

        /// <summary>
        /// Computes descendency graph. In such a graph, for any given vertex, all of the descendants in the given ancestry graph are adjacent in the resulting graph.
        /// That is, from any ancestor of the given ancestry, one could walk to any descendant in one step.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ancestry">For any given vertex, every adjacent vertex is in child-parent relation with the given vertex.</param>
        /// <param name="descendants">Vertices which will be used as a starting points of search of relations, as the adjacency graph only gives relation to one ancestor up.</param>
        /// <param name="equalityComparer">Equality comparer for label identity.</param>
        /// <returns>Returns a dependency graph in an explicit form.</returns>
        public static IGraph<T> ToDescendancy<T>(this IGraph<T> ancestry, IEnumerable<T> descendants,
            IEqualityComparer<T> equalityComparer)
        {
            return (
                from endDescendant in descendants
                from descendant
                in Enumerable.Repeat(endDescendant, 1).Concat(ancestry.WalkAncestry(endDescendant))
                from ancestor in ancestry.WalkAncestry(descendant)
                select new {ancestor, intermediaryDescendant = descendant}
            ).ToLookup(i => i.ancestor, i => i.intermediaryDescendant).AsExplicitGraph();
        }

        public static IGraph<T> AsExplicitGraph<T>(this ILookup<T, T> items)
        {
            return new InstantiatedGraph<T>(items);
        }
    }
}
