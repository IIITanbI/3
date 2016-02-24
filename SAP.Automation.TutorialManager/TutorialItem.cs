namespace SAP.Automation.TutorialManager
{
    using QA.TestLibs.XmlDesiarilization;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [Serializable]
    [XmlType("Tutorial item configuration")]
    public class TutorialItem : XmlBaseType
    {
        [XmlProperty("Tutorial item folder name")]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Attribute)]
        public string FolderName { get; set; }

        [XmlProperty("Tutorial item files")]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Attribute)]
        public List<TutorialFile> TutorialFiles { get; set; }
    }
}
