using System.Globalization;

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
        private static int _id = 0;
        public XDocument CreateReport(TestItem testItem)
        {
            var report = new XDocument();
            report.Add(new XElement("html"));

            report.Element("html")?.Add(GetReportHead, GenerateHtmlForTestItem(testItem));

            return report;
        }


        public static XElement GetReportHead
        {
            get
            {
                var head = new XElement("head");
                var link = new XElement("link", new XAttribute("rel", "stylesheet"),
                    new XAttribute("href",
                        "http://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css"));


                var jqueryScript = new XElement("script", "",
               new XAttribute("src", "https://ajax.googleapis.com/ajax/libs/jquery/1.12.0/jquery.min.js"));

                var bootstrapScript = new XElement("script", "",
               new XAttribute("src", "http://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/bootstrap.min.js"));

                head.Add(link);
                head.Add(jqueryScript);
                head.Add(bootstrapScript);

                return head;
            }
        }
        public static XElement CreatePanelAccordionElement(XElement heading, XElement body, string panelClassType)
        {
            var index = ++_id;

            var divPanelGroup = new XElement("div", new XAttribute("class", "panel-group"));
            var divPanelType = new XElement("div", new XAttribute("class", $"panel {panelClassType}"));
            divPanelGroup.Add(divPanelType);

            var divPanelHeading = new XElement("div", new XElement("h4", new XAttribute("class", "panel-title")),
                                                      new XAttribute("class", "panel-heading"));
            divPanelType.Add(divPanelHeading);

            heading.Add(new XAttribute("data-toggle", "collapse"),
                        new XAttribute("href", $"#accordion-body-{index}"));
            divPanelHeading.Element("h4")?.Add(heading);

            var accordionBody = new XElement("div", new XAttribute("id", $"accordion-body-{index}"),
                                                    new XAttribute("class", "panel-collapse collapse"));

            var attribute = body.Attribute("class");

            if (attribute == null)
                body.Add(new XAttribute("class", "panel body"));

            else
                attribute.Value += " panel-body";

            accordionBody.Add(body);
            divPanelType.Add(accordionBody);


            return divPanelGroup;
        }
        public static XElement GenerateHtmlForTestItem(TestItem item)
        {
            // create accordion-heading
            var head = new XElement("div", new XElement("span", item.Description),
                                            new XElement("div",
            $"Name: {item.Name}; Status: {item.Status}; Duration : {item.Duration}"));


            //create accordion-body for nodes
            var container = new XElement("div", new XAttribute("id", "container-nodes"));
            var accordion = CreatePanelAccordionElement(head, container, "panel-default");


            if (item.Childs.Count > 0)
            {
                var headingChilds = new XElement("div", "Childs");
                var childs = new XElement("div", new XAttribute("id", "container-childs"));

                var childsAccordion = CreatePanelAccordionElement(headingChilds, childs, "panel-success");
                container.Add(childsAccordion);

                item.Childs.ForEach((x) => childs.Add(GenerateHtmlForTestItem(x)));
            }

            if (item.Steps.Count > 0)
            {
                var headingSteps = new XElement("div", "Steps");
                var steps = new XElement("div", new XAttribute("id", "container-steps"),
                                                new XAttribute("class", "list-group"));

                var stepsAccordion = CreatePanelAccordionElement(headingSteps, steps, "panel-warning");
                container.Add(stepsAccordion);

                foreach (var step in item.Steps)
                {
                    var element = new XElement("div", new XElement("span", step.Description),
                                                      new XElement("div",
                     $"Name: {item.Name}; Status: {step.Status}; Duration : {step.Duration}"),
                                                      new XAttribute("class", "list-group-item"));

                    steps.Add(element);
                }
            }


            return accordion;
        }

    }
}
