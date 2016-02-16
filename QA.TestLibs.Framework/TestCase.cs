namespace QA.TestLibs.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using XmlDesiarilization;

    [XmlType("TestProject config")]
    [XmlLocation("test")]
    public class TestCase : TestItem
    {
        public TestCase()
        {
            TestItemType = ItemType.Test;
        }
    }
}
