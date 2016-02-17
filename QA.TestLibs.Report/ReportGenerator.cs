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
        public XElement CreateReport(TestItem testItem)
        {
            var head = ReportGenerator.CreateReportHead("3.3.6");
            //var body = ReportGenerator.GenerateHtmlTestCaseTwo(DataSource.GetSample().Childs[0].Childs[0]);
            var body = ReportGenerator.Go(DataSource.GetSample());
            XElement main = new XElement("div");
            main.Add(head);
            main.Add(body);
            return main;
        }

        public static XElement CreateReportHead(string bootstrapVersion)
        {
            var head = new XElement("head");
            var link = new XElement("link", "", 
                                new XAttribute("rel", "stylesheet"), 
                                new XAttribute("href", $"http://maxcdn.bootstrapcdn.com/bootstrap/{bootstrapVersion}/css/bootstrap.min.css"));

            var jqueryScript = new XElement("script", "", 
                new XAttribute("src", $"https://ajax.googleapis.com/ajax/libs/jquery/1.12.0/jquery.min.js"));

            var bootstrapScript = new XElement("script", "",
               new XAttribute("src", $"http://maxcdn.bootstrapcdn.com/bootstrap/{bootstrapVersion}/js/bootstrap.min.js"));

            head.Add(link);
            head.Add(jqueryScript);
            head.Add(bootstrapScript);

            return head;
        }
        
        public static XElement CreateElement(string tagName, string attributeName, string attributeValue)
        {
            var element = new XElement(tagName);
            var attribute = new XAttribute(attributeName, attributeValue);
            element.Add("");
            element.Add(attribute);
            return element;
        }




        //heading it's 'a' or 'button' element
        public static void CreateAccordion(XElement root, string accordionId, XElement heading, XElement body, string bodyId)
        {
            //create main container for accordion-element
            //var accordion = CreateElement("div", "id", accordionId);
            var accordion = new XElement("div", "",
                new XAttribute("id", accordionId),
                new XAttribute("class", "accordion"));
           //var attribute = new XAttribute("class", "accordion");
           //accordion.Add(attribute);

            //create heading-element for accordion
            var group = CreateElement("div", "class", "accordion-group");

            var head = CreateElement("div", "class", "accordion-heading");
            heading.Add(new XAttribute("class", "accordion-toggle"),
                     new XAttribute("data-parent", $"#{accordionId}"),
                     new XAttribute("data-toggle", "collapse"),
                     new XAttribute("href", $"#{bodyId}"));

            head.Add(heading);
            group.Add(head);

            //create body for accordion
            var bodyElement = CreateElement("div", "class", "accordion-body collapse");
            bodyElement.Add(new XAttribute("id", bodyId));

            body.Add(new XAttribute("class", "accordion-inner"));
            bodyElement.Add(body);

            group.Add(bodyElement);
            accordion.Add(group);

            root.Add(accordion);
        }



        public static XElement GetTableHeader(string[] headerNames)
        {
            XElement thead = new XElement("thead");
            XElement theadRow = new XElement("tr");

            foreach (string name in headerNames)
            {
                XElement th = new XElement("th", name);
                th.SetAttributeValue("bgcolor", "lightgrey");
                theadRow.Add(th);
            }

            thead.Add(theadRow);
            return thead;
        }
        public static XElement GetTable(string[] headerNames)
        {
            XElement table = new XElement("table");
            XElement thead = GetTableHeader(headerNames);

            table.Add(thead);
            table.SetAttributeValue("border", "1");
            table.SetAttributeValue("bordercolor", "#666666");
            table.SetAttributeValue("cellpadding", "10");
            table.SetAttributeValue("style", "width:100%;text-align:center;");

            return table;
        }

        static int id = 0;

        public static XElement Go(TestItem project)
        {
            id++;
            XElement main = new XElement("div");

            XElement table = GetTable(new[] { "Name", "Status", "Duration" });
            XElement tbody = new XElement("tbody");
            XElement tr = new XElement("tr");
            tr.Add( new XElement("td", project.Name, new XAttribute("style", "color:red")),
                    new XElement("td", project.Status),
                    new XElement("td", project.Duration)
                    );
            tbody.Add(tr);
            table.Add(tbody);

            var panelBody = CreateElement("div", "class", "panel-body");
            XElement link = new XElement("a");
            link.Add(table);
            var parentAccordionBody = new XElement("div");
            CreateAccordion(panelBody, "parent-accordion"+id, link, parentAccordionBody, "parent-accordion-body"+id);

            if (project.Childs.Count > 0)
            {
                
                id++;
                var stepsPanelBody = CreateElement("div", "class", "panel-body");

                XElement steps = new XElement("div", "Childs");
                XElement stepsLink = new XElement("a");
                stepsLink.Add(steps);
                var stepsParentAccordionBody = new XElement("div");
                CreateAccordion(stepsPanelBody, "parent-accordion" + id, stepsLink, stepsParentAccordionBody, "parent-accordion-body" + id);
                parentAccordionBody.Add(stepsPanelBody);

                project.Childs.ForEach((x) => stepsParentAccordionBody.Add(Go(x)));
            }
            else
                project.Steps.ForEach((x) => parentAccordionBody.Add(Go(x)));
            
            main.Add(panelBody);
            return main;
        }


        public static XElement Go(Step step)
        {
            id++;
            XElement main = new XElement("div");

            XElement table = GetTable(new[] { "Name", "Status", "Duration" });
            XElement tbody = new XElement("tbody");
            XElement tr = new XElement("tr");
            tr.Add(new XElement("td", step.Name, new XAttribute("style", "color:red")),
                    new XElement("td", step.Status),
                    new XElement("td", step.Duration)
                    );
            tbody.Add(tr);
            table.Add(tbody);

            var panelBody = CreateElement("div", "class", "panel-body");
            XElement link = new XElement("a");
            link.Add(table);
            var parentAccordionBody = new XElement("div");
            CreateAccordion(panelBody, "parent-accordion" + id, link, parentAccordionBody, "parent-accordion-body" + id);

            if (step.Messages.Count > 0)
            {
                foreach(var log in step.Messages)
                {
                    XElement d1 = new XElement("div", log.Message);
                    parentAccordionBody.Add(d1);
                }
            }
            else parentAccordionBody.Value = "No messages";

            main.Add(panelBody);
            return main;
        }
    }
}
