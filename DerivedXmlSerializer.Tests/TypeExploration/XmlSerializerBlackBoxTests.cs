using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using NUnit.Framework;

namespace DerivedXmlSerializer.TypeExploration
{
    /// <summary>
    /// Test some assumptions about the underlying XmlSerializer
    /// </summary>
    public class XmlSerializerBlackBoxTests
    {
        [Test]
        public void DoesNotExposeArrayGetter()
        {
            var serializer = new XmlSerializer(typeof(Container));

            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                serializer.Serialize(sw,  new Container() { ListGetter = { 456 }});
            }

            var output = sb.ToString();
            Assert.That(output.Contains("456"));
            Assert.That(!output.Contains("123"));
        }

        public class Container
        {

            public int[] ArrayGetter => new[] { 123 };

            public List<int> GetInternalListGetterStorage()
            {
                return listGetterStorage;
            }

            private readonly List<int> listGetterStorage = new List<int>();

            public List<int> ListGetter
            {
                get { return listGetterStorage; }
            }
        }
    }
}