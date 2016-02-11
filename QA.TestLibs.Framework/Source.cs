namespace QA.TestLibs.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using XmlDesiarilization;

    [XmlType("Source configuration")]
    public class Source : XmlBaseType
    {
        [XmlProperty("Source type. Xml, External, Generic or ExternalGeneric", IsRequired = false)]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Attribute, "sourceType", "itemSourceType")]
        public SourceType ItemSourceType { get; set; } = SourceType.Xml;

        [XmlProperty("Root element name")]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Value, "root", "rootElement")]
        [XmlConstraint("ItemSourceType", SourceType.External, SourceType.ExternalGeneric)]
        public string RootElementName { get; set; } = null;

        [XmlProperty("Root element name")]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Value)]
        [XmlConstraint("ItemSourceType", SourceType.External, SourceType.ExternalGeneric)]
        public string Path { get; set; } = null;

        [XmlProperty("XElement used as template for generation")]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Value, "xmlTemplate", "generationTemplate")]
        public XElement XmlSource { get; set; }
        
        public enum SourceType
        {
            Xml, External, Generic, ExternalGeneric
        }
    }
}
