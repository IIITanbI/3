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

            var jQuery = new XElement("script", "", new XAttribute("src", "https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"));

            var jsCustom = new XElement("script",
                "$(function(){ $('.parent').find('.accordion').click(function(e){ $(this).parent().children('.child').toggle(); }); });"
            );

            var container = new XElement("div", new XAttribute("class", "container"),
                GetEnvironment(testEnvironmentInfo),
                GetReport(testItem)
            );

            body.Add(container, jQuery, js, jsCustom);

            return body;
        }

        public XElement GetEnvironment(TestEnvironmentInfo testEnvironmentInfo)
        {
            var environment = new XElement("div", new XAttribute("class", "panel panel-default"), new XAttribute("style", "margin-top: 3%"));

            var heading = new XElement("div", new XElement("mark", "Environment"), new XAttribute("class", "panel-heading"));

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
                    new XElement("th", "Total"),
                    new XElement("th", "Passed"),
                    new XElement("th", "Failed"),
                    new XElement("th", "Skipped")
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

        public XElement GetReport(TestItem testItem)
        {
            XElement cont = (new XElement("div",
                new XAttribute("class", "parent"),
                new XElement("div",
                    new XAttribute("class", "panel panel-default accordion"),
                    new XElement("div",
                        new XAttribute("class", "panel-heading"),
                        new XElement("p", $"{testItem.Type}: {testItem.Name}"),
                        new XElement("p", $"Status: {testItem.Status}",
                            new XAttribute("class", $"status{testItem.Status}")
                        )
                    ),
                    new XElement("div",
                        new XAttribute("class", "panel-body"),
                        new XElement("p", $"Description: {testItem.Description}"),
                        GetOverall(testItem),
                        new XElement("p", $"Logs: {testItem.LogMessages}")
                    )
                )
            ));
            if (testItem.Childs.Count != 0) {
                XElement acc = new XElement("div",
                    new XAttribute("class", "child"),
                    new XAttribute("style", "display: none; margin-left: 3%")
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
