namespace SAP.Automation.TutorialManager
{
    using QA.TestLibs.XmlDesiarilization;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [XmlType("Tutorial")]
    public class Tutorial : XmlBaseType
    {
        [XmlProperty("Tutorial folder")]
        public string Folder { get; set; }

        [XmlProperty("Tutorial item list")]
        public List<TutorialItem> TutorialItems { get; set; }
    }
}
