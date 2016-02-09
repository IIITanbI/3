namespace QA.TestLibs.WebDriver
{
    using System;
    using Commands;
    using OpenQA.Selenium;
    using System.Diagnostics;
    using System.Threading;
    using OpenQA.Selenium.Support.UI;
    using System.Drawing;
    using System.Linq;
    using OpenQA.Selenium.Interactions;
    [CommandManager("WebCommand",
        Description = "Manager for WebCommands")]
    public class WebDriverManager
    {
        public WebDriverConfig Config { get; protected set; }
        private ThreadLocal<Stopwatch> _sw = new ThreadLocal<Stopwatch>(() => new Stopwatch());
        ThreadLocal<LocalContainer> _container;

        public WebDriverManager(WebDriverConfig config)
        {
            Config = config;
            _container = new ThreadLocal<LocalContainer>(() =>
            {
                var cont = new LocalContainer();
                cont._driver = Config.CreateDriver();
                cont._javaScriptExecutor = new JavaScriptExecutor(cont._driver);
                cont._wait = new WebDriverWait(cont._driver, Config.WaitTimeout);
                return cont;
            });
        }

        public IWebElement Find(WebElement element, ILogger log)
        {
            log?.DEBUG($"Start searching element: {element.Name}");
            log?.TRACE($"{element}");
            try
            {
                _sw.Value.Reset();
                var el = _container.Value._driver.FindElement(element.Locator.Get());
                _sw.Value.Stop();
                log?.DEBUG($"Element: {element.Name} has been found. Time: {_sw.Value.ElapsedMilliseconds} ms");
                return el;
            }
            catch (Exception ex)
            {
                log?.ERROR("Couldn't find element");
                throw new CommandAbortException("Couldn't find element", ex);
            }
        }

        [Command("Set window size by size", Description = "Set window size using parameter {size}")]
        public void SetWindowSize(Size size)
        {
            _container.Value._driver.Manage().Window.Size = size;
        }

        [Command("Set window size by width and height", Description = "Set window size using parameter {width} {height}")]
        public void SetWindowSize(int width, int height)
        {
            _container.Value._driver.Manage().Window.Size = new Size(width, height);
        }

        [Command("Maximize window")]
        public void WindowMaximize()
        {
            _container.Value._driver.Manage().Window.Maximize();
        }

        [Command("Accept alert")]
        public void AcceptAlert()
        {
            IAlert alert = _container.Value._driver.SwitchTo().Alert();
            alert.Accept();
        }

        [Command("Dismiss alert")]
        public void DismissAlert()
        {
            IAlert alert = _container.Value._driver.SwitchTo().Alert();
            alert.Dismiss();
        }

        [Command("Switch to new tab")]
        public void SwitchToNewTab()
        {
            _container.Value._driver.SwitchTo().Window(_container.Value._driver.WindowHandles.Last());
        }

        [Command("Wait until element is visible")]
        public void WaitUntilElementIsVisible(IWebElement element, ILogger log)
        {
            log?.DEBUG("Wait until element is visible");
            try
            {
                _sw.Value.Reset();
                WaitUntilElementIsVisible(element);
                _sw.Value.Stop();
                log?.DEBUG($"Waiting for visibility has been completed. Time: {_sw.Value.ElapsedMilliseconds} ms");
            }
            catch (Exception ex)
            {
                log?.ERROR("Waiting for visibility has been completed with exception");
                throw new CommandAbortException("Waiting for visibility has been completed with exception", ex);
            }
        }

        [Command("Wait until element is enabled")]
        public void WaitUntilElementIsEnabled(IWebElement element, ILogger log)
        {
            log?.DEBUG("Wait until element is enabled");
            try
            {
                _sw.Value.Reset();
                WaitUntilElementIsVisible(element);
                _sw.Value.Stop();
                log?.DEBUG($"Waiting for enabling has been completed. Time: {_sw.Value.ElapsedMilliseconds} ms");
            }
            catch (Exception ex)
            {
                log?.ERROR("Waiting for enabling has been completed with exception");
                throw new CommandAbortException("Waiting for enabling has been completed with exception", ex);
            }
        }

        [Command("Move to", Description = "Move to element")]
        public void MoveTo(WebElement element, ILogger log)
        {
            var el = Find(element, log);

            WaitUntilElementIsVisible(el, log);
            WaitUntilElementIsEnabled(el, log);

            try
            {
                log?.INFO($"Move to element: {element.Name}");
                new Actions(_container.Value._driver).MoveToElement(el).Build().Perform();
                log?.INFO("Move to completed");
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred moving to element: {element.Name}");
                throw new CommandAbortException($"Error occurred moving to element: {element.Name}", ex);
            }
        }

        [Command("Click", Description = "Click to element")]
        public void Click(WebElement element, ILogger log)
        {
            var el = Find(element, log);

            WaitUntilElementIsVisible(el, log);
            WaitUntilElementIsEnabled(el, log);

            try
            {
                log?.INFO($"Click on element: {element.Name}");
                el.Click();
                log?.INFO("Click completed");
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during clicking on element: {element.Name}");
                throw new CommandAbortException($"Error occurred during clicking on element: {element.Name}", ex);
            }
        }

        [Command("Right click", Description = "Right click to element")]
        public void RightClick(WebElement element, ILogger log)
        {
            var el = Find(element, log);

            WaitUntilElementIsVisible(el, log);
            WaitUntilElementIsEnabled(el, log);

            try
            {
                log?.INFO($"Right click on element: {element.Name}");
                new Actions(_container.Value._driver).MoveToElement(el).ContextClick().Build().Perform();
                log?.INFO("Right click completed");
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during right-clicking on element: {element.Name}");
                throw new CommandAbortException($"Error occurred during right-clicking on element: {element.Name}", ex);
            }
        }

        [Command("Double click", Description = "Double click to element")]
        public void DoubleClick(WebElement element, ILogger log)
        {
            var el = Find(element, log);

            WaitUntilElementIsVisible(el, log);
            WaitUntilElementIsEnabled(el, log);

            try
            {
                log?.INFO($"Double click on element: {element.Name}");
                new Actions(_container.Value._driver).MoveToElement(el).DoubleClick().Build().Perform();
                log?.INFO("Double click completed");
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during double-clicking on element: {element.Name}");
                throw new CommandAbortException($"Error occurred during-clicking on element: {element.Name}", ex);
            }
        }

        public void WaitForPageToLoad()
        {
            _container.Value._wait.Until(d => Equals(_container.Value._javaScriptExecutor.ObjectJSExecutor("return document.readyState").ToString().ToLower(), "complete"));
        }

        public Screenshot TakeScreenshot(ILogger log)
        {
            _container.Value._driver.SwitchTo().Window(_container.Value._driver.CurrentWindowHandle);
            Screenshot screenshot = ((ITakesScreenshot)_container.Value._driver).GetScreenshot();
            return screenshot;
        }

        public void SaveScreenshot(Screenshot screenshot, string path)
        {
            screenshot.SaveAsFile(path, System.Drawing.Imaging.ImageFormat.Png);
        }

        public void HightLightElement(IWebElement element)
        {
            string highlightJavascript = @"arguments[0].style.cssText = ""border-width: 2px; border-style: solid; border-color: red"";";
            _container.Value._javaScriptExecutor.JSExecutor(highlightJavascript, new object[] { element });
        }

        public void UnHightLightElement(IWebElement element)
        {
            string highlightJavascript = @"arguments[0].style.cssText = ""border-width: 0px; border-style: solid; border-color: red"";";
            _container.Value._javaScriptExecutor.JSExecutor(highlightJavascript, new object[] { element });
        }

        public void WaitUntilElementIsVisible(By by)
        {
            _container.Value._wait.Until(driver => driver.FindElement(by)?.Displayed);
        }

        public void WaitUntilElementIsVisible(IWebElement element)
        {
            _container.Value._wait.Until(driver => element.Displayed);
        }

        public void WaitUntilElementIsEnabled(By by)
        {
            _container.Value._wait.Until(driver => driver.FindElement(by)?.Enabled);
        }

        public void WaitUntilElementIsEnabled(IWebElement element)
        {
            _container.Value._wait.Until(driver => element.Enabled);
        }

        private class LocalContainer
        {
            public IWebDriver _driver;
            public JavaScriptExecutor _javaScriptExecutor;
            public WebDriverWait _wait;
        }
    }
}
