namespace QA.TestLibs.TestMetadata
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using XmlDesiarilization;

    [XmlType("Step config")]
    public class Step : XmlBaseType
    {
        [XmlProperty("Step log")]
        public List<LogMessage> Messages { get; set; } = new List<LogMessage>();

        [XmlProperty("Step status")]
        public TestItemStatus Status { get; set; }

        [XmlProperty("Step name")]
        public string Name { get; set; }

        [XmlProperty("Step description")]
        public string Description { get; set; }

        [XmlProperty("Step duration")]
        public TimeSpan Duration { get; set; }
    }
}
