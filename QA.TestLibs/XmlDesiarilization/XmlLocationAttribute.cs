namespace QA.TestLibs.XmlDesiarilization
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class XmlLocationAttribute : Attribute
    {
        public XmlLocationType LocationType { get; private set; } = XmlLocationType.Element;
        public List<string> AllowedNames { get; private set; }

        public XmlLocationAttribute(XmlLocationType locationType, params string[] allowedNames)
        {
            LocationType = locationType;
            AllowedNames = allowedNames.ToList();
        }

        public XmlLocationAttribute(params string[] allowedNames)
        {
            AllowedNames = allowedNames.ToList();
        }
    }
}
