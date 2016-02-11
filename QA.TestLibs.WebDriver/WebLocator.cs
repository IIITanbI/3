namespace QA.TestLibs.WebDriver
{
    using OpenQA.Selenium;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using XmlDesiarilization;

    [XmlType("WebLocator config")]
    [XmlLocation("locator")]
    public class WebLocator : XmlBaseType
    {
        [XmlProperty("Locator type", IsRequired = false)]
        [XmlLocation("type")]
        public WebLocatorType LocatorType { get; set; } = WebLocatorType.XPath;

        [XmlProperty("XPath to element")]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Attribute | XmlLocationType.Value, "xpath", "xPath")]
        public string XPath { get; set; } = null;

        [XmlProperty("Locator value")]
        [XmlConstraint("LocatorType", WebLocatorType.XPath, IsPositive = false)]
        [XmlLocation("value")]
        public string LocatorValue { get; set; } = null;

        [XmlProperty("Is path relative?", IsRequired = false)]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Attribute, "relative")]
        public bool IsRelative { get; set; } = true;

        private By _locator = null;
        public By Get()
        {
            if (_locator != null)
                return _locator;

            switch (LocatorType)
            {
                case WebLocatorType.Class:
                    _locator = By.ClassName(LocatorValue);
                    break;
                case WebLocatorType.Css:
                    _locator = By.CssSelector(LocatorValue);
                    break;
                case WebLocatorType.Id:
                    _locator = By.Id(LocatorValue);
                    break;
                case WebLocatorType.Link:
                    _locator = By.LinkText(LocatorValue);
                    break;
                case WebLocatorType.Name:
                    _locator = By.Name(LocatorValue);
                    break;
                case WebLocatorType.PartialLink:
                    _locator = By.PartialLinkText(LocatorValue);
                    break;
                case WebLocatorType.Tag:
                    _locator = By.TagName(LocatorValue);
                    break;
                case WebLocatorType.XPath:
                    _locator = By.XPath(LocatorValue ?? XPath);
                    break;
                default:
                    break;
            }

            return _locator;
        }

        private string _info = null;
        public override string ToString()
        {
            if (_info != null)
                return _info;

            _info = $"Locator info:\n\tSelector: {LocatorType}\n\tValue: {LocatorValue ?? XPath}";
            if (LocatorType != WebLocatorType.XPath && XPath != null)
                _info += $"\n\tXPath: {XPath}";

            return _info;
        }
    }
}
