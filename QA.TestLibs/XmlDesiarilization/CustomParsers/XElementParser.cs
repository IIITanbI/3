namespace QA.TestLibs.XmlDesiarilization.CustomParsers
{
    using Exceptions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    public class XElementParser : ICustomTypeParser
    {
        public bool IsMatch(Type type)
        {
            return type.IsAssignableFrom(typeof(XElement));
        }

        public object Parse(Type type, XObject config, bool isAssignableTypeAllowed, XmlLocation childLocation, IContext context)
        {
            var xel = config as XElement;
            if (xel == null)
                throw new TestLibsException($"Couldn't cast following config to XElement:\n{config}");
            return xel;
        }
    }
}
