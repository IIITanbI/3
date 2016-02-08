namespace QA.TestLibs.XmlType
{
    using CustomParsers;
    using Exceptions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    public static class XmlParser
    {
        public static List<ICustomTypeParser> Parsers { get; set; } = new List<ICustomTypeParser>
            {
                new StringTypeParser(),
                new EnumTypeParser(),
                new TryParseTypeParser(),
                new GenericTypeParser(),
                new ArrayTypeParser(),
                new DefaultQaTypeParser()
            };

        public static object Parse(Type type, XObject config, bool isAssignableTypesAllowed = false, XmlLocation childLocation = null, IContext context = null)
        {
            if (config == null)
                throw new TestLibsException($"Config for type: {type} is null.");

            var parser = Parsers.FirstOrDefault(p => p.IsMatch(type));
            if (parser == null)
                throw new TestLibsException($"Couldn't find parser for type: {type}");

            return parser.Parse(type, config, isAssignableTypesAllowed, childLocation, context);
        }

        public static T Parse<T>(XElement config, bool isAssignableTypesAllowed = false, XmlLocation childLocation = null, IContext context = null)
        {
            return (T)Parse(typeof(T), config, isAssignableTypesAllowed, childLocation, context);
        }

        public static List<object> Parse(Type type, XElement config, XmlLocation childLocation, bool isAssignableTypesAllowed, IContext context)
        {
            var list = new List<object>();
            var els = childLocation.GetChildElements(config);

            foreach (var el in els)
            {
                var obj = Parse(type, el, isAssignableTypesAllowed, null, context);
                list.Add(obj);
            }

            return list;
        }
    }
}
