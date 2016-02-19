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
            ItemType = TestItemType.Project;
        }

        public override List<TestItem> Build()
        {
            var projects = base.Build();
            var project = projects.First();
            project.SetParent(null);
            return projects;
        }
    }
}
