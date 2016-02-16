namespace QA.TestLibs.TestMetadata
{
    using QA.TestLibs.XmlDesiarilization;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class LogMessage
    {
        [XmlProperty("Log level")]
        public string Level { get; set; }

        [XmlProperty("Log message")]
        public string Message { get; set; }

        [XmlProperty("Log exception")]
        public Exception Exception { get; set; }

        [XmlProperty("Log exception")]
        public DateTime DataStemp { get; set; }
    }
}
