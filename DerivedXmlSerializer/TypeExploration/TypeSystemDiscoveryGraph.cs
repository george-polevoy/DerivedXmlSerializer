using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using DerivedXmlSerializer.Graphs;

namespace DerivedXmlSerializer.TypeExploration
{
    /// <summary>
    /// Represents ancestry of the .NET type system.
    /// </summary>
    class TypeAncestry : IGraph<Type>
    {
        public IEnumerable<Type> GetAdjacent(Type vertex)
        {
            var baseType = vertex.BaseType;
            if (baseType != null)
                yield return baseType;
        }
    }

    public static class TypeSystemDiscoveryGraph
    {
        public static IGraph<Type> Create(IEnumerable<Type> allTypes)
        {
            return
                new TypeAncestry()
                 .ToDescendancy(allTypes, EqualityComparer<Type>.Default)
                 .Concat(new DirectMemberTypeRelations())
                 ;
        }
    }

    public class DirectMemberTypeRelations : IGraph<Type>
    {
        public IEnumerable<Type> GetAdjacent(Type vertex)
        {
            return vertex
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .SelectMany(pi => ExpandType(pi.PropertyType));
        }

        private static IEnumerable<Type> ExpandType(Type type)
        {
            yield return type;

            if (type.IsConstructedGenericType)
                foreach (var genericArgument in type.GetGenericArguments())
                    yield return genericArgument;
        }
    }
}
