using QA.TestLibs.XmlDesiarilization;

namespace QA.TestLibs.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using QA.TestLibs.TestMetadata;

    public class ReportGenerator : IReportGenerator
    {
        public XElement CreateReport(TestItem testItem, TestEnvironmentInfo testEnvironmentInfo)
        {
            var html = new XElement("html",
                new XAttribute("lang", "en"),
                GetHead(),
                GetBody(testItem, testEnvironmentInfo)
            );

            return html;
        }

        public XElement GetHead()
        {
            var head = new XElement("head");

            var charset = new XElement("meta", new XAttribute("charset", "utf-8"));

            var httpEquiv = new XElement("meta",
                new XAttribute("http-equiv", "X-UA-Compatible"),
                new XAttribute("content", "IE=edge")
            );

            var name = new XElement("meta",
                new XAttribute("name", "viewport"),
                new XAttribute("content", "width=device-width, initial-scale=1")
            );

            var title = new XElement("title", "Test result");

            var css = new XElement("link",
                new XAttribute("rel", "stylesheet"),
                new XAttribute("href", "https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css")
            );

            head.Add(charset, httpEquiv, name, title, css);
            return head;
        }

        public XElement GetBody(TestItem testItem, TestEnvironmentInfo testEnvironmentInfo)
        {
            var body = new XElement("body");

            var js = new XElement("script", "", new XAttribute("src", "https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/bootstrap.min.js"));

            //var jQuery = new XElement("script", "", new XAttribute("src", "D:/Visual Studio 2015/Projects/QA/TestConsole/jquery-1.12.0.min.js"));

            //var jsCustom = new XElement("script", "", new XAttribute("src", "D:/Visual Studio 2015/Projects/QA/TestConsole/custom.js"));

            var jQuery = new XElement("script", "", new XAttribute("src", "https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"));

            var jsCustom = new XElement("script", "", new XAttribute("src", "filter js/testFilter.js"));
            var jsCustom1 = new XElement("script", "", new XAttribute("src", "filter js/logFilter.js"));
            var jsCustom2 = new XElement("script", "", new XAttribute("src", "filter js/stepFilter.js"));

            jsCustom = new XElement("script", "", new XAttribute("src", "filter js1/testFilter.js"));
            jsCustom1 = new XElement("script", "", new XAttribute("src", "filter js1/logFilter.js"));
            jsCustom2 = new XElement("script", "", new XAttribute("src", "filter js1/stepFilter.js"));
            var jsCustom3 = new XElement("script", "", new XAttribute("src", "filter js1/filter.js"));

            var container = new XElement("div", new XAttribute("class", "container"),
                GetEnvironment(testEnvironmentInfo),
                GetReport(testItem)
            );

            body.Add(container, jQuery, js, jsCustom, jsCustom1, jsCustom2, jsCustom3);

            return body;
        }

        public XElement GetEnvironment(TestEnvironmentInfo testEnvironmentInfo)
        {
            var environment = new XElement("div", new XAttribute("class", "panel panel-primary"), new XAttribute("style", "margin-top: 3%"));

            var heading = new XElement("div", "Environment", new XAttribute("class", "panel-heading"));

            var table = new XElement("table", new XAttribute("class", "table"));

            var thead = new XElement("thead",
                new XElement("tr",
                    new XElement("th", "CLR version"),
                    new XElement("th", "OS name"),
                    new XElement("th", "OS version"),
                    new XElement("th", "Platform"),
                    new XElement("th", "Machine name"),
                    new XElement("th", "User"),
                    new XElement("th", "User domain")
                )
            );

            var tbody = new XElement("tbody",
                new XElement("tr",
                    new XElement("td", testEnvironmentInfo.CLRVersion),
                    new XElement("td", testEnvironmentInfo.OSName),
                    new XElement("td", testEnvironmentInfo.Platform),
                    new XElement("td", testEnvironmentInfo.MachineName),
                    new XElement("td", testEnvironmentInfo.User),
                    new XElement("td", testEnvironmentInfo.UserDomain)
                )
            );

            table.Add(thead, tbody);

            environment.Add(heading, table);
            return environment;
        }

        public XElement GetOverall(TestItem testItem)
        {
            if (testItem.Type == TestItemType.Test)
                return GetStepTableHeader();

            var table = new XElement("table", 
                new XAttribute("class", "table"),
                new XAttribute("style", "margin-bottom: 0px;")
            );

            var thead = new XElement("thead",
                new XElement("tr",
                    new XElement("th", new XElement("button", "Total", new XAttribute("class", "btn btn-warning filter-passed filter-failed filter-skipped activated"))),
                    new XElement("th", new XElement("button", "Passed", new XAttribute("class", "btn btn-info filter-passed"))),
                    new XElement("th", new XElement("button", "Failed", new XAttribute("class", "btn btn-info filter-failed"))),
                    new XElement("th", new XElement("button", "Skipped", new XAttribute("class", "btn btn-info filter-skipped")))
                )
            );

            var tbody = new XElement("tbody",
                new XElement("tr",
                    new XElement("td", testItem.GetTotal()),
                    new XElement("td", testItem.GetWithStatus(TestItemStatus.Passed)),
                    new XElement("td", testItem.GetWithStatus(TestItemStatus.Failed)),
                    new XElement("td", testItem.GetWithStatus(TestItemStatus.Skipped))
                )
            );

            table.Add(thead, tbody);

            return table;
        }

        public string GetContainerColor(TestItemStatus testItemStatus)
        {
            switch (testItemStatus)
            {
                case TestItemStatus.NotExecuted:
                    return "panel-info";
                case TestItemStatus.Unknown:
                    return "panel-info";
                case TestItemStatus.Passed:
                    return "panel-success";
                case TestItemStatus.Failed:
                    return "panel-danger";
                case TestItemStatus.Skipped:
                    return "panel-warning";
                default:
                    return "panel-default";
            }
        }

        public string GetLogColor(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.TRACE:
                    return "primary";
                case LogLevel.DEBUG:
                    return "success";
                case LogLevel.WARN:
                    return "warning";
                case LogLevel.INFO:
                    return "info";
                case LogLevel.ERROR:
                    return "danger";
                default:
                    return "info";
            }
        }

        public XElement GetPanelExpander(TestItem testItem)
        {
            XElement btn;
            if (testItem.Childs.Count != 0)
            {
                btn = new XElement("button", new XAttribute("class", "btn btnexp btn-warning"), testItem.Childs[0].Type.ToString() + "s");
            }
            else if (testItem.Steps.Count != 0)
            {
                btn = new XElement("button", new XAttribute("class", "btn btnexp btn-warning"), "Steps");
            }
            else
            {
                btn = new XElement("p", "");
            }
            return btn;
        }

        public XElement GetLogExpander(XmlBaseType obj)
        {
            if (!(obj is TestItem || obj is Step))
                return null;

            string name = (obj as TestItem)?.Type.ToString() ?? (obj as Step)?.Name;
            XElement btn = new XElement("button", new XAttribute("class", "btn btnlog btn-info"), name + " logs");
            return btn;
        }

        public XElement GetException(LogMessage logMessage)
        {
            XElement log;
            if (logMessage.Exception != null)
            {
                log = new XElement("div", $"Exception: {logMessage.Exception}");
            }
            else
            {
                log = new XElement("p", "");
            }
            return log;
        }

        public XElement GetLogTableHeader()
        {
            //TRACE, DEBUG, WARN, INFO, ERRO
            int width = 70;

            var bdiv = new XElement("div",
                new XElement("button", "Trace", new XAttribute("class", $"btn btn-{GetLogColor(LogLevel.TRACE)} btn-xs log-filter-trace log-filter-debug log-filter-warn log-filter-info log-filter-error"),  new XAttribute("style", $"width: {width}px;")),
                new XElement("button", "Debug", new XAttribute("class", $"btn btn-{GetLogColor(LogLevel.DEBUG)} btn-xs log-filter-debug log-filter-warn log-filter-info log-filter-error"),  new XAttribute("style", $"width: {width}px;")),
                new XElement("button", "Warn",  new XAttribute("class", $"btn btn-{GetLogColor(LogLevel.WARN)}  btn-xs log-filter-warn log-filter-info log-filter-error"),   new XAttribute("style", $"width: {width}px;")),
                new XElement("button", "Info",  new XAttribute("class", $"btn btn-{GetLogColor(LogLevel.INFO)}  btn-xs log-filter-info log-filter-error"),   new XAttribute("style", $"width: {width}px;")),
                new XElement("button", "Error", new XAttribute("class", $"btn btn-{GetLogColor(LogLevel.ERROR)} btn-xs log-filter-error"),  new XAttribute("style", $"width: {width}px;")),
                new XAttribute("class", "log-filter-btns"),
                new XAttribute("style", "display:none;")
            );

            return bdiv;
        }

        public XElement GetLogs(XmlBaseType obj)
        {
            if (!(obj is TestItem || obj is Step))
                return null;

            string name = (obj as TestItem)?.Name ?? (obj as Step)?.Name;
            List<LogMessage> messages = (obj as TestItem)?.LogMessages ?? (obj as Step)?.Messages;

            var main = new XElement("div",
                new XAttribute("class", "logPanel"),
                new XAttribute("style", "display: none;")
            );

            XElement logTableContainer = new XElement("div",
                new XAttribute("class", "logHeader"),
                new XAttribute("style", "height:30px"),
                new XElement("div", "Logs:",
                    new XAttribute("style", "float:left; height:100%; ")
                ),
                new XElement("div", "",
                    new XAttribute("class", "logexp"),
                    new XAttribute("style", "height:20px; width:20px; background-size:contain; background-repeat:no-repeat; float:left")
                ),
                new XElement("div",
                    new XAttribute("style", "float:left"),
                    GetLogTableHeader()
                )
            );


            var elem = new XElement("div", 
                new XAttribute("class", "log"),
                new XAttribute("style", "clear:left")
             );

            if (messages.Count != 0)
            {
                int levelWidth = 60;
                int dateWidth = 160;
                foreach (var msg in messages)
                {
                    var tmp = new XElement("div",
                        new XElement("span", $"{msg.Level}", 
                            new XAttribute("class", $"bg-{GetLogColor(msg.Level)}"),
                            new XAttribute("style", $"display: inline-block; width: {levelWidth}px")
                        ),
                        new XElement("span", $" | {msg.DataStemp}",
                            new XAttribute("style", $"display: inline-block; width: {dateWidth}px")
                        ),
                        new XElement("span", $" | {msg.Message}"//,
                            //new XAttribute("style", $"display: inline-block; width: {dateWidth}px")
                        ),
                        GetException(msg),
                        new XElement("p")
                    );
                    elem.Add(tmp);
                }
            }
            else
            {
                elem.Add(new XElement("p", $"No logs for {name} item"));
            }

            main.Add(logTableContainer);
            main.Add(elem);
            return main;
        }


        public XElement GetStepTableHeader()
        {
            var table = new XElement("table", new XAttribute("class", "table"));
            //NotExecuted, Unknown, Passed, Failed, Skipped
            var thead = new XElement("thead",
                new XElement("tr",
                    new XElement("th", new XElement("button", "All", new XAttribute("class", $"btn btn-warning step-filter-passed step-filter-failed step-filter-skipped step-filter-unknown activated"))),
                    new XElement("th", new XElement("button", "NotExecuted", new XAttribute("class", $"btn btn-{GetLogColor(LogLevel.TRACE)} step-filter-notexecuted"))),
                    new XElement("th", new XElement("button", "Passed", new XAttribute("class", $"btn btn-{GetLogColor(LogLevel.DEBUG)} step-filter-passed"))),
                    new XElement("th", new XElement("button", "Failed", new XAttribute("class", $"btn btn-{GetLogColor(LogLevel.WARN)} step-filter-failed"))),
                    new XElement("th", new XElement("button", "Skipped", new XAttribute("class", $"btn btn-{GetLogColor(LogLevel.INFO)} step-filter-skipped"))),
                    new XElement("th", new XElement("button", "Unknown", new XAttribute("class", $"btn btn-{GetLogColor(LogLevel.ERROR)} step-filter-unknown")))
                )
            );
            table.Add(thead);
            return table;
        }
        

      
        public XElement GetReport(TestItem testItem)
        {
            XElement cont = new XElement("div",
                new XAttribute("class", "parent"),
                new XElement("div",
                    new XAttribute("class", $"panel {GetContainerColor(testItem.Status)} accordion"),
                    new XElement("div",
                        new XAttribute("class", "panel-heading"),
                        new XElement("p", $"{testItem.Type}: {testItem.Name}"),
                        new XElement("p", $"Status: {testItem.Status}",
                            new XAttribute("class", $"status{testItem.Status}")
                        ),
                        new XElement("p", $"Duration: {testItem.Duration} "),
                        GetPanelExpander(testItem),
                        GetLogExpander(testItem)
                    ),
                    new XElement("div",
                        new XAttribute("class", "panel-body"),
                        new XElement("p", $"Description: {testItem.Description}"),
                        GetOverall(testItem),
                        GetLogs(testItem)
                    )
                )
            );

            if (testItem.Childs.Count != 0)
            {
                XElement acc = new XElement("div",
                    new XAttribute("class", "child"),
                    new XAttribute("style", "display: none; margin-left: 3%;")
                );
                cont.Add(acc);
                foreach (var item in testItem.Childs)
                {
                    acc.Add(GetReport(item));
                }
            }
            else if (testItem.Steps.Count != 0)
            {
                XElement acc = new XElement("div",
                    new XAttribute("class", "child"),
                    new XAttribute("style", "display: none; margin-left: 3%;")
                );
                cont.Add(acc);
                foreach (var step in testItem.Steps)
                {
                    acc.Add(
                        new XElement("div",
                            new XAttribute("class", "parent"),
                            new XElement("div",
                                new XAttribute("class", $"panel {GetContainerColor(step.Status)} accordion"),
                                new XElement("div",
                                    new XAttribute("class", "panel-heading"),
                                    new XElement("p", $"{step.Name}"),
                                    new XElement("p", $"Status: {step.Status}",
                                        new XAttribute("class", $"status{step.Status}")
                                    ),
                                    new XElement("p", $"Duration: {step.Duration} "),
                                    GetLogExpander(step)
                                ),
                                new XElement("div",
                                    new XAttribute("class", "panel-body"),
                                    new XElement("p", $"Description: {step.Description}"),
                                    GetLogs(step)
                                )
                            )
                        )
                    );
                }
            }

            return cont;
        }
    }
}