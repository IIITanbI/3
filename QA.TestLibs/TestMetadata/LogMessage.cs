namespace QA.TestLibs.TestMetadata
{
    using QA.TestLibs.XmlDesiarilization;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [XmlType("LogMessage config")]
    public class LogMessage : XmlBaseType
    {
        [XmlProperty("Log level")]
        public LogLevel Level { get; set; }

        [XmlProperty("Log message")]
        public string Message { get; set; }

        [XmlProperty("Log exception")]
        public Exception Exception { get; set; }

        [XmlProperty("Log exception")]
        public DateTime DataStemp { get; set; }
    }
}
