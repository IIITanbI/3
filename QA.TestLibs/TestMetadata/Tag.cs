namespace QA.TestLibs.TestMetadata
{
    using QA.TestLibs.XmlDesiarilization;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [XmlType("Tag config")]
    public class Tag : XmlBaseType
    {
        [XmlProperty("Tag name")]
        public string Name { get; set; }
    }
}
