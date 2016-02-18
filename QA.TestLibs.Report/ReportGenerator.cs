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
        private  int _id = 0;

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

            var container = new XElement("div", new XAttribute("class", "container"),
                GetEnvironment(testEnvironmentInfo),
                GetReport(testItem)
            );

            body.Add(container, jQuery, js);

            return body;
        }

        public XElement GetEnvironment(TestEnvironmentInfo testEnvironmentInfo)
        {
            var environment = new XElement("div", new XAttribute("class", "panel panel-default"));

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

        public XElement CreatePanelAccordionElement(XElement heading, XElement body, string panelClassType)
        {
            var index = ++_id;

            var divPanelGroup = new XElement("div", new XAttribute("class", "panel-group"));
            var divPanelType = new XElement("div", new XAttribute("class", $"panel {panelClassType}"));
            divPanelGroup.Add(divPanelType);

            var divPanelHeading = new XElement("div",
                new XElement("h4", new XAttribute("class", "panel-title")),
                new XAttribute("class", "panel-heading")
            );
            divPanelType.Add(divPanelHeading);

            heading.Add(
                new XAttribute("data-toggle", "collapse"),
                new XAttribute("href", $"#accordion-body-{index}")
            );
            divPanelHeading.Element("h4")?.Add(heading);

            var accordionBody = new XElement("div",
                new XAttribute("id", $"accordion-body-{index}"),
                new XAttribute("class", "panel-collapse collapse")
            );

            var attribute = body.Attribute("class");

            if (attribute == null)
            {
                body.Add(new XAttribute("class", "panel body"));
            }
            else {
                attribute.Value += " panel-body";
            }

            accordionBody.Add(body);
            divPanelType.Add(accordionBody);

            return divPanelGroup;
        }

        public XElement CreatePanelAccordionElement(XElement heading, XElement body)
        {
            var index = ++_id;

            var divPanelGroup = new XElement("div", new XAttribute("class", "panel-group"));

            heading.Add(
                new XAttribute("data-toggle", "collapse"),
                new XAttribute("href", $"#accordion-body-{index}")
            );
            divPanelGroup.Element("h4")?.Add(heading);

            var accordionBody = new XElement("div",
                new XAttribute("id", $"accordion-body-{index}"),
                new XAttribute("class", "panel-collapse collapse")
            );

            var attribute = body.Attribute("class");

            if (attribute == null)
            {
                body.Add(new XAttribute("class", "panel body"));
            }
            else {
                attribute.Value += " panel-body";
            }

            accordionBody.Add(body);
            divPanelGroup.Add(accordionBody);

            return divPanelGroup;
        }

        public XElement GetReport(TestItem item)
        {
            var head = new XElement("div",
                new XElement("span", item.Description),
                new XElement("div",
                $"Name: {item.Name}; Status: {item.Status}; Duration : {item.Duration}")
            );

            var container = new XElement("div", new XAttribute("id", "container-nodes"));
            var accordion = CreatePanelAccordionElement(head, container, "panel-default");

            if (item.Childs.Count > 0)
            {
                var headingChilds = new XElement("div", "Childs");
                var childs = new XElement("div", new XAttribute("id", "container-childs"));

                var childsAccordion = CreatePanelAccordionElement(headingChilds, childs);
                container.Add(childsAccordion);

                item.Childs.ForEach((x) => childs.Add(GetReport(x)));
            }

            if (item.Steps.Count > 0)
            {
                var headingSteps = new XElement("div", "Steps");
                var steps = new XElement("div",
                    new XAttribute("id", "container-steps"),
                    new XAttribute("class", "list-group")
                );

                var stepsAccordion = CreatePanelAccordionElement(headingSteps, steps);
                container.Add(stepsAccordion);

                foreach (var step in item.Steps)
                {
                    var element = new XElement("div",
                        new XElement("span", step.Description),
                        new XElement("div",
                        $"Name: {item.Name}; Status: {step.Status}; Duration : {step.Duration}"),
                            new XAttribute("class", "list-group-item")
                    );

                    steps.Add(element);
                }
            }
            return accordion;
        }
    }
}
