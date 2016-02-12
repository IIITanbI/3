namespace QA.TestLibs.WebDriver
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using XmlDesiarilization;

    [XmlType("Frame WebElement config")]
    [XmlLocation("frame", "frameElement")]
    public class FrameWebElement : WebElement
    {
        [XmlProperty("Is element frame?", IsRequired = false)]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Attribute, "frame")]
        public bool IsFrame { get; set; } = false;

        [XmlProperty("Value for frame locator")]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Attribute, "frameLocatorValue")]
        [XmlConstraint("IsFrame", true)]
        [XmlConstraint("FrameType", FrameLocatorType.Locator, IsPositive = false)]
        public string FrameValue { get; set; } = null;

        [XmlProperty("Frame locator type. Id, Index or Locator. Default: Locator", IsRequired = false)]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Attribute, "frameLocatorType")]
        [XmlConstraint("IsFrame", true)]
        public FrameLocatorType FrameType { get; set; } = FrameLocatorType.Locator;
    }
}
