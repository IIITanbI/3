namespace QA.TestLibs.WebDriver
{
    using System;
    using System.Collections.ObjectModel;
    using System.Collections.Generic;
    using System.Linq;
    using OpenQA.Selenium;
    using XmlType;
    using System.Drawing;

    [XmlType("WebElement config")]
    [XmlLocation("webElement")]
    public class WebElement : XmlBaseType, IWebElement
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

        public string TagName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Text
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool Enabled
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool Selected
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Point Location
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Size Size
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool Displayed
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void SendKeys(string text)
        {
            throw new NotImplementedException();
        }

        public void Submit()
        {
            throw new NotImplementedException();
        }

        public void Click()
        {
            throw new NotImplementedException();
        }

        public string GetAttribute(string attributeName)
        {
            throw new NotImplementedException();
        }

        public string GetCssValue(string propertyName)
        {
            throw new NotImplementedException();
        }

        public IWebElement FindElement(By by)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            throw new NotImplementedException();
        }
    }
}
