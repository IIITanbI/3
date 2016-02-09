namespace QA.TestLibs.WebDriver
{
    using System;
    using QA.TestLibs.XmlDesiarilization;
    using OpenQA.Selenium;
    
    [XmlType("WebDriver configuration")]
    public abstract class WebDriverConfig : XmlBaseType
    {
        [XmlProperty("WebDriver type: Firefox, Chrome or IE")]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Attribute, "driverType", "webDriverType", "browser", "browserType")]
        public WebDriverType DriverType { get; set; }

        [XmlProperty("Is JavaScript enabled?")]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Attribute, "isJavaScriptEnabled", "isJavaScript", "javaScript")]
        public bool IsJavaScriptEnabled { get; set; }

        [XmlProperty("Timeout for element searching. Format: hh:mm:ss.ms")]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Attribute, "searchTimeout", "elementSearchTimeout", "implicitlyWait")]
        public TimeSpan SearchTimeout { get; set; }

        [XmlProperty("Timeout for page loading. Format: hh:mm:ss.ms")]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Attribute, "pageLoadTimeout", "pageTimeout")]
        public TimeSpan PageLoadTimeout { get; set; }

        [XmlProperty("Timeout for executing javascript. Format: hh:mm:ss.ms")]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Attribute, "javaScriptTimeout", "javascriptTimeout")]
        [XmlConstraint("IsJavaScriptEnabled", true)]
        public TimeSpan JavaScriptTimeout { get; set; }

        [XmlProperty("Timeout for wait operations. Format: hh:mm:ss.ms")]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Attribute, "waitTimeout")]
        public TimeSpan WaitTimeout { get; set; }

        [XmlProperty("Is grid used?")]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Attribute, "isGrid", "isGridUsed")]
        public bool IsGrid { get; set; }

        [XmlProperty("Uri to Grid")]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Attribute, "gridUri", "uriForGrid")]
        [XmlConstraint("IsGrid", true)]
        public bool GridUri { get; set; }

        [XmlProperty("Is highlightable?")]
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
