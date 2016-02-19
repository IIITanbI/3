//namespace QA.TestLibs.Report
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Linq;
//    using System.Text;
//    using System.Threading.Tasks;
//    using System.Xml.Linq;
//    using QA.TestLibs.TestMetadata;

//    public class ReportGenerator : IReportGenerator
//    {
//        public XElement CreateReport(TestItem testItem, TestEnvironmentInfo testEnvironmentInfo)
//        {
//            var html = new XElement("html",
//                new XAttribute("lang", "en"),
//                GetHead(),
//                GetBody(testItem, testEnvironmentInfo)
//            );

//            return html;
//        }

//        public XElement GetHead()
//        {
//            var head = new XElement("head");

//            var charset = new XElement("meta", new XAttribute("charset", "utf-8"));

//            var httpEquiv = new XElement("meta",
//                new XAttribute("http-equiv", "X-UA-Compatible"),
//                new XAttribute("content", "IE=edge")
//            );

//            var name = new XElement("meta",
//                new XAttribute("name", "viewport"),
//                new XAttribute("content", "width=device-width, initial-scale=1")
//            );

//            var title = new XElement("title", "Test result");

//            var css = new XElement("link",
//                new XAttribute("rel", "stylesheet"),
//                new XAttribute("href", "https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css")
//            );

//            head.Add(charset, httpEquiv, name, title, css);
//            return head;
//        }

//        public XElement GetBody(TestItem testItem, TestEnvironmentInfo testEnvironmentInfo)
//        {
//            var body = new XElement("body");

//            var js = new XElement("script", "", new XAttribute("src", "https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/bootstrap.min.js"));

//            var jQuery = new XElement("script", "", new XAttribute("src", "https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"));
//            var JS = new XElement("script", "", new XAttribute("src", "JS.txt"));

//            var jsCustom = new XElement("script",
//                "$(function(){ $('.btn').click(function(e){ $(this).parent().parent().parent().children('.child').toggle(); }); });"
//            );

//            var container = new XElement("div", new XAttribute("class", "container"),
//                GetEnvironment(testEnvironmentInfo),
//                GetReport(testItem)
//            );

//            body.Add(container, jQuery, js, JS);

//            return body;
//        }

//        public XElement GetEnvironment(TestEnvironmentInfo testEnvironmentInfo)
//        {
//            var environment = new XElement("div", new XAttribute("class", "panel panel-default"), new XAttribute("style", "margin-top: 3%"));

//            var heading = new XElement("div", new XElement("mark", "Environment"), new XAttribute("class", "panel-heading"));

//            var table = new XElement("table", new XAttribute("class", "table"));

//            var thead = new XElement("thead",
//                new XElement("tr",
//                    new XElement("th", "CLR version"),
//                    new XElement("th", "OS name"),
//                    new XElement("th", "OS version"),
//                    new XElement("th", "Platform"),
//                    new XElement("th", "Machine name"),
//                    new XElement("th", "User"),
//                    new XElement("th", "User domain")
//                )
//            );

//            var tbody = new XElement("tbody",
//                new XElement("tr",
//                    new XElement("td", testEnvironmentInfo.CLRVersion),
//                    new XElement("td", testEnvironmentInfo.OSName),
//                    new XElement("td", testEnvironmentInfo.Platform),
//                    new XElement("td", testEnvironmentInfo.MachineName),
//                    new XElement("td", testEnvironmentInfo.User),
//                    new XElement("td", testEnvironmentInfo.UserDomain)
//                )
//            );

//            table.Add(thead, tbody);

//            environment.Add(heading, table);
//            return environment;
//        }

//        public XElement GetOverall(TestItem testItem)
//        {
//            if (testItem.Type == TestItemType.Test) return null;

//            var table = new XElement("table", new XAttribute("class", "table"));

//            var thead = new XElement("thead",
//                new XElement("tr",
//                    new XElement("th", "Total"),
//                    new XElement("th", "Passed"),
//                    new XElement("th", "Failed"),
//                    new XElement("th", "Skipped")
//                )
//            );

//            var tbody = new XElement("tbody",
//                new XElement("tr",
//                    new XElement("td", testItem.GetTotal()),
//                    new XElement("td", testItem.GetWithStatus(TestItemStatus.Passed)),
//                    new XElement("td", testItem.GetWithStatus(TestItemStatus.Failed)),
//                    new XElement("td", testItem.GetWithStatus(TestItemStatus.Skipped))
//                )
//            );

//            table.Add(thead, tbody);
//            return table;
//        }

//        public XElement GetReport(TestItem testItem)
//        {
//            XElement panelMain = new XElement("div",  new XAttribute("class", "panel panel-default accordion"));
//            XElement panelHeading = new XElement("div",
//                        new XAttribute("class", "panel-heading"),
//                        new XElement("p", $"{testItem.Type}: {testItem.Name}"),
//                        new XElement("p", $"Status: {testItem.Status}",
//                            new XAttribute("class", $"status{testItem.Status}")
//                        )
//                    );
//            XElement panelBody = new XElement("div",
//                        new XAttribute("class", "panel-body"),
//                        new XElement("p", $"Description: {testItem.Description}"),
//                        GetOverall(testItem),
//                        new XElement("p", $"Logs: {testItem.LogMessages}")
//                    );

