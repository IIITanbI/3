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
        public Type XmlType { get; private set; }

        public WpfTypeControlAttribute(Type xmlType)
        {
            XmlType = xmlType;
        }
    }
}
