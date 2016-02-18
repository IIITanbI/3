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
                Type = "Project",
                Name = "Mock project",
                Description = "Temp mock project for reports",
                LogMessages = new List<LogMessage>() { },
                Status = "Fail",
                Duration = new TimeSpan(00, 00, 60),

                Childs = new List<TestItem>()
                {
                    new TestItem()
                    {
                        Type = "Suite",
                        Name = "Mock suite 1",
                        Description = "Temp mock suite 1 for project",
                        LogMessages = new List<LogMessage>() { },
                        Status = "Passed",
                        Duration = new TimeSpan(00, 00, 30),

                        Childs = new List<TestItem>()
                        {
                            new TestItem()
                            {
                                Type = "Test",
                                Name = "Mock test 1",
                                Description = "Temp mock test 1 for suite 1",
                                LogMessages = new List<LogMessage>() { },
                                Status = "Passed",
                                Duration = new TimeSpan(00, 00, 30),
                                Steps = new List<Step>()
                                {
                                    new Step()
                                    {
                                        Name = "Mock step 1",
                                        Description = "Temp mock step 1 for test 1",
                                        Duration = new TimeSpan(00, 00, 30),
                                        Messages = new List<LogMessage>() { },
                                        Status = "Passed"
                                    },
                                    new Step()
                                    {
                                        Name = "Mock step 2",
                                        Description = "Temp mock step 2 for test 1",
                                        Duration = new TimeSpan(00, 00, 30),
                                        Messages = new List<LogMessage>() { },
                                        Status = "Passed"
                                    }
                                }
                            },
                            new TestItem()
                            {
                                Type = "Test",
                                Name = "Mock test 2",
                                Description = "Temp mock test 2 for suite 1",
                                LogMessages = new List<LogMessage>() { },
                                Status = "Passed",
                                Duration = new TimeSpan(00, 00, 30),
                                Steps = new List<Step>()
                                {
                                    new Step()
                                    {
                                        Name = "Mock step 1",
                                        Description = "Temp mock step 1 for test 2",
                                        Duration = new TimeSpan(00, 00, 30),
                                        Messages = new List<LogMessage>() { },
                                        Status = "Passed"
                                    },
                                    new Step()
                                    {
                                        Name = "Mock step 2",
                                        Description = "Temp mock step 2 for test 2",
                                        Duration = new TimeSpan(00, 00, 30),
                                        Messages = new List<LogMessage>() { },
                                        Status = "Passed"
                                    }
                                }
                            },
                        }
                    },
                    new TestItem()
                    {
                        Type = "Suite",
                        Name = "Mock suite 2",
                        Description = "Temp mock suite 2 for project",
                        LogMessages = new List<LogMessage>() { },
                        Status = "Fail",
                        Duration = new TimeSpan(00, 00, 30),

                        Childs = new List<TestItem>()
                        {
                            new TestItem()
                            {
                                Type = "Test",
                                Name = "Mock test 1",
                                Description = "Temp mock test 1 for suite 2",
                                LogMessages = new List<LogMessage>() { },
                                Status = "Passed",
                                Duration = new TimeSpan(00, 00, 30),
                                Steps = new List<Step>()
                                {
                                    new Step()
                                    {
                                        Name = "Mock step 1",
                                        Description = "Temp mock step 1 for test 1",
                                        Duration = new TimeSpan(00, 00, 30),
                                        Messages = new List<LogMessage>() { },
                                        Status = "Passed"
                                    },
                                    new Step()
                                    {
                                        Name = "Mock step 2",
                                        Description = "Temp mock step 2 for test 1",
                                        Duration = new TimeSpan(00, 00, 30),
                                        Messages = new List<LogMessage>() { },
                                        Status = "Failed"
                                    }
                                }
                            },
                            new TestItem()
                            {
                                Type = "Test",
                                Name = "Mock test 2",
                                Description = "Temp mock test 2 for suite 2",
                                LogMessages = new List<LogMessage>() { },
                                Status = "Fail",
                                Duration = new TimeSpan(00, 00, 30),
                                Steps = new List<Step>()
                                {
                                    new Step()
                                    {
                                        Name = "Mock step 1",
                                        Description = "Temp mock step 1 for test 1",
                                        Duration = new TimeSpan(00, 00, 30),
                                        Messages = new List<LogMessage>() { },
                                        Status = "Failed"
                                    },
                                    new Step()
                                    {
                                        Name = "Mock step 2",
                                        Description = "Temp mock step 2 for test 1",
                                        Duration = new TimeSpan(00, 00, 30),
                                        Messages = new List<LogMessage>() { },
                                        Status = "Passed"
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
