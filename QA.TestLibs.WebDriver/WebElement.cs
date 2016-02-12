namespace QA.TestLibs.WebDriver
{
    using OpenQA.Selenium;
    using System.Collections.Generic;
    using XmlDesiarilization;

    [XmlType("WebElement config")]
    [XmlLocation("webElement")]
    public class WebElement : XmlBaseType
    {
        public WebElement ParentElement { get; set; }

        [XmlProperty("List of child WebElements", IsAssignableTypesAllowed = true, IsRequired = false)]
        [XmlLocation("childs")]
        public List<WebElement> ChildWebElements { get; set; } = new List<WebElement>();

        [XmlProperty("Locator for web element")]
        public WebLocator Locator { get; set; }

        [XmlProperty("Name of WebElement")]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Attribute, "elementName", "webElementName")]
        public string Name { get; set; }

        [XmlProperty("Description of WebElement")]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Attribute, "elementDescription", "webElementDescription")]
        public string Description { get; set; }

        [XmlProperty("Is element frame?", IsRequired = false)]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Attribute, "frame")]
        public bool IsFrame { get; set; } = false;

        [XmlProperty("Value for frame locator")]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Attribute, "frameLocatorValue")]
        [XmlConstraint("IsFrame", true)]
        [XmlConstraint("FrameType", FrameLocatorType.Locator, IsPositive = false)]
        public string FrameValue { get; set; } = null;

        [XmlProperty("Frame locator type. Id, Index or Locator", IsRequired = false)]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Attribute, "frameLocatorType")]
        [XmlConstraint("IsFrame", true)]
        public FrameLocatorType FrameType { get; set; } = FrameLocatorType.Id;

        private string _info = null;
        public override string ToString()
        {
            if (_info != null)
                return _info;

            _info = $"Element info:\n\tName: {Name}\n\tDescription:{Description}\n{Locator}";

            return _info;
        }

        public void Init()
        {
            foreach (var child in ChildWebElements)
            {
                child.ParentElement = this;
                child.Init();
            }
        }

        public enum FrameLocatorType
        {
            Id,
            Index,
            Locator
        }
    }
}
