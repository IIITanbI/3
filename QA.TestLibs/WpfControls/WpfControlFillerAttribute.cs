namespace QA.TestLibs.WpfControls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class WpfControlFillerAttribute : Attribute
    {
        public Type XmlType { get; private set; }

        public WpfControlFillerAttribute(Type xmlType)
        {
            XmlType = xmlType;
        }
    }
}
