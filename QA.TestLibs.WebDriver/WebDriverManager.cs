namespace QA.TestLibs.WebDriver
{
    using System;
    using Commands;
    using OpenQA.Selenium;
    using System.Diagnostics;
    using System.Threading;
    using OpenQA.Selenium.Support.UI;
    using Exceptions;

    [CommandManager("WebCommand", Description = "Manager for WebCommands")]
    public partial class WebDriverManager
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
                cont.Driver = Config.CreateDriver();
                cont.Wait = new WebDriverWait(cont.Driver, Config.WaitTimeout);
                cont.JavaScriptExecutor = cont.Driver as IJavaScriptExecutor;
                if (cont.JavaScriptExecutor == null && Config.IsJavaScriptEnabled)
                {
                    throw new TestLibsException($"Can't initialize JavaScript executor for WebDriver: {cont.Driver}");
                }
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
                var el = _container.Value.Driver.FindElement(element.Locator.Get());
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

        [Command("Click", Description = "Click on element")]
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

        [Command("Send keys", Description = "Send keys to element")]
        public void SendKeys(WebElement element, string value, ILogger log)
        {
            var el = Find(element, log);

            WaitUntilElementIsVisible(el, log);
            WaitUntilElementIsEnabled(el, log);

            try
            {
                log?.INFO($"Send keys '{value}' to element: {element.Name}");
                el.Click();
                el.Clear();
                el.SendKeys(value);
                log?.INFO($"Send keys '{value}' completed");
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during keys '{value}' sending to element: {element.Name}");
                throw new CommandAbortException($"Error occurred keys '{value}' sending to element: {element.Name}", ex);
            }
        }

        [Command("Switch to frame")]
        public void SwitchToFrame(string frame, ILogger log)
        {
            try
            {
                log?.DEBUG($"Switch to frame with name/ID: {frame}");
                _container.Value.Driver.SwitchTo().Frame(frame);
                log?.DEBUG($"Switching to frame completed");
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during switching to frame: {frame}");
                throw new CommandAbortException($"Error occurred during switching to frame: {frame}", ex);
            }
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
                WaitUntilElementIsEnabled(element);
                _sw.Value.Stop();
                log?.DEBUG($"Waiting for enabling has been completed. Time: {_sw.Value.ElapsedMilliseconds} ms");
            }
            catch (Exception ex)
            {
                log?.ERROR("Waiting for enabling has been completed with exception");
                throw new CommandAbortException("Waiting for enabling has been completed with exception", ex);
            }
        }

        public Screenshot TakeScreenshot(ILogger log)
        {
            _container.Value.Driver.SwitchTo().Window(_container.Value.Driver.CurrentWindowHandle);
            Screenshot screenshot = ((ITakesScreenshot)_container.Value.Driver).GetScreenshot();
            return screenshot;
        }

        public void SaveScreenshot(Screenshot screenshot, string path)
        {
            screenshot.SaveAsFile(path, System.Drawing.Imaging.ImageFormat.Png);
        }

        public void WaitUntilElementIsVisible(By by)
        {
            _container.Value.Wait.Until(driver => driver.FindElement(by)?.Displayed);
        }

        public void WaitUntilElementIsVisible(IWebElement element)
        {
            _container.Value.Wait.Until(driver => element.Displayed);
        }

        public void WaitUntilElementIsEnabled(By by)
        {
            _container.Value.Wait.Until(driver => driver.FindElement(by)?.Enabled);
        }

        public void WaitUntilElementIsEnabled(IWebElement element)
        {
            _container.Value.Wait.Until(driver => element.Enabled);
        }

        private class LocalContainer
        {
            public IWebDriver Driver;
            public IJavaScriptExecutor JavaScriptExecutor;
            public WebDriverWait Wait;
        }
    }
}
