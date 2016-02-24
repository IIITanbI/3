namespace SAP.Automation.TutorialManager
{
    using QA.TestLibs.XmlDesiarilization;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [XmlType("Tutorial item")]
    public class TutorialItem : XmlBaseType
    {
        [XmlProperty("Tutorial item folder name")]
        public string FolderName { get; set; }

        [XmlProperty("Tutorial item files")]
        public List<TutorialFile> TutorialFiles { get; set; }
    }
}
