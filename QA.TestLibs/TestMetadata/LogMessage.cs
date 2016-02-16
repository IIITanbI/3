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
        public string LogLevel { get; set; }

        [XmlProperty("Log message")]
        public string LogMsg { get; set; }

        [XmlProperty("Log exception")]
        public Exception LogException { get; set; }

        [XmlProperty("Log exception")]
        public DateTime LogDataStemp { get; set; }
    }
}
