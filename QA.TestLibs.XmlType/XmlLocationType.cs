namespace QA.TestLibs.XmlType
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    
    [Flags]
    public enum XmlLocationType
    {
        Element = 0x00,
        Attribute = 0x01,
        Value = 0x02
    }
}
