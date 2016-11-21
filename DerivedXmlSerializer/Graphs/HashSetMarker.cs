using System.Collections.Generic;

namespace DerivedXmlSerializer.Graphs
{
    public static class MarkerFactory
    {
        public static IMarker<T> CreateMarker<T>()
        {
            return new HashSetMarker<T>();
        }

        public static IMarker<T> CreateMarker<T>(IEqualityComparer<T> equalityComparer)
        {
            return new HashSetMarker<T>(equalityComparer);
        }
    }

    internal class HashSetMarker<T> : IMarker<T>
    {
        private readonly HashSet<T> _hashSet;

        internal HashSetMarker()
        {
            _hashSet = new HashSet<T>();
        }

        internal HashSetMarker(IEqualityComparer<T> equalityComparer)
        {
            _hashSet = new HashSet<T>(equalityComparer);
        }

        public bool Mark(T v)
        {
            return _hashSet.Add(v);
        }

        public bool IsMarked(T v)
        {
            return _hashSet.Contains(v);
        }
    }
}