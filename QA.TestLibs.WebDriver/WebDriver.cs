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

        public WebDriver(WebDriverConfig webDriverConfig)
        {
            _driver = webDriverConfig.CreateDriver();
            _javaScriptExecutor = new JavaScriptExecutor(_driver);
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

        public void TakeScreenshot(string screen)
        {
            Screenshot ss = ((ITakesScreenshot)_driver).GetScreenshot();
            ss.SaveAsFile(screen, System.Drawing.Imaging.ImageFormat.Png);
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

        public void ClickRightSide(WebElement webElement)
        {
            new Actions(_driver).MoveToElement(webElement, webElement.Location.X - 1, webElement.Location.Y / 2).Click().Build().Perform();
        }

        public void MoveTo(WebElement webElement)
        {
            new Actions(_driver).MoveToElement(webElement).Build().Perform();
        }

        public void WaitForPageToLoad(int waitTimeInSeconds)
        {
            var timeout = new TimeSpan(0, 0, waitTimeInSeconds);
            var wait = new WebDriverWait(_driver, timeout);
            var javascript = _driver as IJavaScriptExecutor;
            if (javascript == null)
            {
                throw new ArgumentException("driver", "Driver must support javascript execution");
            }
            wait.Until(d =>
            {
                try
                {
                    string readyState = javascript.ExecuteScript(
                        "if (document.readyState) return document.readyState;").ToString();
                    return readyState.ToLower() == "complete";
                }
                catch (InvalidOperationException e)
                {
                    return e.Message.ToLower().Contains("Window is no longer available. Unable to get browser");
                }
                catch (WebDriverException e)
                {
                    return e.Message.ToLower().Contains("Browser is no longer available. Unable to connect");
                }
                catch (Exception)
                {
                    return false;
                }
            });
        }
    }
}
