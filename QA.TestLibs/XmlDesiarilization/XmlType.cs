namespace QA.TestLibs.XmlDesiarilization
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Reflection;
    using System.Xml.Linq;
    using Exceptions;
    using CustomParsers;

    public class XmlType
    {
        public Type XType { get; private set; }

        public List<XmlProperty> XmlProperties { get; private set; } = new List<XmlProperty>();
        public string Description { get; private set; }
        public XmlLocation Location { get; private set; }
        public PropertyInfo KeyProperty { get; protected set; }

        public XmlType(Type type)
        {
            XType = type;
            Init();
        }

        private void Init()
        {
            var xmlTypeAtt = XType.GetCustomAttribute<XmlTypeAttribute>();

            Description = xmlTypeAtt.Description;

            KeyProperty = XType.GetProperty(xmlTypeAtt.KeyPropertyName);
            if (KeyProperty == null)
                throw new TestLibsException($"Couldn't find KeyProperty with name: {xmlTypeAtt.KeyPropertyName} for type: {XType}");

            Location = new XmlLocation(XType);
            var propertyInfos = XType.GetProperties();

            foreach (var propertyInfo in propertyInfos)
            {
                var xmlPropertyAtt = propertyInfo.GetCustomAttribute<XmlPropertyAttribute>(true);
                if (xmlPropertyAtt != null)
                {
                    var xmlProperty = new XmlProperty(this, propertyInfo);
                    XmlProperties.Add(xmlProperty);
                }
            }
        }

        public override string ToString()
        {
            return $"XmlType for type: {XType.Name}\nDescription: {Description}";
        }
    }
}
