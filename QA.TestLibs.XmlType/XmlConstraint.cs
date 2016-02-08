namespace QA.TestLibs.XmlType
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class XmlConstraint
    {
        public List<object> RequiredValues { get; set; }
        public XmlProperty Property { get; set; }
        public bool IsPosisitive { get; set; } = true;

        public bool Verify(object obj)
        {
            if(IsPosisitive)
                return RequiredValues.Contains(Property.GetValue(obj));

            return !RequiredValues.Contains(Property.GetValue(obj));
        }
    }
}
