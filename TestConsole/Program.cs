using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using QA.TestLibs.Report;
using QA.TestLibs;
using System.IO;
using QA.TestLibs.TestMetadata;
using System.Management;

namespace TestConsole
{
    class Program
    {
        public static string GetOSName()
        {
            var name = (from x in new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem").Get().OfType<ManagementObject>()
                        select x.GetPropertyValue("Caption")).FirstOrDefault();
            return name != null ? name.ToString() : "Unknown";
        }

        static void Main(string[] args)
        {

            
            //var head = ReportGenerator.CreateReportHead("3.3.6");
            ////var body = ReportGenerator.GenerateHtmlTestCaseTwo(DataSource.GetSample().Childs[0].Childs[0]);
            //var body = ReportGenerator.Go(DataSource.GetSample());
            //XElement main = new XElement("div");
            //main.Add(head);
            //main.Add(body);

           // var main = new ReportGenerator().CreateReport(DataSource.GetSample(), new QA.TestLibs.TestMetadata.TestEnvironmentInfo());
            //var main = new ReportGenerator().CreateReport(DataSource.GetSample(), new QA.TestLibs.TestMetadata.TestEnvironmentInfo());
           // main.Save("html.html");


            ReflectionManager.LoadAssemblies(Directory.GetCurrentDirectory());

            var ds = DataSource.GetSample();

            var te = new TestEnvironmentInfo()
            {
                CLRVersion = Environment.Version.ToString(),
                OSName = GetOSName(),
                OSVersion = Environment.OSVersion.ToString(),
                Platform = Environment.OSVersion.Platform.ToString(),
                MachineName = Environment.MachineName.ToString(),
                User = Environment.UserName.ToString(),
                UserDomain = Environment.UserDomainName.ToString()
            };
            new ReportGenerator().CreateReport(ds, te).Save("out.html", SaveOptions.None);

            // var accordion = ReportGenerator.CreateAccordion(root, "head-first", head, body, "body-first");

            // Console.ReadKey();
        }
    }
}
