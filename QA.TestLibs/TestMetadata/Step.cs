namespace QA.TestLibs.TestMetadata
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using XmlDesiarilization;

    public class Step
    {
        [XmlProperty("Step log")]
        public List<LogMessage> Messages { get; set; }

        [XmlProperty("Step status")]
        public string Status { get; set; }

        [XmlProperty("Step duration")]
        public TimeSpan Duration { get; set; }
    }
}
