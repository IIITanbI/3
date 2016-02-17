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
        public XDocument CreateReport(TestItem testItem, TestEnvironmentInfo testEnvironmentInfo)
        {
            XDocument report = new XDocument();
            report.AddFirst(new XDocumentType("html", null, null, null));

            var html = new XElement("html");
            html.Add(new XAttribute("lang", "en"));
            html.Add(GetHead());
            html.Add(GetBody(testItem, testEnvironmentInfo));

            report.Add(html);

            return report;
        }

        public XElement GetHead()
        {
            var head = new XElement("head");

            var charset = new XElement("meta");
            charset.Add(new XAttribute("charset", "utf-8"));

            var httpEquiv = new XElement("meta");
            httpEquiv.Add(new XAttribute("http-equiv", "X-UA-Compatible"));
            httpEquiv.Add(new XAttribute("content", "IE=edge"));

            var name = new XElement("meta");
            name.Add(new XAttribute("name", "viewport"));
            name.Add(new XAttribute("content", "width=device-width, initial-scale=1"));

            var title = new XElement("title", "Test result");

            var css = new XElement("link");
            css.Add(new XAttribute("rel", "stylesheet"));
            css.Add(new XAttribute("href", "https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css"));
            css.Add(new XAttribute("integrity", "sha384-1q8mTJOASx8j1Au+a5WDVnPi2lkFfwwEAa8hDDdjZlpLegxhjVME1fgjWPGmkzs7"));
            css.Add(new XAttribute("crossorigin", "anonymous"));

            head.Add(charset, httpEquiv, name, title, css);
            return head;
        }

        public XElement GetBody(TestItem testItem, TestEnvironmentInfo testEnvironmentInfo)
        {
            var body = new XElement("body");

            var js = new XElement("script", "");
            js.Add(new XAttribute("src", "https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/bootstrap.min.js"));
            js.Add(new XAttribute("integrity", "sha384-0mSbJDEHialfmuBBQP6A4Qrprq5OVfW37PRR3j5ELqxss1yVqOtnepnHVP9aJ7xS"));
            js.Add(new XAttribute("crossorigin", "anonymous"));

            var jQuery = new XElement("script", "");
            jQuery.Add(new XAttribute("src", "https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"));

            var container = new XElement("div", new XAttribute("class", "container"));
            container.Add(GetEnvironment(testEnvironmentInfo));
            container.Add(GetReport(testItem));

            body.Add(container, jQuery, js);

            return body;
        }

        public XElement GetEnvironment(TestEnvironmentInfo testEnvironmentInfo)
        {
            var environment = new XElement("div");
            environment.Add(new XAttribute("class", "panel panel-default"));

            var heading = new XElement("div", new XElement("mark", "Environment"));
            heading.Add(new XAttribute("class", "panel-heading"));

            var table = new XElement("table");
            table.Add(new XAttribute("class", "table"));

            var thead = new XElement("thead");
            var trh = new XElement("tr");
            trh.Add(new XElement("th", "CLR version"));
            trh.Add(new XElement("th", "OS name"));
            trh.Add(new XElement("th", "OS version"));
            trh.Add(new XElement("th", "Platform"));
            trh.Add(new XElement("th", "Machine name"));
            trh.Add(new XElement("th", "User"));
            trh.Add(new XElement("th", "User domain"));
            thead.Add(trh);

            var tbody = new XElement("tbody");
            var trb = new XElement("tr");
            trb.Add(new XElement("td", testEnvironmentInfo.CLRVersion));
            trb.Add(new XElement("td", testEnvironmentInfo.OSName));
            trb.Add(new XElement("td", testEnvironmentInfo.OSVersion));
            trb.Add(new XElement("td", testEnvironmentInfo.Platform));
            trb.Add(new XElement("td", testEnvironmentInfo.MachineName));
            trb.Add(new XElement("td", testEnvironmentInfo.User));
            trb.Add(new XElement("td", testEnvironmentInfo.UserDomain));
            tbody.Add(trb);

            table.Add(thead, tbody);

            environment.Add(heading, table);
            return environment;
        }

        public XElement GetReport(TestItem testItem)
        {
            var report = new XElement("div",
                new XAttribute("class", "panel-group"),
                new XAttribute("role", "tablist")
            );

            var panel = new XElement("div", new XAttribute("class", "panel panel-default"));

            var panelHead = new XElement("div",
                new XAttribute("class", "panel-heading"),
                new XAttribute("role", "tab"),
                new XAttribute("id", "collapseListGroupHeading1")
            );

            var panelHeadLink = new XElement("h4",
                new XAttribute("class", "panel-title"),
                new XElement("a",
                    new XAttribute("class", "collapsed"),
                    new XAttribute("role", "button"),
                    new XAttribute("data-toggle", "collapse"),
                    new XAttribute("href", "#collapseListGroup1"),
                    new XAttribute("aria-expanded", "false"),
                    new XAttribute("aria-controls", "collapseListGroup1"),
                    $"Name: {testItem.Name}, Description: {testItem.Description}"
            ));

            panelHead.Add(panelHeadLink);
            panel.Add(panelHead);

            if (testItem.Childs.Count != 0)
            {
                var panelGroup = new XElement("div",
                    new XAttribute("id", "collapseListGroup1"),
                    new XAttribute("class", "panel-collapse collapse"),
                    new XAttribute("role", "tabpanel"),
                    new XAttribute("aria-labelledby", "collapseListGroupHeading1"),
                    new XAttribute("aria-expanded", "false")
                );

                var ul = new XElement("ul",
                    new XAttribute("class", "list-group")
                );

                foreach (var item in testItem.Childs)
                {
                    ul.Add(new XElement("li",
                        new XAttribute("class", "list-group-item"),
                        $"Name: {item.Name}, Description: {item.Description}"
                    ));
                }

                panelGroup.Add(ul);

                panel.Add(panelGroup);
            }

            report.Add(panel);

            return report;
        }
    }
}
