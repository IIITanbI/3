namespace QA.TestLibs.WebDriver
{
    using System;
    using System.Collections.ObjectModel;
    using System.Collections.Generic;
    using System.Linq;
    using OpenQA.Selenium;
    using XmlType;

    [XmlType("WebElement config")]
    [XmlLocation("webElement")]
    public class WebElement : XmlBaseType
    {
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
    }
}
