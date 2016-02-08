namespace QA.TestLibs.XmlType
{
    using Exceptions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    public class XmlProperty
    {
        public XmlType ParentType { get; private set; }
        public PropertyInfo Info { get; private set; }
        public Type PropertyType { get; private set; }

        public bool IsRequired { get; private set; }
        public bool IsAssignableTypesAllowed { get; private set; }
        public bool IsPropertyTypeXml { get; private set; }

        public string Description { get; private set; }

        public Lazy<XmlLocation> Location { get; private set; }
        public Lazy<XmlLocation> ChildLocation { get; private set; } = null;
        public Lazy<List<XmlConstraint>> Constraints { get; private set; } = null;

        public XmlProperty(XmlType parentType, PropertyInfo propertyInfo)
        {
            Info = propertyInfo;
            PropertyType = propertyInfo.PropertyType;
            ParentType = parentType;

            Init();
        }

        private void Init()
        {
            var xmlPropertyAtt = Info.GetCustomAttribute<XmlPropertyAttribute>(true);

            IsRequired = xmlPropertyAtt.IsRequired;
            IsAssignableTypesAllowed = xmlPropertyAtt.IsAssignableTypesAllowed;
            IsPropertyTypeXml = PropertyType.GetCustomAttribute<XmlTypeAttribute>(true) == null;
            Description = xmlPropertyAtt.Description;

            Location = new Lazy<XmlLocation>(() => new XmlLocation(this));

            if (PropertyType.IsGenericType || PropertyType.IsArray)
                ChildLocation = new Lazy<XmlLocation>(() => new XmlLocation(this, true));

            Constraints = new Lazy<List<XmlConstraint>>(() =>
                {
                    var constraintList = new List<XmlConstraint>();
                    var constraintAtts = Info.GetCustomAttributes<XmlConstraintAttribute>(true).ToList();

                    foreach (var constraintAtt in constraintAtts)
                    {
                        var constraintProperty = ParentType.XmlProperties.FirstOrDefault(p => p.Info.Name == constraintAtt.PropertyName);
                        if (constraintProperty == null)
                            throw new TestLibsException($"Couldn't make constraint. Type {ParentType} doesn't contains property: {constraintAtt.PropertyName}");

                        var constraint = new XmlConstraint { Property = constraintProperty, RequiredValues = constraintAtt.RequiredValues, IsPosisitive = constraintAtt.IsPositive };
                        constraintList.Add(constraint);
                    }

                    return constraintList;
                }
            );
        }

        public override string ToString()
        {
            return $"{ParentType.XType.FullName} .{Info.Name}";
        }

        public object GetValue(object obj)
        {
            return Info.GetValue(obj);
        }

        public void SetValue(object obj, object value)
        {
            Info.SetValue(obj, value);
        }

        public override bool Equals(object obj)
        {
            var r = obj as XmlProperty;
            if (r == null) return false;

            return Info.Name == r.Info.Name;
        }

        public override int GetHashCode()
        {
            return Info.Name.GetHashCode();
        }
    }
}