//            if (testItem.Childs.Count > 0)
//                panelHeading.Add(new XElement("button", new XAttribute("class", "btn test"), "Tests"));
//            if (testItem.Steps.Count > -1)
//                panelHeading.Add(new XElement("button", new XAttribute("class", "btn log"), "Logs"));

//            panelMain.Add(panelHeading);
//            panelMain.Add(panelBody);
//            XElement cont = (new XElement("div",
//                new XAttribute("class", "parent"),
//                panelMain
//            ));
//            /*
//            XElement cont = (new XElement("div",
//                new XAttribute("class", "parent"),
//                new XElement("div",
//                    new XAttribute("class", "panel panel-default accordion"),
//                    new XElement("div",
//                        new XAttribute("class", "panel-heading"),
//                        new XElement("p", $"{testItem.Type}: {testItem.Name}"),
//                        new XElement("p", $"Status: {testItem.Status}",
//                            new XAttribute("class", $"status{testItem.Status}")
//                        ),
//                        //<button type="button">Click Me!</button>
//                        new XElement("button", new XAttribute("class", "btn"), "Click me")
//                    ),
//                    new XElement("div",
//                        new XAttribute("class", "panel-body"),
//                        new XElement("p", $"Description: {testItem.Description}"),
//                        GetOverall(testItem),
//                        new XElement("p", $"Logs: {testItem.LogMessages}")
//                    )
//                )
//            ));
//            */

//            if (testItem.Childs.Count != 0)
//            {
//                XElement acc = new XElement("div",
//                    new XAttribute("class", "child"),
//                    new XAttribute("style", "display: none; margin-left: 3%")
//                );
//                cont.Add(acc);
//                foreach (var item in testItem.Childs)
//                {
//                    acc.Add(GetReport(item));
//                }
//            }
//            if (testItem.LogMessages.Count != 0)
//            {
//                XElement acc = new XElement("div",
//                    new XAttribute("class", "log"),
//                    new XAttribute("style", "display: none; margin-left: 3%")
//                );
//                cont.Add(acc);
//                foreach (var log in testItem.LogMessages)
//                {
//                    XElement logPanel = new XElement("div",
//                        new XAttribute("class", "panel-heading"),
//                        new XElement("p", $"Message: {log.Message}"),
//                        new XElement("p", $"Data: {log.DataStemp}"),
//                        new XElement("p", $"Level: {log.Level}",
//                            new XAttribute("class", $"level{log.Level}")
//                        ),
//                        new XElement("p", $"Exception: {log.Exception}")
//                    );
//                    acc.Add(logPanel);
//                }
//            }

//            return cont;
//        }


        
//    }
//}


//using System.Globalization;

//namespace QA.TestLibs.Report
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Linq;
//    using System.Text;
//    using System.Threading.Tasks;
//    using System.Xml.Linq;
//    using QA.TestLibs.TestMetadata;

//    public class ReportGenerator : IReportGenerator
//    {
//        public XElement CreateReport(TestItem testItem, TestEnvironmentInfo info)
//        {
//            var head = ReportGenerator.CreateReportHead("3.3.6");
//            //var body = ReportGenerator.GenerateHtmlTestCaseTwo(DataSource.GetSample().Childs[0].Childs[0]);
//            var body = ReportGenerator.Go(DataSource.GetSample());
//            XElement main = new XElement("div");
//            main.Add(head);
//            main.Add(body);
//            return main;
//        }

//        public static XElement CreateReportHead(string bootstrapVersion)
//        {
//            var head = new XElement("head");
//            var link = new XElement("link", "",
//                                new XAttribute("rel", "stylesheet"),
//                                new XAttribute("href", $"http://maxcdn.bootstrapcdn.com/bootstrap/{bootstrapVersion}/css/bootstrap.min.css"));

//            var jqueryScript = new XElement("script", "",
//                new XAttribute("src", $"https://ajax.googleapis.com/ajax/libs/jquery/1.12.0/jquery.min.js"));

//            var bootstrapScript = new XElement("script", "",
//               new XAttribute("src", $"http://maxcdn.bootstrapcdn.com/bootstrap/{bootstrapVersion}/js/bootstrap.min.js"));

//            head.Add(link);
//            head.Add(jqueryScript);
//            head.Add(bootstrapScript);

//            return head;
//        }

//        public static XElement CreateElement(string tagName, string attributeName, string attributeValue)
//        {
//            var element = new XElement(tagName);
//            var attribute = new XAttribute(attributeName, attributeValue);
//            element.Add("");
//            element.Add(attribute);
//            return element;
//        }




