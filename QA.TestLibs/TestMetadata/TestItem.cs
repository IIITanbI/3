namespace QA.TestLibs.TestMetadata
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using XmlDesiarilization;

    [XmlType("TestItem config")]
    public class TestItem : XmlBaseType
    {
        [XmlProperty("Test item name")]
        public string Name { get; set; }

        [XmlProperty("Test item description")]
        public string Description { get; set; }

        [XmlProperty("Test item type")]
        public TestItemType Type { get; set; }

        [XmlProperty("Test item log")]
        public List<LogMessage> LogMessages { get; set; } = new List<LogMessage>();

        [XmlProperty("Test item status")]
        public TestItemStatus Status { get; set; }

        [XmlProperty("Test item duration")]
        public TimeSpan Duration { get; set; }

        [XmlProperty("Test item steps", IsRequired = false)]
        public List<Step> Steps { get; set; } = new List<Step>();

        [XmlProperty("List of test item childes", IsRequired = false)]
        public List<TestItem> Childs { get; set; } = new List<TestItem>();

        [XmlProperty("Test item failed tries", IsRequired = false)]
        public List<TestItem> FailedTries { get; set; } = new List<TestItem>();
    }
}
