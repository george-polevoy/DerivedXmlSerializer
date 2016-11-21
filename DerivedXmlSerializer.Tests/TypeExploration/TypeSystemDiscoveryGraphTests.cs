using System;
using System.Collections.Generic;
using System.Linq;
using DerivedXmlSerializer.Graphs;
using DerivedXmlSerializer.TypeExploration.SampleTypes;
using NUnit.Framework;

namespace DerivedXmlSerializer.TypeExploration
{
    class TypeSystemDiscoveryGraphTests
    {
        [Test]
        public void DiscoversPublicProperties()
        {
            IEnumerable<Type> allTypes =
                typeof(string).Assembly.GetTypes().Concat(new[] {typeof(TestDerivedContainer1), typeof(TestBaseArrayItem)});
            var typeDiscovery = TypeSystemDiscoveryGraph.Create(allTypes);
            var enumerable = typeDiscovery.DepthFirstSearch(typeof(TestBaseContainer), MarkerFactory.CreateMarker<Type>);
            var actual = enumerable.ToArray();

            foreach (var type in actual)
            {
                Console.WriteLine(type);
            }
        }
    }
}