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
            var link = CreateElement("link", "rel", "stylesheet");

            var href = new XAttribute("href",
                $"http://maxcdn.bootstrapcdn.com/bootstrap/{bootstrapVersion}/css/bootstrap.min.css");
            link.Add(href);

            var jqueryScript = CreateElement("script", "src",
                "https://ajax.googleapis.com/ajax/libs/jquery/1.12.0/jquery.min.js");
            var bootstrapScript = CreateElement("script", "src",
                $"http://maxcdn.bootstrapcdn.com/bootstrap/{bootstrapVersion}/js/bootstrap.min.js");

            head.Add(link);
            head.Add(jqueryScript);
            head.Add(bootstrapScript);

            return head;
        }
        public static XElement CreateElement(string tagName, string attributeName, string attributeValue)
        {
            var element = new XElement(tagName);
            var attribute = new XAttribute(attributeName, attributeValue);

            element.Add(attribute);
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


            var tableInfo = CreateTable(true, "Name", "Status", "Duration");
            var testCaseId = new XAttribute("id","test-case");

            tableInfo.Add(testCaseId);
            panelBody.Add(tableInfo);

            var divTestCaseValue = CreateElement("div", "id", "test-case-values");
            tableInfo = CreateTable(false, item.Name, item.Status.ToString(), item.Duration.ToString());

            divTestCaseValue.Add(tableInfo);
            panelBody.Add(divTestCaseValue);

            var divSteps = CreateElement("div", "id", "test-case-steps");
            panelBody.Add(divSteps);

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

            panelBody.Add(logsHeading);

            var logs = CreateElement("div", "id", "logs");
            var logsAttribute = new XAttribute("class", "panel panel-default");
            logs.Add(logsAttribute);

            var logsInfo = CreateTable(true, "Level", "Message", "Exception", "Data stamp");

            logs.Add(logsInfo);
            panelBody.Add(logs);


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
        public static XElement CreateTable(bool isTHead = true, params string[] cells)
        {
            var table = CreateElement("table", "class", "table");

            var row = isTHead ? new XElement("th") : new XElement("tr");

            foreach (var cell in cells)
            {
                var element = new XElement("td", cell);
                row.Add(element);
            }

            table.Add(row);
            return table;
        }
    }
}
