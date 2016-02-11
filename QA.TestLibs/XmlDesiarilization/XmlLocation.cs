namespace QA.TestLibs.XmlDesiarilization
{
    using Exceptions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using ExtensionMethods;
    using System.Xml.Linq;
    using System.Xml;
    public class XmlLocation
    {
        public List<string> AttributeAllowedNames { get; private set; } = new List<string>();
        public List<string> ElementAllowedNames { get; private set; } = new List<string>();
        public bool CouldBeValue { get; private set; } = false;

        public XmlLocation(XmlProperty propertyInfo, bool isChildLocation = false)
        {
            if (!isChildLocation)
            {
                var locationAtts = propertyInfo.Info.GetCustomAttributes<XmlLocationAttribute>(true).ToList();
                locationAtts.Add(new XmlLocationAttribute(XmlLocationType.Element | XmlLocationType.Attribute, propertyInfo.Info.Name));
                locationAtts.ForEach(AddLocation);

                if (propertyInfo.IsAssignableTypesAllowed)
                {
                    var assignableTypes = ReflectionManager.GetAssignableTypes(propertyInfo.PropertyType);
                    assignableTypes.ForEach(AddLocation);
                }
                else
                    AddLocation(propertyInfo.PropertyType);
            }
            else
            {
                var childLocationAtts = propertyInfo.Info.GetCustomAttributes<XmlChildLocationAttribute>(true).ToList();
                childLocationAtts.ForEach(AddLocation);

                var childType = propertyInfo.PropertyType.IsGenericType
                        ? propertyInfo.PropertyType.GetGenericArguments().Last()
                        : (propertyInfo.PropertyType.IsArray
                            ? propertyInfo.PropertyType.GetElementType()
                            : propertyInfo.PropertyType);

                if (childType == null)
                    throw new TestLibsException($"Couldn't find child type for type: {propertyInfo.PropertyType}, for property: {propertyInfo}");

                if (propertyInfo.IsAssignableTypesAllowed)
                {
                    var assignableTypes = ReflectionManager.GetAssignableTypes(childType);
                    assignableTypes.ForEach(AddLocation);
                }
                else
                    AddLocation(childType);
            }
        }
        public XmlLocation(Type type)
        {
            AddLocation(type);
        }

        public void AddLocation(Type type)
        {
            var locationAtts = type.GetCustomAttributes<XmlLocationAttribute>(true).ToList();

            if (!type.IsGenericType && !type.IsArray)
                locationAtts.Add(new XmlLocationAttribute(XmlLocationType.Element, type.Name));

            locationAtts.ForEach(AddLocation);
        }

        public void AddLocation(XmlType xmlType)
        {
            AddLocation(xmlType.Location.ElementAllowedNames);
        }

        public void AddLocation(XmlLocationAttribute locationAttribute)
        {
            if (locationAttribute.LocationType.HasFlag(XmlLocationType.Element))
            {
                ElementAllowedNames.AddRange(locationAttribute.AllowedNames);
                ElementAllowedNames.AddCaseVariantsForFirstChar();
                ElementAllowedNames.Sort();
            }

            if (locationAttribute.LocationType.HasFlag(XmlLocationType.Attribute))
            {
                AttributeAllowedNames.AddRange(locationAttribute.AllowedNames);
                AttributeAllowedNames.AddCaseVariantsForFirstChar();
                AttributeAllowedNames.Sort();
            }

            if (locationAttribute.LocationType.HasFlag(XmlLocationType.Value))
            {
                CouldBeValue = true;
            }
        }

        public void AddLocation(List<string> allowedElementNames)
        {
            ElementAllowedNames.AddRange(allowedElementNames);
            ElementAllowedNames.AddCaseVariantsForFirstChar();
            ElementAllowedNames.Sort();
        }

        public bool Check(XmlLocationType locationType, string name)
        {
            if (locationType.HasFlag(XmlLocationType.Element))
            {
                if (ElementAllowedNames.Contains(name))
                    return true;
            }
            if (locationType.HasFlag(XmlLocationType.Attribute))
            {
                if (AttributeAllowedNames.Contains(name))
                    return true;
            }

            return false;
        }

        public XObject GetElement(XElement config)
        {
            XObject result = null;

            result = config.AttributeByNames(AttributeAllowedNames);
            if (result != null) return result;

            result = config.ElementByNames(ElementAllowedNames);
            if (result != null) return result;

            if (CouldBeValue)
                return config;

            return null;
        }

        public List<XElement> GetChildElements(XElement config)
        {
            return config.ElementsByNames(ElementAllowedNames, true);
        }

        //public override int GetHashCode()
        //{
        //    return _key.GetHashCode();
        //}

        //public override bool Equals(object obj)
        //{
        //    var r = obj as ConfigComplexLocation;
        //    if (r == null) return false;

        //    return string.Compare(_key, r._key) == 0;
        //}

        public override string ToString()
        {
            return $"Property xml location:\n{ElementAllowedNames.ToStringWithList("\t- Element: ")}{AttributeAllowedNames.ToStringWithList("\t- Attribute: ")}\t- Could be value: {CouldBeValue}";
        }
    }
}
