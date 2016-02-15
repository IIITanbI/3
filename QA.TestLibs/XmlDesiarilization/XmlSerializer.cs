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
                if (xmlProperty.PropertyType.IsGenericType)
                {
                    var xcel = new XElement(xmlProperty.Info.Name);
                    if (xmlProperty.PropertyType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                    {
                        foreach (var pVal in ((IDictionary)propVal).Values)
                        {
                            var xel = Serialize(pVal);
                            xcel.Add(xel);
                        }
                    }
                    else
                    {
                        foreach (var pVal in (IEnumerable)propVal)
                        {
                            var xel = Serialize(pVal);
                            xcel.Add(xel);
                        }
                    }
                    xElement.Add(xcel);
                }
                else if (typeof(XmlBaseType).IsAssignableFrom(xmlProperty.PropertyType))
                {
                    var xel = Serialize(propVal);
                    xElement.Add(xel);
                }
                else
                {
                    bool tempIsRequired = false;
                    int constCount = xmlProperty.Constraints.Value.Count;

                    if (constCount != 0)
                    {
                        tempIsRequired = (bool) xmlProperty.Constraints.Value[0].Property.GetValue(tmpObj);
                    }

                    if (xmlProperty.IsRequired && (propVal != xmlProperty.GetValue(tmpObj)))
                    {
                        if (constCount != 0)
                        {
                            if (tempIsRequired)
                            {
                                var tmp = propVal ?? "null";
                                xElement.Add(new XElement(xmlProperty.Info.Name, System.Security.SecurityElement.Escape(tmp.ToString())));
                            }
                        }
                        else
                        {
                            var tmp = propVal ?? "null";
                            xElement.Add(new XElement(xmlProperty.Info.Name, System.Security.SecurityElement.Escape(tmp.ToString())));
                        }
                    }
                }
            }
            return xElement;
        }
    }
}
