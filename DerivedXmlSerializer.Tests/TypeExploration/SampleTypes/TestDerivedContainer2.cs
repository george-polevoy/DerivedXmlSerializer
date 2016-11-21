using System.Collections.Generic;

namespace DerivedXmlSerializer.TypeExploration.SampleTypes
{
    public class TestDerivedContainer2 : TestBaseContainer
    {
        public string StringPropInDerivedContainer { get; set; }

        public int IntPropInDerivedContainer { get; set; }

        public List<TestBaseArrayItem> Items { get; set; }
    }
}