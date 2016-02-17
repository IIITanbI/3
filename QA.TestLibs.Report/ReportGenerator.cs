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
            var report = new XElement("html");

            var head = CreateReportHead("3.3.6");
            var body = new XElement("body");

            report.Add(head);
            report.Add(body);
            return report;
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
        public static XElement CreateElement(string tagName, object value, params XAttribute[] attributes)
        {
            var element = new XElement(tagName, value);
            
            foreach (var att in attributes)
            {
                element.Add(att);
            }
            element.Add("");
            return element;
        }

        public static XElement GenerateHtmlTestCase(TestItem item)
        {
            var testCase = CreateElement("div", "class", "test-case");

            var panel = new XElement("div");
            XAttribute attribute;

            switch (item.Status)
            {
                case Status.NotExecuted:
                    attribute = new XAttribute("class", "panel panel-info");
                    break;
                case Status.Unknown:
                    attribute = new XAttribute("class", "panel");
                    break;
                case Status.Passed:
                    attribute = new XAttribute("class", "panel panel-success");
                    break;
                case Status.Failed:
                    attribute = new XAttribute("class", "panel panel-danger");
                    break;
                case Status.Skipped:
                    attribute = new XAttribute("class", "panel panel-warning");
                    break;
                default:
                    // attribute = new XAttribute("class", "panel panel-primary");
                    throw new ArgumentOutOfRangeException();
            }

            testCase.Add(panel);
            panel.Add(attribute);

            var panelBody = CreateElement("div", "class", "panel-body");

            var description = CreateElement("div", "id", "test-case-description");
            var panelHeading = new XAttribute("class", "panel-heading");

            description.Add(panelHeading);
            description.Add(item.Description);

            testCase.Add(description);
            testCase.Add(panelBody);

            var mainAccordionGroup = CreateElement("div", "class", "accordion-group");
            panelBody.Add(mainAccordionGroup);


            var tableInfo = CreateTable(true, "Name", "Status", "Duration");
            var testCaseId = new XAttribute("id", "test-case");

            tableInfo.Add(testCaseId);
            mainAccordionGroup.Add(tableInfo);

            var divTestCaseValue = CreateElement("div", "id", "test-case-accordion");
            var accordionAttribute = new XAttribute("class", "accordion");

            divTestCaseValue.Add(accordionAttribute);
            tableInfo = CreateTable(false, item.Name, item.Status.ToString(), item.Duration.ToString());

            var headingAccordion = CreateElement("div", "class", "accordion-heading");
            headingAccordion.Add(tableInfo);
            mainAccordionGroup.Add(headingAccordion);

            var divSteps = CreateElement("div", "id", "test-case-steps");
            mainAccordionGroup.Add(divSteps);

            attribute = new XAttribute("class", "panel panel-default");
            divSteps.Add(attribute);


            var heading = CreateElement("div", "class", "panel-heading");
            heading.Add("Steps");

            divSteps.Add(heading);



            var stepDescription = CreateElement("div", "id", "steps-description");
            stepDescription.Add(attribute);

            divSteps.Add(stepDescription);


            var stepsPanelBody = CreateElement("div", "class", "panel-body");
            divSteps.Add(stepsPanelBody);

            var table = CreateElement("table", "class", "table");

            var stepsTableInfo = CreateTable(true, "Name", "Status", "Duration");
            var testCaseStepsId = new XAttribute("id", "test-case-steps");

            stepsTableInfo.Add(testCaseStepsId);
            stepsPanelBody.Add(stepsTableInfo);

            stepsPanelBody.Add(table);

            foreach (var step in item.Steps)
            {

                var row = new XElement("tr", new XElement("td", step.Name),
                                             new XElement("td", step.Status),
                                             new XElement("td", step.Duration.ToString())
                                          //   new XElement("div", step.Description)
                                          );

                table.Add(row);
            }

            var logsHeading = CreateElement("div", "class", "panel-heading");
            logsHeading.Add("Logs");

            mainAccordionGroup.Add(logsHeading);

            var logs = CreateElement("div", "id", "logs");
            var logsAttribute = new XAttribute("class", "panel panel-default");
            logs.Add(logsAttribute);

            var logsInfo = CreateTable(true, "Level", "Message", "Exception", "Data stamp");

            logs.Add(logsInfo);
            mainAccordionGroup.Add(logs);


            var messages = CreateElement("div", "id", "messages");
            logs.Add(messages);

            var messageTable = CreateElement("table", "class", "table");
            messages.Add(messageTable);

            foreach (var logItem in item.LogMessages)
            {
                var row = new XElement("tr", new XElement("td", logItem.Level),
                                             new XElement("td", logItem.Message),
                                             new XElement("td", logItem.Exception.Message),
                                             new XElement("td", logItem.DataStemp.ToString(CultureInfo.InvariantCulture))
                                          );
                messageTable.Add(row);
            }

            //don't generate treis
            return testCase;
        }
        public static XElement CreateTable(bool isThead = true, params string[] cells)
        {
            var table = CreateElement("table", "class", "table table-bordered");

            var row = isThead ? new XElement("thead") : new XElement("tr");

            foreach (var cell in cells)
            {
                var element = new XElement("td", cell);
                row.Add(element);
            }

            table.Add(row);
            return table;
        }


        //heading it's 'a' or 'button' element
        public static void CreateAccordion(XElement root, string accordionId, XElement heading, XElement body, string bodyId)
        {
            //create main container for accordion-element
            //var accordion = CreateElement("div", "id", accordionId);
            var accordion = CreateElement("div", null,
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

        public static XElement GenerateHtmlTestCaseTwo(TestItem item)
        {
            var testCase = CreateElement("div", "class", "test-case");

            var mainPanel = new XElement("div");
            XAttribute attribute;

            switch (item.Status)
            {
                case Status.NotExecuted:
                    attribute = new XAttribute("class", "panel panel-info");
                    break;
                case Status.Unknown:
                    attribute = new XAttribute("class", "panel");
                    break;
                case Status.Passed:
                    attribute = new XAttribute("class", "panel panel-success");
                    break;
                case Status.Failed:
                    attribute = new XAttribute("class", "panel panel-danger");
                    break;
                case Status.Skipped:
                    attribute = new XAttribute("class", "panel panel-warning");
                    break;
                default:
                    // attribute = new XAttribute("class", "panel panel-primary");
                    throw new ArgumentOutOfRangeException();
            }

            testCase.Add(mainPanel);
            mainPanel.Add(attribute);

            //create div for description test-case
            #region
            var testCaseDescription = CreateElement("div", "id", "test-case-description");
            var panelHeadingAttribute = new XAttribute("class", "panel-heading");

            testCaseDescription.Add(panelHeadingAttribute);
            testCaseDescription.Add(item.Description);

            mainPanel.Add(testCaseDescription);
            #endregion

            //create div for panel-body element where will be all data values test-case's
            #region

            var panelBody = CreateElement("div", "class", "panel-body");
            mainPanel.Add(panelBody);


            var testCaseFiledsDescription = CreateTable(true, "Name test-case", "Status test-case", "Duration test-case");
            var testCaseFieldsDescriptionId = new XAttribute("id", "test-case-fields-description");

            testCaseFiledsDescription.Add(testCaseFieldsDescriptionId);
            panelBody.Add(testCaseFiledsDescription);

            var parentAccordionBody = new XElement("div");

            var testCaseFieldsValues = CreateTable(false, item.Name, item.Status.ToString(), item.Duration.ToString());
            var testCaseFieldsValuesId = new XAttribute("id", "test-case-fields-values");

            testCaseFieldsValues.Add(testCaseFieldsValuesId);
            var link = new XElement("a");
            link.Add(testCaseFieldsValues);


            CreateAccordion(panelBody, "parent-accordion", link, parentAccordionBody, "parent-accordion-body");

            #endregion

            //create div child-accordion for steps
            #region

            var divSteps = CreateElement("div", "id", "test-case-steps");
            parentAccordionBody.Add(divSteps);

            attribute = new XAttribute("class", "panel panel-default");
            divSteps.Add(attribute);


            var headingStep = CreateElement("a", "id", "steps");
            headingStep.Add("Steps");

            var stepsBody = new XElement("div");
            CreateAccordion(divSteps, "test-step-case", headingStep, stepsBody, "step-body");

            var tableDescriptionStep = CreateTable(true, "name step", "status step", "duration step");
            stepsBody.Add(tableDescriptionStep);

            var stepsContainer = CreateElement("div", "id", "steps-container");
            stepsBody.Add(stepsContainer);

            var stepsTable = GenerateHtmlForSteps(item.Steps);
            stepsContainer.Add(stepsTable);

            var messagesLink = CreateElement("a", "id", "test-case-messages");
            messagesLink.Add("Messages");

            var messagesBody = new XElement("div");
            CreateAccordion(stepsContainer, "messages-accordion", messagesLink, messagesBody, "messages-value-body");

            var messageTable = CreateTable(true, "Level", "Message", "Exception", "Data stamp");
            var tbody = new XElement("tbody");

            messageTable.Add(tbody);
            messagesBody.Add(messageTable);

            foreach (var message in item.LogMessages)
            {
                var row = new XElement("tr", new XElement("td", message.Level),
                                             new XElement("td", message.Message),
                                             new XElement("td", message.Exception.Message),
                                             new XElement("td", message.DataStemp.ToString(CultureInfo.InvariantCulture)));

                tbody.Add(row);
            }

            #endregion
            return testCase;
        }

        public static XElement GenerateHtmlForSteps(IEnumerable<Step> steps)
        {
            var table = new XElement("table", new XAttribute("class", "table table-bordered"));

            foreach (var step in steps)
            {
                var row = new XElement("tr", new XElement("td", step.Name),
                                             new XElement("td", step.Status),
                                             new XElement("td", step.Duration.ToString()));
                table.Add(row);
            }
            return table;
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
                project.Childs.ForEach((x) => parentAccordionBody.Add(Go(x)));
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
