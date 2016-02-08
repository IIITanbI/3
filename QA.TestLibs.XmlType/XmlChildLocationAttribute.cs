namespace QA.TestLibs.XmlType
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class XmlChildLocationAttribute : XmlLocationAttribute
    {
        public XmlChildLocationAttribute(params string[] allowedNames)
            : base(XmlLocationType.Element, allowedNames)
        {

        }
    }
}
