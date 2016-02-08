namespace QA.TestLibs.WebDriver.WebDriverConfigs
{
    using OpenQA.Selenium;
    using OpenQA.Selenium.Firefox;
    using OpenQA.Selenium.Remote;
    using XmlType;

    [XmlType("Firefox WebDriver config")]
    [XmlLocation("firefoxWebDriverConfig", "firefoxWebDriver")]
    public class FirefoxWebDriverConfig : WebDriverConfig
    {
        [XmlProperty("Path to Firefox profile")]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Attribute, "profileDirectoryPath", "pathToProfileDirectory")]
        public string ProfileDirectoryPath { get; set; }

        public override IWebDriver CreateLocalDriver()
        {
            var profile = CreateProfile();
            return new FirefoxDriver(profile);
        }

        public override IWebDriver CreateRemoteDriver()
        {
            var profile = CreateProfile();

            var capability = DesiredCapabilities.Firefox();
            capability.SetCapability(CapabilityType.BrowserName, "Firefox");
            capability.SetCapability(CapabilityType.IsJavaScriptEnabled, IsJavaScriptEnabled.ToString().ToLower());
            capability.SetCapability(FirefoxDriver.ProfileCapabilityName, profile);

            return new RemoteWebDriver(capability);
        }

        protected FirefoxProfile CreateProfile()
        {
            var profile = new FirefoxProfile(ProfileDirectoryPath);
            profile.EnableNativeEvents = true;
            profile.DeleteAfterUse = true;
            profile.AcceptUntrustedCertificates = true;

            return profile;
        }
    }
}
