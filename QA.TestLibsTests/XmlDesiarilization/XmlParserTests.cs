namespace QA.TestLibs.XmlDesiarilization.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using XmlDesiarilization;

    [TestClass()]
    public class XmlParserTests
    {
        public static XElement SimpleConfig = XDocument.Load("TestData.xml").Element("test").Element("simple");
        public static XElement ComplexConfig = XDocument.Load("TestData.xml").Element("test").Element("complex");

        public XmlParserTests()
        {
            ReflectionManager.LoadAssemblies();
        }

        [TestMethod()]
        [Description("Parse object with all base types. All values are in element nodes")]
        public void ParseSimpleAllElements()
        {
            var config = SimpleConfig.Element("simpleClass1_1");
            var obj = XmlParser.Parse<DataSamples.SimpleClass1>(config);

            Assert.AreEqual(100, obj.IntValue);
            Assert.AreEqual(100.1, obj.DoubleValue);
            Assert.AreEqual(true, obj.BoolValue);
            Assert.AreEqual(DataSamples.SimpleEnum.Value2, obj.EnumValue);
            Assert.AreEqual("Key1", obj.Key);
            Assert.AreEqual(DateTime.Parse("1/18/2016 1:49:30 PM"), obj.DateTimeValue);
            Assert.AreEqual(null, obj.UniqueName);
        }

        [TestMethod()]
        [Description("Parse object with all base types. Most values are in attribute nodes, one left in element node and one in element in cdata")]
        public void ParseSimpleAllAttributes()
        {
            var config = SimpleConfig.Element("simpleClass1_2");
            var obj = XmlParser.Parse<DataSamples.SimpleClass1>(config);

            Assert.AreEqual(100, obj.IntValue);
            Assert.AreEqual(100.1, obj.DoubleValue);
            Assert.AreEqual(true, obj.BoolValue);
            Assert.AreEqual(DataSamples.SimpleEnum.Value2, obj.EnumValue);
            Assert.AreEqual("Key1", obj.Key);
            Assert.AreEqual(DateTime.Parse("1/18/2016 1:49:30 PM"), obj.DateTimeValue);
            Assert.AreEqual(null, obj.UniqueName);
        }

        [TestMethod()]
        [Description("Parse object with constraint. Constraint link property doesn't present. Dependent property doesn't present")]
        public void ParseSimpleWithConstraintCase1()
        {
            var config = SimpleConfig.Element("simpleClass2_1");
            var obj = XmlParser.Parse<DataSamples.SimpleClass2>(config);

            Assert.AreEqual(100, obj.IntValue);
            Assert.AreEqual(DataSamples.SimpleEnum.Value1, obj.EnumVal);
            Assert.AreEqual(null, obj.Slave);
        }

        [TestMethod()]
        [Description("Parse object with constraint. Constraint link property has value not in constraint. Dependent property doesn't present")]
        public void ParseSimpleWithConstraintCase2()
        {
            var config = SimpleConfig.Element("simpleClass2_2");
            var obj = XmlParser.Parse<DataSamples.SimpleClass2>(config);

            Assert.AreEqual(100, obj.IntValue);
            Assert.AreEqual(DataSamples.SimpleEnum.Value3, obj.EnumVal);
            Assert.AreEqual(null, obj.Slave);
        }

        [TestMethod()]
        [Description("Parse object with constraint. Constraint link property has value not in constraint. Dependent property present")]
        public void ParseSimpleWithConstraintCase3()
        {
            var config = SimpleConfig.Element("simpleClass2_3");
            var obj = XmlParser.Parse<DataSamples.SimpleClass2>(config);

            Assert.AreEqual(100, obj.IntValue);
            Assert.AreEqual(DataSamples.SimpleEnum.Value3, obj.EnumVal);
            Assert.AreEqual(null, obj.Slave);
        }

        [TestMethod()]
        [Description("Parse object with constraint. Constraint link property has value in constraint. Dependent property present")]
        public void ParseSimpleWithConstraintCase4()
        {
            var config = SimpleConfig.Element("simpleClass2_4");
            var obj = XmlParser.Parse<DataSamples.SimpleClass2>(config);

            Assert.AreEqual(100, obj.IntValue);
            Assert.AreEqual(DataSamples.SimpleEnum.Value2, obj.EnumVal);
            Assert.AreEqual("slaveVal", obj.Slave);
        }

        [TestMethod()]
        [Description("Parse object with constraint. Constraint link property in attribute node and has value in constraint. Dependent property present")]
        public void ParseSimpleWithConstraintCase5()
        {
            var config = SimpleConfig.Element("simpleClass2_5");
            var obj = XmlParser.Parse<DataSamples.SimpleClass2>(config);

            Assert.AreEqual(100, obj.IntValue);
            Assert.AreEqual(DataSamples.SimpleEnum.Value2, obj.EnumVal);
            Assert.AreEqual("slaveVal", obj.Slave);
        }

        [TestMethod()]
        [Description("Parse object with property that could be a value of root node. Just value property as value")]
        public void ParseSimpleWithValueCase1()
        {
            var config = SimpleConfig.Element("simpleClass3_1");
            var obj = XmlParser.Parse<DataSamples.SimpleClass3>(config);

            Assert.AreEqual(100, obj.IntValue);
            Assert.AreEqual(null, obj.StringValue);
        }

        [TestMethod()]
        [Description("Parse object with property that could be a value of root node. Just value property in element node")]
        public void ParseSimpleWithValueCase2()
        {
            var config = SimpleConfig.Element("simpleClass3_2");
            var obj = XmlParser.Parse<DataSamples.SimpleClass3>(config);

            Assert.AreEqual(100, obj.IntValue);
            Assert.AreEqual(null, obj.StringValue);
        }

        [TestMethod()]
        [Description("Parse object with property that could be a value of root node. Just value property in attribute node")]
        public void ParseSimpleWithValueCase3()
        {
            var config = SimpleConfig.Element("simpleClass3_3");
            var obj = XmlParser.Parse<DataSamples.SimpleClass3>(config);

            Assert.AreEqual(100, obj.IntValue);
            Assert.AreEqual(null, obj.StringValue);
        }

        [TestMethod()]
        [Description("Parse object with property that could be a value of root node. Value property in attribute node and simple property in element node")]
        public void ParseSimpleWithValueCase4()
        {
            var config = SimpleConfig.Element("simpleClass3_4");
            var obj = XmlParser.Parse<DataSamples.SimpleClass3>(config);

            Assert.AreEqual(100, obj.IntValue);
            Assert.AreEqual("stringVal", obj.StringValue);
        }

        [TestMethod()]
        [Description("Parse object with property that could be a value of root node. Value property in as value and simple property in attribute node")]
        public void ParseSimpleWithValueCase5()
        {
            var config = SimpleConfig.Element("simpleClass3_5");
            var obj = XmlParser.Parse<DataSamples.SimpleClass3>(config);

            Assert.AreEqual(100, obj.IntValue);
            Assert.AreEqual("stringVal", obj.StringValue);
        }

        [TestMethod()]
        [Description("Parse object with QA type field with location attribute and QA type field without QA location attribute")]
        public void ParseComplexCase1()
        {
            var config = ComplexConfig.Element("complexClass1_1");
            var obj = XmlParser.Parse<DataSamples.ComplexClass1>(config);

            Assert.IsNotNull(obj.SomeSimpleClass1);
            Assert.AreEqual(100, obj.SomeSimpleClass1.IntValue);
            Assert.AreEqual(100.1, obj.SomeSimpleClass1.DoubleValue);
            Assert.AreEqual(true, obj.SomeSimpleClass1.BoolValue);
            Assert.AreEqual(DataSamples.SimpleEnum.Value2, obj.SomeSimpleClass1.EnumValue);
            Assert.AreEqual("Key1", obj.SomeSimpleClass1.Key);
            Assert.AreEqual(DateTime.Parse("1/18/2016 1:49:30 PM"), obj.SomeSimpleClass1.DateTimeValue);
            Assert.AreEqual(null, obj.SomeSimpleClass1.UniqueName);

            Assert.IsNotNull(obj.SomeSimpleClass2);
            Assert.AreEqual(100, obj.SomeSimpleClass2.IntValue);
            Assert.AreEqual(DataSamples.SimpleEnum.Value2, obj.SomeSimpleClass2.EnumVal);
            Assert.AreEqual("slaveVal", obj.SomeSimpleClass2.Slave);
        }


        [TestMethod()]
        [Description("Parse object with List non QA type field without location attribute and List non QA type field with location attribute and the same with List of QA types")]
        public void ParseComplexCase2()
        {
            var config = ComplexConfig.Element("complexClass2_1");
            var obj = XmlParser.Parse<DataSamples.ComplexClass2>(config);

            Assert.IsNotNull(obj.IntList1);
            Assert.AreEqual(3, obj.IntList1.Count);
            Assert.AreEqual(1, obj.IntList1[0]);
            Assert.AreEqual(2, obj.IntList1[1]);
            Assert.AreEqual(3, obj.IntList1[2]);

            Assert.IsNotNull(obj.IntList2);
            Assert.AreEqual(3, obj.IntList2.Count);
            Assert.AreEqual(1, obj.IntList2[0]);
            Assert.AreEqual(2, obj.IntList2[1]);
            Assert.AreEqual(3, obj.IntList2[2]);

            Assert.IsNotNull(obj.ListOfSimpleClass1es1);
            Assert.AreEqual(2, obj.ListOfSimpleClass1es1.Count);
            Assert.AreEqual(100, obj.ListOfSimpleClass1es1[0].IntValue);
            Assert.AreEqual(100.1, obj.ListOfSimpleClass1es1[0].DoubleValue);
            Assert.AreEqual(true, obj.ListOfSimpleClass1es1[0].BoolValue);
            Assert.AreEqual(DataSamples.SimpleEnum.Value2, obj.ListOfSimpleClass1es1[0].EnumValue);
            Assert.AreEqual("Key1", obj.ListOfSimpleClass1es1[0].Key);
            Assert.AreEqual(DateTime.Parse("1/18/2016 1:49:30 PM"), obj.ListOfSimpleClass1es1[0].DateTimeValue);
            Assert.AreEqual(null, obj.ListOfSimpleClass1es1[0].UniqueName);
            Assert.AreEqual(100, obj.ListOfSimpleClass1es1[1].IntValue);
            Assert.AreEqual(100.1, obj.ListOfSimpleClass1es1[1].DoubleValue);
            Assert.AreEqual(true, obj.ListOfSimpleClass1es1[1].BoolValue);
            Assert.AreEqual(DataSamples.SimpleEnum.Value2, obj.ListOfSimpleClass1es1[1].EnumValue);
            Assert.AreEqual("Key1", obj.ListOfSimpleClass1es1[1].Key);
            Assert.AreEqual(DateTime.Parse("1/18/2016 1:49:30 PM"), obj.ListOfSimpleClass1es1[1].DateTimeValue);
            Assert.AreEqual(null, obj.ListOfSimpleClass1es1[1].UniqueName);

            Assert.IsNotNull(obj.ListOfSimpleClass1es2);
            Assert.AreEqual(2, obj.ListOfSimpleClass1es2.Count);
            Assert.AreEqual(100, obj.ListOfSimpleClass1es2[0].IntValue);
            Assert.AreEqual(100.1, obj.ListOfSimpleClass1es2[0].DoubleValue);
            Assert.AreEqual(true, obj.ListOfSimpleClass1es2[0].BoolValue);
            Assert.AreEqual(DataSamples.SimpleEnum.Value2, obj.ListOfSimpleClass1es2[0].EnumValue);
            Assert.AreEqual("Key1", obj.ListOfSimpleClass1es2[0].Key);
            Assert.AreEqual(DateTime.Parse("1/18/2016 1:49:30 PM"), obj.ListOfSimpleClass1es2[0].DateTimeValue);
            Assert.AreEqual(null, obj.ListOfSimpleClass1es2[0].UniqueName);
            Assert.AreEqual(100, obj.ListOfSimpleClass1es2[1].IntValue);
            Assert.AreEqual(100.1, obj.ListOfSimpleClass1es2[1].DoubleValue);
            Assert.AreEqual(true, obj.ListOfSimpleClass1es2[1].BoolValue);
            Assert.AreEqual(DataSamples.SimpleEnum.Value2, obj.ListOfSimpleClass1es2[1].EnumValue);
            Assert.AreEqual("Key1", obj.ListOfSimpleClass1es2[1].Key);
            Assert.AreEqual(DateTime.Parse("1/18/2016 1:49:30 PM"), obj.ListOfSimpleClass1es2[1].DateTimeValue);
            Assert.AreEqual(null, obj.ListOfSimpleClass1es2[1].UniqueName);
        }

        [TestMethod()]
        [Description("Parse object with QA type with allowed inheretence")]
        public void ParseComplexCase3()
        {
            var config = ComplexConfig.Element("complexClass3_1");
            var obj = XmlParser.Parse<DataSamples.ComplexClass3>(config);

            Assert.IsNotNull(obj.SomeSimple);
            Assert.IsInstanceOfType(obj.SomeSimple, typeof(DataSamples.SimpleClass2));
            Assert.AreEqual(100, ((DataSamples.SimpleClass2)obj.SomeSimple).IntValue);

            Assert.IsNotNull(obj.ListOfSomeSimples);
            Assert.AreEqual(2, obj.ListOfSomeSimples.Count);
            Assert.IsInstanceOfType(obj.ListOfSomeSimples[1], typeof(DataSamples.SimpleClass2));
            Assert.AreEqual(100, ((DataSamples.SimpleClass2)obj.ListOfSomeSimples[1]).IntValue);
            Assert.IsInstanceOfType(obj.ListOfSomeSimples[0], typeof(DataSamples.SimpleClass1));
            Assert.AreEqual(100.100, ((DataSamples.SimpleClass1)obj.ListOfSomeSimples[0]).DoubleValue);
        }
    }
}