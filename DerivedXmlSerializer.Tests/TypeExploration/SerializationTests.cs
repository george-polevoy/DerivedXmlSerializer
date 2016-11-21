using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using DerivedXmlSerializer.TypeExploration.SampleTypes;
using NUnit.Framework;

namespace DerivedXmlSerializer.TypeExploration
{
    public class SerializationTests
    {
        [Test]
        public void CanSerializeDerivedUnreferencedType()
        {
            var allTypes = new[]
            {
                typeof(TestBaseContainer),
                typeof(TestDerivedContainer1),
                typeof(TestDerivedArrayItem1),
                typeof(TestDerivedArrayItem2),
            };
            var allSerializers = XmlSerializer.FromTypes(allTypes);
            var serializerDictionary = Enumerable.Range(0, allTypes.Length)
                .ToDictionary(i => allTypes[i], i => allSerializers[i]);

            var sampleObject = new TestDerivedContainer1
            {
                Items = new List<TestBaseArrayItem>
                {
                    new TestDerivedArrayItem1(),
                    new TestDerivedArrayItem2(),
                }
            };

            var serializer = serializerDictionary[typeof(TestBaseContainer)];

            serializer.Serialize(Console.Out, sampleObject);
        }
    }
}