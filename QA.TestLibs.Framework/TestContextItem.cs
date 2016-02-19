namespace QA.TestLibs.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using XmlDesiarilization;
    
    [XmlType("TestContext item config")]
    [XmlLocation("contextItem")]
    public class TestContextItem : Source
    {
        [XmlProperty("TypeName for test item")]
        [XmlLocation(XmlLocationType.Attribute, "typeName", "type")]
        public string TypeName { get; set; }

        [XmlProperty("TestItem config to desirialize")]
        [XmlLocation(XmlLocationType.Value)]
        public XElement ItemConfig { get; set; }

        public List<TestContextItem> Build()
        {
            switch (ItemSourceType)
            {
                case SourceType.Xml:
                    
                    return new List<TestContextItem> { this };

                case SourceType.External:

                    var doc = XDocument.Load(Path);
                    var xml = doc.Element(RootElementName);
                    var testContextItem = XmlParser.Parse<TestContextItem>(xml, true);

                    return testContextItem.Build();

                case SourceType.Generic:
                case SourceType.ExternalGeneric:

                    throw new NotImplementedException();

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
