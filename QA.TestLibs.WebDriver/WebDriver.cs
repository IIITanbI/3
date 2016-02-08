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
        private IWebDriver _driver;
        private JavaScriptExecutor _javaScriptExecutor;
        private WebDriverWait _wait; 

        public WebDriver(WebDriverConfig webDriverConfig)
        {
            _driver = webDriverConfig.CreateDriver();
            _javaScriptExecutor = new JavaScriptExecutor(_driver);
            _wait = new WebDriverWait(_driver, webDriverConfig.WaitTimeout);
        }

        public void WindowMaximize()
        {
            _driver.Manage().Window.Maximize();
        }

        public void SetWindowSize(int width, int height)
        {
            _driver.Manage().Window.Size = new Size(width, height);
        }

        public void SetWindowSize(Size size)
        {
            _driver.Manage().Window.Size = size;
        }

        public Screenshot TakeScreenshot()
        {
            _driver.SwitchTo().Window(_driver.CurrentWindowHandle);
            Screenshot screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
            return screenshot;
        }

        public void SaveScreenshot(Screenshot screenshot, string path)
        {
            screenshot.SaveAsFile(path, System.Drawing.Imaging.ImageFormat.Png);
        }

        public void AcceptAlert()
        {
            IAlert alert = _driver.SwitchTo().Alert();
            alert.Accept();
        }

        public void DismissAlert()
        {
            IAlert alert = _driver.SwitchTo().Alert();
            alert.Dismiss();
        }

        public void SwitchToTab(string tabName)
        {
            _driver.SwitchTo().Window(tabName);
        }

        public void SwitchToNewTab()
        {
            _driver.SwitchTo().Window(_driver.WindowHandles.Last());
        }

        public void Click(WebElement webElement)
        {
            new Actions(_driver).MoveToElement(webElement).Click().Build().Perform();
        }

        public void DoubleClick(WebElement webElement)
        {
            new Actions(_driver).MoveToElement(webElement).DoubleClick().Build().Perform();
        }

        public void RightClick(WebElement webElement)
        {
            new Actions(_driver).MoveToElement(webElement).ContextClick().Build().Perform();
        }

        public void ClickRightSide(WebElement webElement)
        {
            new Actions(_driver).MoveToElement(webElement, webElement.Location.X - 1, webElement.Location.Y / 2).Click().Build().Perform();
        }

        public void MoveTo(WebElement webElement)
        {
            new Actions(_driver).MoveToElement(webElement).Build().Perform();
        }

        public void WaitForPageToLoad()
        {
            _wait.Until(d => Equals(_javaScriptExecutor.ObjectJSExecutor("return document.readyState").ToString().ToLower(), "complete"));
        }

    }
}
