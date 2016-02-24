namespace QA.TestLibs.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using XmlDesiarilization;
    using TestMetadata;

    public class DataSource
    {
        public static TestItem GetSample()
        {
            var item = new TestItem()
            {
                Type = TestItemType.Project,
                Name = "Mock project",
                Description = "Temp mock project for reports",
                LogMessages = new List<LogMessage>() {
                       new LogMessage() {DataStemp = DateTime.Now, Level = LogLevel.DEBUG, Message = "message", Exception = new AccessViolationException()},
                       new LogMessage() {DataStemp = DateTime.Now, Level = LogLevel.ERROR, Message = "message", Exception = new AccessViolationException()},
                       new LogMessage() {DataStemp = DateTime.Now, Level = LogLevel.INFO, Message = "message", Exception = new AccessViolationException()},
                       new LogMessage() {DataStemp = DateTime.Now, Level = LogLevel.TRACE, Message = "message", Exception = new AccessViolationException()},
                       new LogMessage() {DataStemp = DateTime.Now, Level = LogLevel.WARN, Message = "message", Exception = new AccessViolationException()}
                },
                Status = TestItemStatus.Failed,
                Duration = new TimeSpan(00, 00, 60),

                Childs = new List<TestItem>()
                {
                    new TestItem()
                    {
                        Type = TestItemType.Suite,
                        Name = "Mock suite 1",
                        Description = "Temp mock suite 1 for project",
                        LogMessages = new List<LogMessage>()
                        {
                            new LogMessage()
                            {
                                DataStemp = DateTime.Now,
                                Level = LogLevel.INFO,
                                Exception = null,
                                Message = "All cool",
                                UniqueName = "log1"
                            },
                            new LogMessage()
                            {
                                DataStemp = DateTime.Now,
                                Level = LogLevel.TRACE,
                                Exception = null,
                                Message = "Trace",
                                UniqueName = "log1"
                            }
                        },
                        Status = TestItemStatus.Passed,
                        Duration = new TimeSpan(00, 00, 30),

                        Childs = new List<TestItem>()
                        {
                            new TestItem()
                            {
                                Type = TestItemType.Test,
                                Name = "Mock test 1",
                                Description = "Temp mock test 1 for suite 1",
                                LogMessages = new List<LogMessage>() {
                                   new LogMessage() {DataStemp = DateTime.Now, Level = LogLevel.DEBUG, Message = "message", Exception = new AccessViolationException()},
                                   new LogMessage() {DataStemp = DateTime.Now, Level = LogLevel.ERROR, Message = "message", Exception = new AccessViolationException()},
                                   new LogMessage() {DataStemp = DateTime.Now, Level = LogLevel.INFO, Message = "message", Exception = new AccessViolationException()},
                                   new LogMessage() {DataStemp = DateTime.Now, Level = LogLevel.TRACE, Message = "message", Exception = new AccessViolationException()},
                                   new LogMessage() {DataStemp = DateTime.Now, Level = LogLevel.WARN, Message = "message", Exception = new AccessViolationException()}
                                },
                                Status = TestItemStatus.Passed,
                                Duration = new TimeSpan(00, 00, 30),
                                Steps = new List<Step>()
                                {
                                    new Step()
                                    {
                                        Name = "Mock step 1",
                                        Description = "Temp mock step 1 for test 1",
                                        Duration = new TimeSpan(00, 00, 30),
                                        Messages = new List<LogMessage>() { },
                                        Status = TestItemStatus.Unknown
                                    },
                                    new Step()
                                    {
                                        Name = "Mock step 2",
                                        Description = "Temp mock step 2 for test 1",
                                        Duration = new TimeSpan(00, 00, 30),
                                        Messages = new List<LogMessage>()
                                        {
                                            new LogMessage()
                                            {
                                                DataStemp = DateTime.Now,
                                                Level = LogLevel.INFO,
                                                Exception = null,
                                                Message = "All cool",
                                                UniqueName = "log1"
                                            }
                                        },
                                        Status = TestItemStatus.Passed
                                    }
                                }
                            },
                            new TestItem()
                            {
                                Type = TestItemType.Test,
                                Name = "Mock test 2",
                                Description = "Temp mock test 2 for suite 1",
                                LogMessages = new List<LogMessage>()
                                {
                                    new LogMessage()
                                    {
                                        DataStemp = DateTime.Now,
                                        Level = LogLevel.DEBUG,
                                        Exception = null,
                                        Message = "Debug message",
                                        UniqueName = "log1"
                                    }
                                },
                                Status = TestItemStatus.Skipped,
                                Duration = new TimeSpan(00, 00, 30),
                                Steps = new List<Step>()
                                {
                                    new Step()
                                    {
                                        Name = "Mock step 1",
                                        Description = "Temp mock step 1 for test 2",
                                        Duration = new TimeSpan(00, 00, 30),
                                        Messages = new List<LogMessage>() { },
                                        Status = TestItemStatus.Passed
                                    },
                                    new Step()
                                    {
                                        Name = "Mock step 2",
                                        Description = "Temp mock step 2 for test 2",
                                        Duration = new TimeSpan(00, 00, 30),
                                        Messages = new List<LogMessage>() { },
                                        Status = TestItemStatus.Passed
                                    }
                                }
                            },
                        }
                    },
                    new TestItem()
                    {
                        Type = TestItemType.Suite,
                        Name = "Mock suite 2",
                        Description = "Temp mock suite 2 for project",
                        LogMessages = new List<LogMessage>() { },
                        Status = TestItemStatus.Failed,
                        Duration = new TimeSpan(00, 00, 30),

                        Childs = new List<TestItem>()
                        {
                            new TestItem()
                            {
                                Type = TestItemType.Test,
                                Name = "Mock test 1",
                                Description = "Temp mock test 1 for suite 2",
                                LogMessages = new List<LogMessage>()
                                {
                                    new LogMessage()
                                    {
                                        DataStemp = DateTime.Now,
                                        Level = LogLevel.WARN,
                                        Exception = null,
                                        Message = "Warning",
                                        UniqueName = "log2"
                                    }
                                },
                                Status = TestItemStatus.Passed,
                                Duration = new TimeSpan(00, 00, 30),
                                Steps = new List<Step>()
                                {
                                    new Step()
                                    {
                                        Name = "Mock step 1",
                                        Description = "Temp mock step 1 for test 1",
                                        Duration = new TimeSpan(00, 00, 30),
                                        Messages = new List<LogMessage>() { },
                                        Status = TestItemStatus.Passed
                                    },
                                    new Step()
                                    {
                                        Name = "Mock step 2",
                                        Description = "Temp mock step 2 for test 1",
                                        Duration = new TimeSpan(00, 00, 30),
                                        Messages = new List<LogMessage>() { },
                                        Status = TestItemStatus.Passed
                                    }
                                }
                            },
                            new TestItem()
                            {
                                Type = TestItemType.Test,
                                Name = "Mock test 2",
                                Description = "Temp mock test 2 for suite 2",
                                LogMessages = new List<LogMessage>()
                                {
                                    new LogMessage()
                                    {
                                        DataStemp = DateTime.Now,
                                        Level = LogLevel.ERROR,
                                        Exception = null,
                                        Message = "Catch error",
                                        UniqueName = "log1"
                                    }
                                },
                                Status = TestItemStatus.Failed,
                                Duration = new TimeSpan(00, 00, 30),
                                Steps = new List<Step>()
                                {
                                    new Step()
                                    {
                                        Name = "Mock step 1",
                                        Description = "Temp mock step 1 for test 1",
                                        Duration = new TimeSpan(00, 00, 30),
                                        Messages = new List<LogMessage>() { },
                                        Status = TestItemStatus.Failed
                                    },
                                    new Step()
                                    {
                                        Name = "Mock step 2",
                                        Description = "Temp mock step 2 for test 1",
                                        Duration = new TimeSpan(00, 00, 30),
                                        Messages = new List<LogMessage>() { },
                                        Status = TestItemStatus.Passed
                                    }
                                }
                            },
                        }
                    }
                }
            };
            return item;
        }
    }
}
