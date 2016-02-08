namespace QA.TestLibs.XmlType
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ExtensionMethods;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class XmlConstraintAttribute : Attribute
    {
        public string PropertyName { get; private set; }
        public List<object> RequiredValues { get; private set; }
        public bool IsPositive { get; set; } = true;

        public XmlConstraintAttribute(string propertyName, params object[] allowedValues)
        {
            PropertyName = propertyName;
            RequiredValues = allowedValues.ToList();
        }

        public override string ToString()
        {
            return $"Constraint. Require property with name: {PropertyName}. Required values:\n{RequiredValues.ToStringWithList()}";
        }
    }
}
