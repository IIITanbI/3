namespace QA.TestLibs.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using XmlDesiarilization;

    [XmlType("Test suite config")]
    [XmlLocation("Suite")]
    public class TestSuite : TestItem
    {
        [XmlProperty("Child test items", IsAssignableTypesAllowed = true)]
        [XmlLocation("suites", "tests", "testSuites", "cases", "testCases")]
        [XmlConstraint("ItemSourceType", SourceType.Xml)]
        public List<TestItem> TestItems { get; set; } = new List<TestItem>();

        [XmlProperty("Is parallel execution allowed for children", IsRequired = false)]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Attribute, "parallel", "isParallel", "parallelAllowed")]
        [XmlConstraint("ItemSourceType", SourceType.Xml)]
        public bool IsParallelExecutionAllowed { get; set; } = false;

        [XmlProperty("Level of parallelism", IsRequired = false)]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Attribute, "levelOfparallelism")]
        [XmlConstraint("ItemSourceType", SourceType.Xml)]
        [XmlConstraint("IsParallelExecutionAllowed", true)]
        public int ParallelismLevel { get; set; } = -1;

        public TestSuite()
        {
            TestItemType = ItemType.Suite;
        }

        public override List<TestItem> Build()
        {
            var builtSuites = base.Build();

            foreach (var builtSuite in builtSuites)
            {
                var builtChildren = new List<TestItem>();

                foreach (var testItem in ((TestSuite)builtSuite).TestItems)
                {
                    builtChildren.AddRange(testItem.Build());
                }

                ((TestSuite)builtSuite).TestItems.Clear();
                ((TestSuite)builtSuite).TestItems = builtChildren;
            }

            return builtSuites;
        }

        public override void SetParent(TestSuite parent)
        {
            base.SetParent(parent);

            foreach (var testItem in TestItems)
            {
                testItem.SetParent(this);
            }
        }

        public override TestItem GetState()
        {
            var testSuite = (TestSuite)base.GetState();
            foreach (var child in TestItems)
            {
                testSuite.TestItems.Add(child.GetState());
            }
            return testSuite;
        }

        public override void ExecuteStageCase()
        {
            base.ExecuteStageCase();

            if (!IsParallelExecutionAllowed)
            {
                foreach (var testItem in TestItems)
                {
                    testItem.Execute();
                }
            }
            else
            {
                if (ParallelismLevel != -1 && ParallelismLevel > 1)
                    Parallel.ForEach(TestItems, new ParallelOptions { MaxDegreeOfParallelism = ParallelismLevel }, ti => ti.Execute());
                else
                    Parallel.ForEach(TestItems, ti => ti.Execute());
            }

            if (TestItems.Any(ti => ti.Status == ItemStatus.Failed))
                Status = ItemStatus.Failed;
        }

        public override void MarkAsFailedOrSkipped(ItemStatus status = ItemStatus.Failed)
        {
            base.MarkAsFailedOrSkipped(status);

            foreach (var testItem in TestItems)
            {
                testItem.MarkAsFailedOrSkipped(ItemStatus.Skipped);
            }
        }
    }
}
