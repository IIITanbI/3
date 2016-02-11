namespace QA.TestLibs.WebDriver.WebDriverConfigs
{
    using OpenQA.Selenium;
    using OpenQA.Selenium.IE;
    using OpenQA.Selenium.Remote;
    using XmlDesiarilization;

    [XmlType("Chrome IE config")]
    [XmlLocation("ieWebDriverConfig", "ieWebDriver")]
    public class IEWebDriverConfig : WebDriverConfig
    {
        [XmlProperty("Path to IE profile")]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Attribute, "profileDirectoryPath", "pathToProfileDirectory")]
        public string ProfileDirectoryPath { get; set; }

        public override IWebDriver CreateLocalDriver()
        {
            DriverType = WebDriverType.IE;
            var driverService = InternetExplorerDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;
            var internetExplorerOptions = new InternetExplorerOptions
            {
                IntroduceInstabilityByIgnoringProtectedModeSettings = true,
                InitialBrowserUrl = "about:blank",
                EnableNativeEvents = true
            };
            return new InternetExplorerDriver(driverService, internetExplorerOptions);
        }

        public override IWebDriver CreateRemoteDriver()
        {
            var capability = DesiredCapabilities.InternetExplorer();
            return new RemoteWebDriver(capability);
        }
    }
}
