namespace QA.TestLibs.WebDriver.WebDriverConfigs
{
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.Remote;
    using XmlDesiarilization;

    [XmlType("Chrome WebDriver config")]
    [XmlLocation("chromeWebDriverConfig", "chromeWebDriver")]
    public class ChromeWebDriverConfig : WebDriverConfig
    {
        [XmlProperty("Path to Chrome profile")]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Attribute, "profileDirectoryPath", "pathToProfileDirectory")]
        public string ProfileDirectoryPath { get; set; }

        public override IWebDriver CreateLocalDriver()
        {
            DriverType = WebDriverType.Chrome;
            var driverService = ChromeDriverService.CreateDefaultService();
            driverService.EnableVerboseLogging = true;
            driverService.HideCommandPromptWindow = true;
            var chromeOptions = new ChromeOptions();
            var capabilities = DesiredCapabilities.Chrome();
            chromeOptions.AddArguments(new string[] { "test-type" });
            capabilities.SetCapability(ChromeOptions.Capability, chromeOptions);
            return new ChromeDriver(driverService, chromeOptions);
        }

        public override IWebDriver CreateRemoteDriver()
        {
            var capability = DesiredCapabilities.Chrome();
            return new RemoteWebDriver(capability);
        }
    }
}
