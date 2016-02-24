namespace SAP.Automation.TutorialManager
{
    using QA.TestLibs.XmlDesiarilization;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [Serializable]
    [XmlType("Tutorial file configuration")]
    public class TutorialFile : XmlBaseType
    {
        [XmlProperty("Tutorial file name")]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Attribute)]
        public string Name { get; set; }

        [XmlProperty("Tutorial title")]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Attribute)]
        public string Title { get; set; }

        [XmlProperty("Tutorial description")]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Attribute)]
        public string Description { get; set; }

        [XmlProperty("Tutorial tags")]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Attribute)]
        public List<string> Tags { get; set; }

        [XmlProperty("Tutorial content")]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Attribute)]
        public string Content { get; set; }
    }
}