//        //heading it's 'a' or 'button' element
//        public static void CreateAccordion(XElement root, string accordionId, XElement heading, XElement body, string bodyId)
//        {
//            //create main container for accordion-element
//            //var accordion = CreateElement("div", "id", accordionId);
//            var accordion = new XElement("div", "",
//                new XAttribute("id", accordionId),
//                new XAttribute("class", "accordion"));
//            //var attribute = new XAttribute("class", "accordion");
//            //accordion.Add(attribute);

//            //create heading-element for accordion
//            var group = CreateElement("div", "class", "accordion-group");

//            var head = CreateElement("div", "class", "accordion-heading");
//            heading.Add(new XAttribute("class", "accordion-toggle"),
//                     new XAttribute("data-parent", $"#{accordionId}"),
//                     new XAttribute("data-toggle", "collapse"),
//                     new XAttribute("href", $"#{bodyId}"));

//            head.Add(heading);
//            group.Add(head);

//            //create body for accordion
//            var bodyElement = CreateElement("div", "class", "accordion-body collapse");
//            bodyElement.Add(new XAttribute("id", bodyId));

//            body.Add(new XAttribute("class", "accordion-inner"));
//            bodyElement.Add(body);

//            group.Add(bodyElement);
//            accordion.Add(group);

//            root.Add(accordion);
//        }



//        public static XElement GetTableHeader(string[] headerNames)
//        {
//            XElement thead = new XElement("thead");
//            XElement theadRow = new XElement("tr");

//            foreach (string name in headerNames)
//            {
//                XElement th = new XElement("th", name);
//                th.SetAttributeValue("bgcolor", "lightgrey");
//                theadRow.Add(th);
//            }

//            thead.Add(theadRow);
//            return thead;
//        }
//        public static XElement GetTable(string[] headerNames)
//        {
//            XElement table = new XElement("table");
//            XElement thead = GetTableHeader(headerNames);

//            table.Add(thead);
//            table.SetAttributeValue("border", "1");
//            table.SetAttributeValue("bordercolor", "#666666");
//            table.SetAttributeValue("cellpadding", "10");
//            table.SetAttributeValue("style", "width:100%;text-align:center;");

//            return table;
//        }

//        static int id = 0;

//        public static XElement Go(TestItem project)
//        {
//            id++;
//            XElement main = new XElement("div");

//            XElement table = GetTable(new[] { "Name", "Status", "Duration" });
//            XElement tbody = new XElement("tbody");
//            XElement tr = new XElement("tr");
//            tr.Add(new XElement("td", project.Name, new XAttribute("style", "color:red")),
//                    new XElement("td", project.Status),
//                    new XElement("td", project.Duration)
//                    );
//            tbody.Add(tr);
//            table.Add(tbody);

//            var panelBody = CreateElement("div", "class", "panel-body");
//            XElement link = new XElement("a");
//            link.Add(table);
//            var parentAccordionBody = new XElement("div");
//            CreateAccordion(panelBody, "parent-accordion" + id, link, parentAccordionBody, "parent-accordion-body" + id);

//            if (project.Childs.Count > 0)
//            {

//                id++;
//                var stepsPanelBody = CreateElement("div", "class", "panel-body");

//                XElement steps = new XElement("div", "Childs");
//                XElement stepsLink = new XElement("a");
//                stepsLink.Add(steps);
//                var stepsParentAccordionBody = new XElement("div");
//                CreateAccordion(stepsPanelBody, "parent-accordion" + id, stepsLink, stepsParentAccordionBody, "parent-accordion-body" + id);
//                parentAccordionBody.Add(stepsPanelBody);

//                project.Childs.ForEach((x) => stepsParentAccordionBody.Add(Go(x)));
//            }
//            else
//                project.Steps.ForEach((x) => parentAccordionBody.Add(Go(x)));

//            main.Add(panelBody);
//            return main;
//        }


//        public static XElement Go(Step step)
//        {
//            id++;
//            XElement main = new XElement("div");

//            XElement table = GetTable(new[] { "Name", "Status", "Duration" });
//            XElement tbody = new XElement("tbody");
//            XElement tr = new XElement("tr");
//            tr.Add(new XElement("td", step.Name, new XAttribute("style", "color:red")),
//                    new XElement("td", step.Status),
//                    new XElement("td", step.Duration)
//                    );
//            tbody.Add(tr);
//            table.Add(tbody);

//            var panelBody = CreateElement("div", "class", "panel-body");
//            XElement link = new XElement("a");
//            link.Add(table);
//            var parentAccordionBody = new XElement("div");
//            CreateAccordion(panelBody, "parent-accordion" + id, link, parentAccordionBody, "parent-accordion-body" + id);

//            if (step.Messages.Count > 0)
//            {
//                foreach (var log in step.Messages)
//                {
//                    XElement d1 = new XElement("div", log.Message);
//                    parentAccordionBody.Add(d1);
//                }
//            }
//            else parentAccordionBody.Value = "No messages";

//            main.Add(panelBody);
//            return main;
//        }
//    }
//}