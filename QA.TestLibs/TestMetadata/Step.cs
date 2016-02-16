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
        public List<LogMessage> StepLogMessages { get; set; }

        [XmlProperty("Step status")]
        public string StepStatus { get; set; }

        [XmlProperty("Step duration")]
        public TimeSpan StepDuration { get; set; }
    }
}
