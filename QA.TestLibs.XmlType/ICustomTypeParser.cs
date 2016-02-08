namespace QA.TestLibs.XmlType
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    public interface ICustomTypeParser
    {
        bool IsMatch(Type type);
        object Parse(Type type, XObject config, bool isAssignableTypeAllowed, XmlLocation childLocation, IContext context);
    }
}
