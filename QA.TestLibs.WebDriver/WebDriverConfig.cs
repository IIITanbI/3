namespace QA.TestLibs.WebDriver
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using QA.TestLibs.XmlType;

    [XmlType("WebDriver configuration")]
    public class WebDriverConfig
    {
        [XmlProperty("WebDriver type: Firefox, Chrome or IE")]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Attribute, "driverType", "webDriverType", "browser", "browserType")]
        public WebDriverType DriverType { get; set; }

        [XmlProperty("Is JavaScript enabled?")]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Attribute, "isJavaScriptEnabled", "isJavaScript", "javaScriptEnabled")]
        public bool JavaScriptEnabled { get; set; }

        [XmlProperty("Timeout for element searching. Format: hh:mm:ss.ms")]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Attribute, "searchTimeout", "elementSearchTimeout", "implicitlyWait")]
        public TimeSpan SearhTimeout { get; set; }

        [XmlProperty("Timeout for page loading. Format: hh:mm:ss.ms")]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Attribute, "pageLoadTimeout", "pageTimeout")]
        public TimeSpan PageLoadTimeout { get; set; }

        [XmlProperty("Is grid used?")]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Attribute, "isGrid", "isGridUsed")]
        public bool IsGrid { get; set; }

        [XmlProperty("Uri to Grid")]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Attribute, "gridUri", "uriForGrid")]
        [XmlConstraint("IsGrid", true)]
        public bool GridUri { get; set; }
    }
}
