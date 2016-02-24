namespace SAP.Automation.TutorialManager
{
    using QA.TestLibs.XmlDesiarilization;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [XmlType("Tutorial config")]
    public class TutorialConfig : XmlBaseType
    {
        [XmlProperty("Tutorial temp folder")]
        public string Folder { get; set; }
    }
}
