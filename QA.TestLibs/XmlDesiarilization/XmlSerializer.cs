namespace QA.TestLibs.XmlDesiarilization
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using System.Reflection;
    using ExtensionMethods;

    public static class XmlSerializer
    {
        public static XElement Serialize(object obj)
        {
            var type = obj.GetType();
            var tmpObj = Activator.CreateInstance(type);

            var xmlType = ReflectionManager.GetXmlType(type);

            var xElement = new XElement(xmlType.XType.Name);

            foreach (var xmlProperty in xmlType.XmlProperties)
            {
                var propVal = xmlProperty.GetValue(obj);
                if (xmlProperty.PropertyType.IsGenericType || xmlProperty.PropertyType.IsArray)
                {
                    var xcel = new XElement(xmlProperty.Info.Name);
                    var children = propVal.GetChilds();
                    children.ForEach(c => xcel.Add(Serialize(c)));
                    xElement.Add(xcel);
                }
                else if (typeof(XmlBaseType).IsAssignableFrom(xmlProperty.PropertyType))
                {
                    var xel = Serialize(propVal);
                    xElement.Add(xel);
                }
                else
                {
                    if (xmlProperty.IsRequired && (propVal != xmlProperty.GetValue(tmpObj)))
                    {
                        var tmp = propVal ?? "null";
                        xElement.Add(new XElement(xmlProperty.Info.Name, new XCData(tmp.ToString())));
                    }
                }
            }
            return xElement;
        }
    }
}
