namespace QA.TestLibs.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using XmlDesiarilization;

    [XmlType("Test step config")]
    [XmlLocation("step", "testingStep")]
    public class TestStep : XmlBaseType
    {
        [XmlProperty("Name of TestStep")]
        public string Name { get; set; }

        [XmlProperty("Description for TestStep")]
        public string Description { get; set; }

        [XmlProperty("Order of TestStep. Pre, Case, CasePost or Post", IsRequired = false)]
        [XmlLocation(XmlLocationType.Attribute | XmlLocationType.Element, "order")]
        public Order StepOrder { get; set; } = Order.Case;

        [XmlProperty("Is step skipped on fail", IsRequired = false)]
        [XmlLocation(XmlLocationType.Attribute | XmlLocationType.Element, "skipOnFail")]
        public bool IsSkippedOnFail { get; set; } = false;

        [XmlProperty("Number of tries for the step execution", IsRequired = false)]
        [XmlLocation(XmlLocationType.Attribute | XmlLocationType.Element, "retries")]
        public int TryCount { get; set; } = 1;

        public enum Order
        {
            Pre, Case, CasePost, Post
        }
    }
}
