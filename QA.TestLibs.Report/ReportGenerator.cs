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

            var js = new XElement("script");
            js.Add(new XAttribute("src", "https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/bootstrap.min.js"));
            js.Add(new XAttribute("integrity", "sha384-0mSbJDEHialfmuBBQP6A4Qrprq5OVfW37PRR3j5ELqxss1yVqOtnepnHVP9aJ7xS"));
            js.Add(new XAttribute("crossorigin", "anonymous"));

            var container = new XElement("div", new XAttribute("class", "container"));
            container.Add(GetEnvironment(testEnvironmentInfo));

            body.Add(container, js);

            return body;
        }

        public XElement GetEnvironment(TestEnvironmentInfo testEnvironmentInfo)
        {
            var environment = new XElement("div");
            environment.Add(new XAttribute("class", "panel panel-default"));

            var heading = new XElement("div", "Environment");
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
            trb.Add(new XElement("th", testEnvironmentInfo.CLRVersion));
            trb.Add(new XElement("th", testEnvironmentInfo.OSName));
            trb.Add(new XElement("th", testEnvironmentInfo.OSVersion));
            trb.Add(new XElement("th", testEnvironmentInfo.Platform));
            trb.Add(new XElement("th", testEnvironmentInfo.MachineName));
            trb.Add(new XElement("th", testEnvironmentInfo.User));
            trb.Add(new XElement("th", testEnvironmentInfo.UserDomain));
            tbody.Add(trb);

            table.Add(thead, tbody);

            environment.Add(heading, table);
            return environment;
        }
    }
}
