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
        [XmlProperty("List of test item childes")]
        public string Name { get; set; }

        [XmlProperty("List of test description")]
        public string Description { get; set; };

        [XmlProperty("List of test item childes", IsRequired = false)]
        public List<TestItem> Childs { get; set; } = new List<TestItem>();

        [XmlProperty("Test item type")]
        public string Type { get; set; }

        [XmlProperty("Test item steps", IsRequired = false)]
        public List<Step> Steps { get; set; } = new List<Step>();

        [XmlProperty("Test item log")]
        public List<LogMessage> LogMessages { get; set; }

        [XmlProperty("Test item tries", IsRequired = false)]
        public List<TestTry> Tries { get; set; } = new List<TestTry>();

        [XmlProperty("Test item status")]
        public string Status { get; set; }

        [XmlProperty("Test item duration")]
        public TimeSpan Duration { get; set; }
    }
}
