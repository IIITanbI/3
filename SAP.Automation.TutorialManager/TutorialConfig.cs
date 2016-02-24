namespace SAP.Automation.TutorialManager
{
    using QA.TestLibs.XmlDesiarilization;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [Serializable]
    [XmlType("Tutorial configuration")]
    public class TutorialConfig : XmlBaseType
    {
        [XmlProperty("Tutorial folder")]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Attribute)]
        public string Folder { get; set; }

        [XmlProperty("Tutorial items")]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Attribute)]
        public List<TutorialItem> TutorialItems { get; set; }
    }
}
