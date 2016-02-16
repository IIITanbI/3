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
            var body = new XElement("body");

            report.Add(body);
            //create root div-element
            var divInfo = CreateElement("div", "class", "panel panel-default");

            //create heading table-element
            #region
            var divHeading = CreateElement("div", "class", "panel-heading");
            var tableHeading = CreateElement("table", "class", "table");

            var thead = new XElement("thead");
            thead.Add(new XElement("td", "Name"));
            thead.Add(new XElement("td", "Status"));
            thead.Add(new XElement("td", "Duration"));

            tableHeading.Add(thead);
            divHeading.Add(tableHeading);
            #endregion
            //end heading table-element

            var divDescription = CreateElement("div", "class", "panel-footer");
            divDescription.Add(testItem.Description);


            var divBody = CreateElement("div", "class", "panel-body");

            var script = CreateElement("script", "src", "bootstrap.css");
            report.Add(script);

            return report;
        }


        private XElement CreateElement(string tagName, string attributeName, string attributeValue)
        {
            var element = new XElement(tagName);
            var attribute = new XAttribute(attributeName, attributeValue);

            element.Add(attribute);
            return element;
        }

        private XElement GenerateHtmlTestSuite(TestItem element)
        {
            var suite = CreateElement("div", "class", "testSuite");

            var table = CreateElement("table", "class", "table");
            var thead = new XElement("thead");

            table.Add(thead);
           
            

            return suite;
        }
    }
}
