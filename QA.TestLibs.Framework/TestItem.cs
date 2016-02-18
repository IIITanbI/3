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
        [XmlConstraint("ItemSourceType", SourceType.Xml)]
        public int RetryCount { get; set; } = 1;

        public TestItem ParentItem { get; protected set; }
        public TestLogger Log { get; set; }
        public ItemStatus Status { get; set; } = ItemStatus.NotExecuted;
        public ItemType TestItemType { get; set; } = ItemType.Test;

        protected int _tryNumber = 1;

        public List<TestItem> FailedTries { get; set; } = new List<TestItem>();
        protected List<StepMeta> StepsMeta { get; set; } = new List<StepMeta>();

        public virtual List<TestItem> Build()
        {
            switch (ItemSourceType)
            {
                case SourceType.Xml:

                    Log = new TestLogger(Name, TestItemType.ToString());
                    foreach (var step in Steps)
                    {
                        step.Build();
                    }
                    Context.Build();
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

        public void Execute()
        {
            Status = ItemStatus.Unknown;
            StepsMeta.Clear();
            var realLog = Log;
            Log = new TestLogger(Name, TestItemType.ToString());

            Log.INFO($"Start executing {TestItemType}: {Name}");
            Log.INFO($"Description: {Description}");
            Log.DEBUG("Start of initialization");
            try
            {
                ExecuteStageInitialization();
            }
            catch (Exception ex)
            {
                Log.ERROR("Error has occurred during initialization", ex);
                Log.SpamToLog(realLog);
                Log = realLog;
                MarkAsFailedOrSkipped();
            }
            Log.DEBUG("Initialization has been completed");


            if (RetryCount != 1)
            {
                realLog.DEBUG($"Start try {_tryNumber} of {RetryCount}");
            }

            try
            {
                ExecuteStagePre();
                ExecuteStageCase();
            }
            catch (Exception ex)
            {
                if (RetryCount == 1)
                {
                    Log.ERROR("Error occurred during step execution", ex);
                    Log.SpamToLog(realLog);
                    Log = realLog;
                    MarkAsFailedOrSkipped();
                }
                else
                {
                    Log.WARN("Error occurred during step execution", ex);
                }
            }
            finally
            {
                try
                {
                    ExecuteStagePost();

                    if (Status != ItemStatus.Failed)
                    {
                        Log.SpamToLog(realLog);
                        Log = realLog;
                    }
                }
                catch (Exception ex)
                {
                    if (RetryCount == 1)
                    {
                        if (Status != ItemStatus.Failed)
                        {
                            Log.ERROR("Error occurred during step execution", ex);
                            Log.SpamToLog(realLog);
                            Log = realLog;
                            MarkAsFailedOrSkipped();
                        }
                        else
                        {
                            Log.WARN("Error occurred during step execution", ex);
                        }
                    }
                }
                finally
                {
                    if (Status == ItemStatus.Failed)
                    {
                        if (_tryNumber < RetryCount)
                        {
                            realLog.WARN($"Try {_tryNumber} of {RetryCount} completed with error. Try again");

                            FailedTries.Add(GetState());

                            Log = realLog;
                            _tryNumber++;
                            Execute();
                        }
                    }
                }
            }
        }

        public virtual TestItem GetState()
        {
            TestItem testItem = (TestItem)Activator.CreateInstance(GetType());

            testItem.Name = Name;
            testItem.Description = Description;
            testItem.Context.ContextValues = Context.ContextValues;
            testItem.Context.CommandManagersItems = Context.CommandManagersItems;
            testItem.TestItemType = TestItemType;
            testItem.Log = Log;
            testItem.StepsMeta = StepsMeta;

            return testItem;
        }

        public virtual void ExecuteStageInitialization()
        {
            Log.TRACE("Start of Context initialization");
            Context.Initialize();
            Log.DEBUG("Context has been initialized");
        }

        public virtual void ExecuteStagePre()
        {
            var stepsToExecute = Steps.Where(step => step.StepOrder == TestStep.Order.Pre).ToList();
            Log.TRACE($"Found: {stepsToExecute.Count} Pre steps");
            if (stepsToExecute.Count > 0)
            {
                Log.TRACE("Start executing Pre steps");

                foreach (var step in stepsToExecute)
                {
                    ExecuteStep(step);
                }

                Log.TRACE("Pre steps have been executed");
            }
        }
        public virtual void ExecuteStageCase()
        {
            var stepsToExecute = Steps.Where(step => step.StepOrder == TestStep.Order.Case || step.StepOrder == TestStep.Order.CasePost).ToList();
            Log.TRACE($"Found: {stepsToExecute.Count} Case steps");
            if (stepsToExecute.Count > 0)
            {
                Log.TRACE("Start executing Case steps");

                foreach (var step in stepsToExecute)
                {
                    ExecuteStep(step);
                }

                Log.TRACE("Case steps have been executed");
            }
        }
        public virtual void ExecuteStagePost()
        {
            var stepsToExecute = Steps.Where(step => step.StepOrder == TestStep.Order.Post).ToList();
            Log.TRACE($"Found: {stepsToExecute.Count} Post steps");
            if (stepsToExecute.Count > 0)
            {
                Log.TRACE("Start executing Post steps");

                foreach (var step in stepsToExecute)
                {
                    ExecuteStep(step);
                }

                Log.TRACE("Post steps have been executed");
            }
        }
        public virtual void ExecuteStep(TestStep testStep)
        {
            Log.INFO($"Start executing TestStep: {testStep.Name}");
            Log.INFO($"Description: {testStep.Description}");

            var stepMeta = new StepMeta();
            stepMeta.Step = testStep;
            stepMeta.ItemStatus = ItemStatus.Unknown;
            stepMeta.Log = new TestLogger(testStep.Name, "Step");
            stepMeta.Log.AddParent(Log);
            StepsMeta.Add(stepMeta);

            for (int i = 1; i <= testStep.TryCount; i++)
            {
                try
                {
                    testStep.Execute(Context, stepMeta.Log);
                    stepMeta.ItemStatus = ItemStatus.Passed;
                }
                catch (Exception ex)
                {
                    if (i == testStep.TryCount)
                    {
                        if (i > 1)
                            stepMeta.Log.WARN($"Try: {i} of {testStep.TryCount} completed with error");

                        if (testStep.IsSkippedOnFail)
                        {
                            stepMeta.Log.WARN($"Step completed with error but skipped", ex);
                            stepMeta.ItemStatus = ItemStatus.Skipped;
                        }
                        else
                        {
                            stepMeta.ItemStatus = ItemStatus.Failed;
                            throw ex;
                        }
                    }
                    else
                    {
                        stepMeta.Log.WARN($"Try: {i} of {testStep.TryCount} completed with error", ex);
                        stepMeta.Log.WARN("Try again");
                    }
                }
            }
        }

        public virtual void MarkAsFailedOrSkipped(ItemStatus status = ItemStatus.Failed)
        {
            if (Status == ItemStatus.NotExecuted)
                Status = status;
            if (Status == ItemStatus.Unknown)
                Status = ItemStatus.Failed;
        }



        public virtual void SetParent(TestSuite parent)
        {
            if (parent != null)
            {
                ParentItem = parent;
                Context.ParentContext = parent.Context;
                MergeSteps(parent.Steps);
                Log.AddParent(parent.Log);
            }
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

        public TestMetadata.TestItem GetReportItem()
        {
            var reportItem = new TestMetadata.TestItem();
            reportItem.Name = Name;
            reportItem.Description = Description;
            reportItem.Status = Status.ToString();

            return reportItem;
        }

        public enum ItemStatus
        {
            NotExecuted, Unknown, Passed, Failed, Skipped
        }

        public enum ItemType
        {
            Project, Suite, Test
        }

        protected class StepMeta
        {
            public TestStep Step { get; set; }
            public ItemStatus ItemStatus { get; set; }
            public TestLogger Log { get; set; }
        }
    }
}
