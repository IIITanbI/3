namespace QA.TestLibs.WebDriver
{
    using System;
    using QA.TestLibs.XmlDesiarilization;
    using OpenQA.Selenium;

    [Serializable]
    [XmlType("WebDriver configuration")]
    public abstract class WebDriverConfig : XmlBaseType
    {
        public WebDriverType DriverType { get; set; }

        [XmlProperty("Is JavaScript enabled? Default: true", IsRequired = false)]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Attribute, "isJavaScriptEnabled", "isJavaScript", "javaScript")]
        public bool IsJavaScriptEnabled { get; set; } = true;

        [XmlProperty("Timeout for element searching. Format: hh:mm:ss.ms. Default: 10s", IsRequired = false)]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Attribute, "searchTimeout", "elementSearchTimeout", "implicitlyWait")]
        public TimeSpan SearchTimeout { get; set; } = TimeSpan.FromSeconds(10);

        [XmlProperty("Timeout for page loading. Format: hh:mm:ss.ms. Default: 20s", IsRequired = false)]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Attribute, "pageLoadTimeout", "pageTimeout")]
        public TimeSpan PageLoadTimeout { get; set; } = TimeSpan.FromSeconds(20);

        [XmlProperty("Timeout for executing javascript. Format: hh:mm:ss.ms. Default: 10s", IsRequired = false)]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Attribute, "javaScriptTimeout", "javascriptTimeout")]
        [XmlConstraint("IsJavaScriptEnabled", true)]
        public TimeSpan JavaScriptTimeout { get; set; } = TimeSpan.FromSeconds(10);

        [XmlProperty("Timeout for wait operations. Format: hh:mm:ss.ms. Default: 20s", IsRequired = false)]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Attribute, "waitTimeout")]
        public TimeSpan WaitTimeout { get; set; } = TimeSpan.FromSeconds(20);

        [XmlProperty("Is grid used?. Default: false", IsRequired = false)]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Attribute, "isGrid", "isGridUsed")]
        public bool IsGrid { get; set; } = false;

        [XmlProperty("Uri to Grid")]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Attribute, "gridUri", "uriForGrid")]
        [XmlConstraint("IsGrid", true)]
        public string GridUri { get; set; } = null;

        [XmlProperty("Is highlightable?", IsRequired = false)]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Attribute, "highlightEnabled", "highlight")]
        [XmlConstraint("IsJavaScriptEnabled", true)]
        public bool IsHighlight { get; set; } = false;

        public IWebDriver CreateDriver()
        {
            IWebDriver driver = IsGrid
                ? CreateRemoteDriver()
                : CreateLocalDriver();

            driver.Manage().Timeouts().SetPageLoadTimeout(PageLoadTimeout);
            driver.Manage().Timeouts().ImplicitlyWait(SearchTimeout);

            if (IsJavaScriptEnabled)
                driver.Manage().Timeouts().SetScriptTimeout(JavaScriptTimeout);

            return driver;
        }

        public abstract IWebDriver CreateRemoteDriver();

        public abstract IWebDriver CreateLocalDriver();
    }
}
