namespace QA.TestLibs.XmlType.CustomParsers
{
    using Exceptions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    public class GenericTypeParser : ICustomTypeParser
    {
        public bool IsMatch(Type type)
        {
            return type.IsGenericType;
        }

        public object Parse(Type type, XObject config, bool isAssignableTypeAllowed, XmlLocation childLocation, IContext context)
        {
            var configEl = config as XElement;
            if (configEl == null)
                throw new TestLibsException($"Following configuration element:\n{config}\n\tcouldn't be cast to type: {type}. XElement expected");
            
            var genArgs = type.GetGenericArguments();

            if (genArgs.Length > 2)
                throw new TestLibsException($"Unsupported type: {type.ToString()}. Couldn't parse following config to that type:\n{configEl}");

            var childObjType = genArgs.Last();
            var childObjs = XmlParser.Parse(childObjType, configEl, childLocation, false, context);

            var propertyObj = Activator.CreateInstance(type);

            if (type.GetGenericTypeDefinition() == typeof(List<>))
            {
                var add = type.GetMethod("Add", new[] { childObjType });

                foreach (var child in childObjs)
                {
                    add.Invoke(propertyObj, new object[] { child });
                }
            }
            else if (type.GetGenericTypeDefinition() == typeof(LinkedList<>))
            {
                var add = type.GetMethod("AddLast", new[] { childObjType });

                foreach (var child in childObjs)
                {
                    add.Invoke(propertyObj, new object[] { child });
                }
            }
            else if (type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                var childXmlType = ReflectionManager.GetXmlType(childObjType);
                if (childXmlType == null)
                    throw new TestLibsException($"Couldn't parse type: {type} to dictionary, because child type: {childObjType} isn't XmlType");

                var add = type.GetMethod("Add", new[] { childXmlType.KeyProperty.PropertyType, childObjType });

                foreach (var child in childObjs)
                {
                    var key = childXmlType.KeyProperty.GetValue(child);
                    add.Invoke(propertyObj, new object[] { key, child });
                }
            }
            else
            {
                throw new TestLibsException($"Unsupported generic type: {type}. Couldn't parse following config to that type:\n{configEl}");
            }

            return propertyObj;
        }
    }
}
