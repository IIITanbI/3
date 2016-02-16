namespace QA.TestLibs.Framework
{
    using Exceptions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using XmlDesiarilization;
    using TestLogger;

    [XmlType("TestItem config")]
    public abstract class TestItem : Source
    {
        [XmlProperty("Name of TestItem")]
        [XmlLocation("")]
        [XmlConstraint("ItemSourceType", SourceType.Xml)]
        public string Name { get; set; }

        [XmlProperty("Description for TestItem")]
        [XmlConstraint("ItemSourceType", SourceType.Xml)]
        public string Description { get; set; }

        [XmlProperty("TestingContext config", IsRequired = false)]
        [XmlConstraint("ItemSourceType", SourceType.Xml)]
        public TestContext Context { get; set; } = new TestContext();

        [XmlProperty("List of TestSteps", IsRequired = false)]
        [XmlLocation("testSteps", "testingSteps")]
        [XmlChildLocation("step", "testingStep", "testStep")]
        [XmlConstraint("ItemSourceType", SourceType.Xml)]
        public LinkedList<TestStep> Steps { get; set; } = new LinkedList<TestStep>();

        [XmlProperty("Number of tries for executing testItem", IsRequired = false)]
        [XmlLocation(XmlLocationType.Attribute | XmlLocationType.Element, "retries")]
        public int TryCount { get; set; } = 1;

        public TestItem ParentItem { get; protected set; }
        public TestLogger Log { get; set; }
        public Status TStatus { get; set; } = Status.NotExecuted;
        public ItemType TestItemType { get; set; } = ItemType.Test;

        public virtual List<TestItem> Build()
        {
            switch (ItemSourceType)
            {
                case SourceType.Xml:

                    Context.Build();
                    Log = new TestLogger(Name, TestItemType.ToString());
                    return new List<TestItem> { this };

                case SourceType.External:

                    var doc = XDocument.Load(Path);
                    var xml = doc.Element(RootElementName);
                    var testItem = XmlParser.Parse<TestItem>(xml, true);

                    return testItem.Build();

                case SourceType.Generic:
                case SourceType.ExternalGeneric:

                    throw new NotImplementedException();

                default:
                    throw new NotImplementedException();
            }
        }

        public virtual void Execute()
        {

        }

        public virtual void SetParent(TestSuite parent)
        {
            ParentItem = parent;
            Context.ParentContext = parent.Context;
            Log.SetParent(parent.Log);
            MergeSteps(parent.Steps);
        }

        public void MergeSteps(LinkedList<TestStep> parentSteps)
        {
            foreach (var step in parentSteps)
            {
                switch (step.StepOrder)
                {
                    case TestStep.Order.Pre:
                        break;

                    case TestStep.Order.Case:

                        var lastPre = Steps.LastOrDefault(cs => cs.StepOrder == TestStep.Order.Pre);
                        if (lastPre != null)
                            Steps.AddAfter(Steps.Find(lastPre), step);
                        else
                            Steps.AddFirst(step);

                        break;

                    case TestStep.Order.CasePost:

                        var firstPost = Steps.LastOrDefault(cs => cs.StepOrder == TestStep.Order.Post);
                        if (firstPost != null)
                            Steps.AddBefore(Steps.Find(firstPost), step);
                        else
                            Steps.AddLast(step);

                        break;

                    case TestStep.Order.Post:
                        break;
                    default:
                        throw new TestLibsException($"Unknown TestStepOrder: {step.StepOrder}");
                }
            }
        }

        public override void Init()
        {
            Context.Initialize();
        }

        public virtual void ExecuteStep(TestStep testStep)
        {

        }

        public enum Status
        {
            NotExecuted, Unknown, Passed, Failed, Skipped
        }

        public enum ItemType
        {
            Project, Suite, Test
        }
    }
}
