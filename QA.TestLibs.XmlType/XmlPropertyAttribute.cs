namespace QA.TestLibs.XmlType
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class XmlPropertyAttribute : XmlDescriptionAttribute
    {
        public bool IsAssignableTypesAllowed { get; set; } = false;
        public bool IsRequired { get; set; } = true;

        public XmlPropertyAttribute(string description)
            :base(description)
        {

        }
    }
}
