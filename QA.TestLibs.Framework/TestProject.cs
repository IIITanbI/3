namespace QA.TestLibs.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using XmlDesiarilization;


    [XmlType("TestProject config")]
    [XmlLocation("project")]
    public class TestProject : TestSuite
    {
        public TestProject()
        {
            TestItemType = ItemType.Project;
        }

        public override List<TestItem> Build()
        {
            var project = base.Build();
            project.First().SetParent(null);
            return project;
        }

        public override void SetParent(TestSuite parent)
        {
            
        }
    }
}
