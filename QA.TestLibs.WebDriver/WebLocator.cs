namespace QA.TestLibs.WebDriver
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using XmlType;

    [XmlType("WebLocator config")]
    [XmlLocation("locator")]
    public class WebLocator : XmlBaseType
    {
        [XmlProperty("Locator type", IsRequired = false)]
        [XmlLocation("type")]
        public WebLocatorType LocatorType { get; set; } = WebLocatorType.XPath;

        [XmlProperty("XPath to element")]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Attribute | XmlLocationType.Value, "xpath", "xPath")]
        public string XPath { get; set; }

        [XmlProperty("Locator value")]
        [XmlConstraint("LocatorType", WebLocatorType.XPath, IsPositive = false)]
        [XmlLocation("value")]
        public string LocatorValue { get; set; }
    }
}
