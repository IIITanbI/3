namespace QA.TestLibs.WebDriver
{
    using OpenQA.Selenium;
    using System.Collections.Generic;
    using System.Linq;
    using XmlDesiarilization;
    using Exceptions;

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

        public WebElement this[string name]
        {
            get
            {
                var nameParts = name.Split('.');
                return this[nameParts];
            }
        }

        public WebElement this[string[] nameParts]
        {
            get
            {
                if (nameParts.Length == 1)
                {
                    return this;
                }
                var cwe = ChildWebElements.FirstOrDefault(c => c.Name == nameParts[1]);
                if (cwe == null)
                {
                    throw new TestLibsException($"Couldn't find child element with name: {nameParts[1]} in parent element with name: {Name}");
                }
                return cwe[nameParts.Skip(1).ToArray()];
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
