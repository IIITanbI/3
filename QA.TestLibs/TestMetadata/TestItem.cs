namespace QA.TestLibs.TestMetadata
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using XmlDesiarilization;

    public class TestItem
    {
        [XmlProperty("List of test item childes", IsRequired = false)]
        public List<TestItem> TestItemChilds { get; set; } = new List<TestItem>();

        [XmlProperty("Test item type")]
        public string TestItemType { get; set; }

        [XmlProperty("Test item steps", IsRequired = false)]
        public List<Step> TestItemSteps { get; set; } = new List<Step>();

        [XmlProperty("Test item log")]
        public List<LogMessage> TestItemLogMessages { get; set; }

        [XmlProperty("Test item tries", IsRequired = false)]
        public List<TestTry> TestItemTries { get; set; } = new List<TestTry>();

        [XmlProperty("Test item status")]
        public string TestItemStatus { get; set; }

        [XmlProperty("Test item duration")]
        public TimeSpan TestItemDuration { get; set; }
    }
}
