namespace QA.TestLibs.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using XmlDesiarilization;

    [XmlType("ManagerConfig")]
    [XmlLocation("manager", "commandManager", "managerItem")]
    public class CommandManagerItem : XmlBaseType
    {
        [XmlProperty("Manager type name")]
        [XmlLocation(XmlLocationType.Attribute, "type")]
        public string ManagerType { get; set; }

        [XmlProperty("Command manager name")]
        public string Name { get; set; }

        [XmlProperty("Manager config")]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Value)]
        public XElement Config { get; set; }
    }
}
