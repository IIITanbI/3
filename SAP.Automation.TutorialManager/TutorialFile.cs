namespace SAP.Automation.TutorialManager
{
    using QA.TestLibs.XmlDesiarilization;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [XmlType("Tutorial file")]
    public class TutorialFile : XmlBaseType
    {
        [XmlProperty("Tutorial file name")]
        public string Name { get; set; }

        [XmlProperty("Tutorial title")]
        public string Title { get; set; }

        [XmlProperty("Tutorial description")]
        public string Description { get; set; }

        [XmlProperty("Tutorial tags")]
        [XmlChildLocation("tag")]
        public List<string> Tags { get; set; }

        [XmlProperty("Tutorial content")]
        public string Content { get; set; }
    }
}
