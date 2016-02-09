namespace QA.TestLibs.XmlDesiarilization
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public abstract class XmlDescriptionAttribute : Attribute
    {
        public string Description { get; set; }
        public XmlDescriptionAttribute(string description)
        {
            Description = description;
        }
    }
}
