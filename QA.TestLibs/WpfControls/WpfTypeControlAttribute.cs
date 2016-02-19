namespace QA.TestLibs.WpfControls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class WpfTypeControlAttribute : Attribute
    {
        public string XmlTypeName { get; private set; }

        public WpfTypeControlAttribute(string xmlTypeName)
        {
            XmlTypeName = xmlTypeName;
        }
    }
}
