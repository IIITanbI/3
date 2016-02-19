namespace QA.TestLibs.XmlDesiarilization
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    [Serializable]
    [XmlType("Base type for all QA types")]
    public class XmlBaseType
    {
        [XmlProperty("Unique name for object", IsRequired = false)]
        [XmlLocation(XmlLocationType.Attribute | XmlLocationType.Element, "unique")]
        public string UniqueName { get; set; } = null;

        public virtual void Init()
        {

        }
    }
}
