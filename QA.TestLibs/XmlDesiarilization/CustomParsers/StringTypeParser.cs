namespace QA.TestLibs.XmlDesiarilization.CustomParsers
{
    using ExtensionMethods;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    public class StringTypeParser : ICustomTypeParser
    {
        public bool IsMatch(Type type)
        {
            return type.Name == "String";
        }

        public object Parse(Type type, XObject config, bool isAssignableTypeAllowed, XmlLocation childLocation, IContext context)
        {
            return config.Value();
        }
    }
}
