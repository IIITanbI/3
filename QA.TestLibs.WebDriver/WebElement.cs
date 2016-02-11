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

        [XmlProperty("Is element frame?")]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Attribute, "frame")]
        [XmlConstraint("IsFrame", true)]
        public string FrameId { get; set; } = null;

        private string _info = null;
        public override string ToString()
        {
            if (_info != null)
                return _info;

            _info = $"Element info:\n\tName: {Name}\n\tDescription:{Description}\n{Locator}";

            return _info;
        }
    }
}
