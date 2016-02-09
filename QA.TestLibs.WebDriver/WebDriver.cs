namespace QA.TestLibs.WebDriver
{
    using OpenQA.Selenium;
    using OpenQA.Selenium.Interactions;
    using OpenQA.Selenium.Support.UI;
    using System;
    using System.Drawing;
    using System.Linq;

    public class WebDriver
    {
        public WebDriverConfig Config { get; protected set; }
        private IWebDriver _driver;
        private JavaScriptExecutor _javaScriptExecutor;
        private WebDriverWait _wait;

        public WebDriver(WebDriverConfig webDriverConfig)
        {
            Config = webDriverConfig;
            _driver = webDriverConfig.CreateDriver();
            _javaScriptExecutor = new JavaScriptExecutor(_driver);
            _wait = new WebDriverWait(_driver, webDriverConfig.WaitTimeout);
        }

        public IWebElement FindElement(By by)
        {
            return _driver.FindElement(by);
        }
    }
}
