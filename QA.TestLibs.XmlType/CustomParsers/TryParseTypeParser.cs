namespace QA.TestLibs.XmlType.CustomParsers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using ExtensionMethods;
    using Exceptions;

    public class TryParseTypeParser : ICustomTypeParser
    {
        public bool IsMatch(Type type)
        {
            return type.IsPrimitive || type.Name == "DateTime";
        }

        public object Parse(Type type, XObject config, bool isAssignableTypeAllowed, XmlLocation childLocation, IContext context)
        {
            var value = config.Value();

            var m = type.GetMethod("TryParse", new Type[] { typeof(string), type.MakeByRefType() });
            object[] args = { value, null };
            var r = (bool)m.Invoke(null, args);

            if (!r)
                throw new TypeCastException(value, type, config);

            return args[1];
        }
    }
}
