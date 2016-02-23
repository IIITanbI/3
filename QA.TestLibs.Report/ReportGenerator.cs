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

            var jsCustom = new XElement("script", "", new XAttribute("src", "custom.js"));
            var jsCustom1 = new XElement("script", "", new XAttribute("src", "logFilter.js"));

            var container = new XElement("div", new XAttribute("class", "container"),
                GetEnvironment(testEnvironmentInfo),
                GetReport(testItem)
            );

            body.Add(container, jQuery, js, jsCustom1, jsCustom);

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
            if (testItem.Type == TestItemType.Test) return null;

            var table = new XElement("table", new XAttribute("class", "table"));

            var thead = new XElement("thead",
                new XElement("tr",
                    new XElement("th", new XElement("button", "Total", new XAttribute("class", "btn btn-warning filter-total activated"))),
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
            else
            {
                btn = new XElement("p", "");
            }
            return btn;
        }

        public XElement GetLogExpander(TestItem testItem)
        {
            XElement btn = new XElement("button", new XAttribute("class", "btn btnlog btn-info"), testItem.Type + " logs");
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

        public XElement GetLogs(TestItem testItem)
        {
            XElement logTableContainer = new XElement("div", new XAttribute("style", "display:table"),
                new XElement("div", "Logs:", new XAttribute("style", "display:table-cell")),
                GetLogTableHeader()
            );

            var main = new XElement("div",
                new XAttribute("class", "logPanel"),
                new XAttribute("style", "display: none;")
            );

            var elem = new XElement("div", new XAttribute("class", "log"));

            if (testItem.LogMessages.Count != 0)
            {
                foreach (var msg in testItem.LogMessages)
                {
                    var tmp = new XElement("div",
                        new XElement("span", $"{msg.Level}",
                            new XAttribute("class", $"bg-{GetLogColor(msg.Level)}")
                        ),
                        $" | {msg.DataStemp} | {msg.Message}",
                        GetException(msg),
                        new XElement("p")
                    );
                    elem.Add(tmp);
                }
            }
            else
            {
                elem.Add(new XElement("p", $"No logs for {testItem.Name} item"));
            }

            main.Add(logTableContainer);
            main.Add(elem);
            return main;
        }

        public XElement GetLogTableHeader()
        {
            var table = new XElement("table", new XAttribute("class", "table"));
            //TRACE, DEBUG, WARN, INFO, ERROR
            var thead = new XElement("thead",
                new XElement("tr",
                    new XElement("th", new XElement("button", "All", new XAttribute("class", $"btn btn-warning log-filter-total activated"))),
                    new XElement("th", new XElement("button", "Trace", new XAttribute("class", $"btn btn-{GetLogColor(LogLevel.TRACE)} log-filter-trace"))),
                    new XElement("th", new XElement("button", "Debug", new XAttribute("class", $"btn btn-{GetLogColor(LogLevel.DEBUG)} log-filter-debug"))),
                    new XElement("th", new XElement("button", "Warn", new XAttribute("class", $"btn btn-{GetLogColor(LogLevel.WARN)} log-filter-warn"))),
                    new XElement("th", new XElement("button", "Info", new XAttribute("class", $"btn btn-{GetLogColor(LogLevel.INFO)} log-filter-info"))),
                    new XElement("th", new XElement("button", "Error", new XAttribute("class", $"btn btn-{GetLogColor(LogLevel.ERROR)} log-filter-error")))
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
            return cont;
        }
    }
}