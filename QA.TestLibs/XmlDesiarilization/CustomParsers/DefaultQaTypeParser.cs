namespace QA.TestLibs.XmlDesiarilization.CustomParsers
{
    using Exceptions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using ExtensionMethods;

    public class DefaultQaTypeParser : ICustomTypeParser
    {
        private static Lazy<XmlType> _xmlType = new Lazy<XmlType>(() => ReflectionManager.GetXmlType(typeof(XmlBaseType)));
        private static Lazy<XmlProperty> _uniqProperty = new Lazy<XmlProperty>(
                () => _xmlType.Value.XmlProperties.First(p => p.Info.Name == "UniqueName")
            );

        public bool IsMatch(Type type)
        {
            if (typeof(XmlBaseType).IsAssignableFrom(type))
                return true;
            return false;
        }

        public object Parse(Type type, XObject config, bool isAssignableTypeAllowed, XmlLocation childLocation, IContext context)
        {
            var configElement = config as XElement;
            if (configElement == null)
                throw new TestLibsException($"Couldn't parse type: {type} from following config (element node expected):\n{config}");

            var xmlType = _xmlType.Value;

            string uniqueName = null;
            object createdObject = null;

            if (context != null)
            {
                createdObject = new XmlBaseType();
                
                var xmlNode = _uniqProperty.Value.Location.Value.GetElement(configElement);
                if(xmlNode != null)
                    uniqueName = XmlParser.Parse(_uniqProperty.Value.PropertyType, xmlNode) as string;

                if (uniqueName != null)
                {
                    if (context.Contains(type, uniqueName))
                        return context.ResolveValue(type, uniqueName);
                }
            }

            if (isAssignableTypeAllowed)
            {
                var possibleTypeName = configElement.Name.ToString();
                var assignableXmlTypes = ReflectionManager.GetAssignableTypes(type);

                foreach (var assignableXmlType in assignableXmlTypes)
                {
                    if (assignableXmlType.Location.Check(XmlLocationType.Element, possibleTypeName))
                    {
                        xmlType = assignableXmlType;
                    }
                }

                if (xmlType == _xmlType.Value)
                    throw new TestLibsException($"Couldn't find any type that could be parsed from element with name: {possibleTypeName}\nPossible element names: {assignableXmlTypes.Select(xt => $"Type: {xt.XType}, description: {xt.Description}\n{xt.Location}").ToList().ToStringWithList("=")}");
            }
            else
            {
                xmlType = ReflectionManager.GetXmlType(type);
            }

            var parsedPropertiesDict = new Dictionary<XmlProperty, bool>();
            xmlType.XmlProperties.ForEach(p => parsedPropertiesDict.Add(p, false));

            createdObject = Activator.CreateInstance(xmlType.XType);
            if (uniqueName != null)
            {
                _uniqProperty.Value.SetValue(createdObject, uniqueName);
                parsedPropertiesDict[_uniqProperty.Value] = true;
            }


            var propertyToParse = parsedPropertiesDict.Keys.FirstOrDefault(k => !parsedPropertiesDict[k]);
            while (propertyToParse != null)
            {
                ParseProperty(propertyToParse, configElement, createdObject, parsedPropertiesDict, context);
                propertyToParse = parsedPropertiesDict.Keys.FirstOrDefault(k => !parsedPropertiesDict[k]);
            }

            return createdObject;
        }

        public static void ParseProperty(XmlProperty propertyToParse, XElement configElement, object createdObject, Dictionary<XmlProperty, bool> parsedPropertiesDict, IContext context)
        {
            var constarintProperties = propertyToParse.Constraints.Value;

            foreach (var constraintProperty in constarintProperties)
            {
                if (!parsedPropertiesDict[constraintProperty.Property])
                    ParseProperty(constraintProperty.Property, configElement, createdObject, parsedPropertiesDict, context);
            }

            if (!constarintProperties.All(c => c.Verify(createdObject)))
            {
                parsedPropertiesDict[propertyToParse] = true;
                return;
            }

            var xmlNode = propertyToParse.Location.Value.GetElement(configElement);
            if (xmlNode == null)
            {
                if (propertyToParse.IsRequired)
                    throw new TestLibsException($"There is no required element to parse. Property name: {propertyToParse.Info.Name}\nDescription: {propertyToParse.Description}\n{propertyToParse.Location}");

                parsedPropertiesDict[propertyToParse] = true;
                return;
            }

            var propertyValue = XmlParser.Parse(propertyToParse.PropertyType, xmlNode, propertyToParse.IsAssignableTypesAllowed, propertyToParse.ChildLocation?.Value, context);

            propertyToParse.SetValue(createdObject, propertyValue);
            parsedPropertiesDict[propertyToParse] = true;
        }
    }
}
