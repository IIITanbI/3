namespace QA.TestLibs.WebDriver.WebDriverConfigs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using QA.TestLibs.XmlType;

    [XmlType("Firefox WebDriver config")]
    public class FirefoxWebDriverConfig : WebDriverConfig
    {
        [XmlProperty("Path to Firefox profile")]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Attribute, "profilePath", "pathToProfile")]
        public string ProfilePath { get; set; }
    }
}
