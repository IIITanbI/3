namespace QA.TestLibs.XmlType
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class XmlTypeAttribute : XmlDescriptionAttribute
    {
        public string KeyPropertyName { get; private set; }

        public XmlTypeAttribute(string description, string keyPropertyName = "UniqueName")
            : base(description)
        {
            KeyPropertyName = keyPropertyName;
        }
    }
}
