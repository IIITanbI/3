namespace SAP.Automation.AemPageManager
{
    using QA.TestLibs.XmlDesiarilization;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [XmlType("AemPage")]
    public class AemPage : XmlBaseType
    {
        [XmlProperty("Aem page title")]
        public string Title { get; set; } = null;

        [XmlProperty("Parent aem page path")]
        public string ParentPath { get; set; } = null;

        [XmlProperty("Aem page template")]
        public string Template { get; set; } = null;
    }
}
