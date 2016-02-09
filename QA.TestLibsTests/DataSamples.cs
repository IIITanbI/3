namespace QA.TestLibs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using XmlDesiarilization;

    public class DataSamples
    {
        public enum SimpleEnum
        {
            Value1, Value2, Value3
        }

        [XmlType("Some base class for SimleClasses")]
        [XmlLocation("sClass")]
        public class SimpleClass : XmlBaseType
        {

        }

        [XmlType("Simple class1", "Key")]
        [XmlLocation("sClass1")]
        public class SimpleClass1 : SimpleClass
        {
            [XmlProperty("Key for SimpleClass")]
            [XmlLocation(XmlLocationType.Attribute | XmlLocationType.Element, "key")]
            public string Key { get; set; }

            [XmlProperty("Some Int value")]
            [XmlLocation(XmlLocationType.Attribute | XmlLocationType.Element, "int", "intValue")]
            public int IntValue { get; set; }

            [XmlProperty("Some Double value")]
            [XmlLocation(XmlLocationType.Attribute | XmlLocationType.Element, "double", "doubleValue")]
            public double DoubleValue { get; set; }

            [XmlProperty("Some Enum value")]
            [XmlLocation(XmlLocationType.Attribute | XmlLocationType.Element, "enum", "enumValue")]
            public SimpleEnum EnumValue { get; set; }

            [XmlProperty("Some DateTime value")]
            [XmlLocation(XmlLocationType.Attribute | XmlLocationType.Element, "datetime", "dateTime", "dateTimeValue", "datetimeValue")]
            public DateTime DateTimeValue { get; set; }

            [XmlProperty("Some Bool value")]
            [XmlLocation(XmlLocationType.Attribute | XmlLocationType.Element, "bool", "boolValue")]
            public bool BoolValue { get; set; }
        }

        [XmlType("Simple class2")]
        [XmlLocation("sClass2")]
        public class SimpleClass2 : SimpleClass
        {
            [XmlProperty("Some value depends from enum")]
            [XmlLocation("slave")]
            [XmlConstraint("EnumVal", SimpleEnum.Value2)]
            public string Slave { get; set; } = null;

            [XmlProperty("Some enum value", IsRequired = false)]
            [XmlLocation(XmlLocationType.Attribute | XmlLocationType.Element, "enumVal", "enum")]
            public SimpleEnum EnumVal { get; set; } = SimpleEnum.Value1;

            [XmlProperty("Some Int value")]
            [XmlLocation(XmlLocationType.Attribute | XmlLocationType.Element, "int", "intValue")]
            public int IntValue { get; set; }
        }

        [XmlType("Simple class3")]
        [XmlLocation("sClass3")]
        public class SimpleClass3 : SimpleClass
        {
            [XmlProperty("Some Int value, could be Value")]
            [XmlLocation(XmlLocationType.Attribute | XmlLocationType.Element | XmlLocationType.Value, "int", "intValue")]
            public int IntValue { get; set; }

            [XmlProperty("Some not required String value", IsRequired = false)]
            [XmlLocation(XmlLocationType.Attribute | XmlLocationType.Element, "string", "stringValue")]
            public string StringValue { get; set; } = null;
        }

        [XmlType("Complex class 1")]
        [XmlLocation("cClass1")]
        public class ComplexClass1 : XmlBaseType
        {
            [XmlProperty("Some SimpleClass1")]
            [XmlLocation("someNamedSimpleClass1")]
            public SimpleClass1 SomeSimpleClass1 { get; set; }

            [XmlProperty("Some SimpleClass1")]
            public SimpleClass2 SomeSimpleClass2 { get; set; }
        }

        [XmlType("Complex class 2")]
        [XmlLocation("cClass2")]
        public class ComplexClass2 : XmlBaseType
        {
            [XmlProperty("Some IntList")]
            [XmlLocation("intList1")]
            public List<int> IntList1 { get; set; }

            [XmlProperty("Some IntList")]
            [XmlLocation("intList2")]
            [XmlChildLocation("number")]
            public List<int> IntList2 { get; set; }

            [XmlProperty("List of SimpleClass1es")]
            public List<SimpleClass1> ListOfSimpleClass1es1 { get; set; }

            [XmlProperty("List of SimpleClasse1s")]
            [XmlChildLocation("namedSimpleClass1")]
            public List<SimpleClass1> ListOfSimpleClass1es2 { get; set; }
        }

        [XmlType("Complex class 3")]
        [XmlLocation("cClass3")]
        public class ComplexClass3 : XmlBaseType
        {
            [XmlProperty("Some class implemented ISimple", IsAssignableTypesAllowed = true)]
            public SimpleClass SomeSimple { get; set; }

            [XmlProperty("List of some class implemented ISimple", IsAssignableTypesAllowed = true)]
            public List<SimpleClass> ListOfSomeSimples { get; set; }
        }
    }
}
