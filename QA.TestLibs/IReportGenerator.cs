namespace QA.TestLibs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using TestMetadata;

    public interface IReportGenerator
    {
        XElement CreateReport(TestItem testItem);
    }
}
