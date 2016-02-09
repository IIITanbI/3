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

        [Command("Set window size by size", Description = "Set window size using parameter {size}")]
        public void SetWindowSize(Size size, ILogger log)
        {
            try
            {
                log?.INFO($"Resize window using size: {size.ToString()}");
                _container.Value.Driver.Manage().Window.Size = size;
                log?.INFO("Window resizing completed");
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during window resizing");
                throw new CommandAbortException($"Error occurred during window resizing", ex);
            }
        }

        [Command("Set window size by width and height", Description = "Set window size using parameter {width} {height}")]
        public void SetWindowSize(int width, int height, ILogger log)
        {
            try
            {
                log?.INFO($"Resize window using width: {width} and height: {height}");
                _container.Value.Driver.Manage().Window.Size = new Size(width, height);
                log?.INFO("Window resizing completed");
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during window resizing");
                throw new CommandAbortException($"Error occurred during window resizing", ex);
            }
        }

        [Command("Maximize window")]
        public void WindowMaximize(ILogger log)
        {
            try
            {
                log?.INFO($"Maximize window");
                _container.Value.Driver.Manage().Window.Maximize();
                log?.INFO("Window maximizing completed");
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during window maximizing");
                throw new CommandAbortException($"Error occurred during window maximizing", ex);
            }
        }

        [Command("Accept alert")]
        public void AcceptAlert(ILogger log)
        {
            try
            {
                log?.INFO($"Accept alert");
                IAlert alert = _container.Value.Driver.SwitchTo().Alert();
                alert.Accept();
                log?.INFO("Alert accepting completed");
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during alert accepting");
                throw new CommandAbortException($"Error occurred during alert accepting", ex);
            }
        }

        [Command("Dismiss alert")]
        public void DismissAlert(ILogger log)
        {
            try
            {
                log?.INFO($"Dismiss alert");
                IAlert alert = _container.Value.Driver.SwitchTo().Alert();
                alert.Dismiss();
                log?.INFO("Alert dismissing completed");
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during alert dismissing");
                throw new CommandAbortException($"Error occurred during alert dismissing", ex);
            }
        }

        [Command("Switch to new tab")]
        public void SwitchToNewTab(ILogger log)
        {
            try
            {
                log?.INFO($"Switch to new tab");
                _container.Value.Driver.SwitchTo().Window(_container.Value.Driver.WindowHandles.Last());
                log?.INFO("Switching to new tab completed");
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during tab switching");
                throw new CommandAbortException($"Error occurred during tab switching", ex);
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
                new Actions(_container.Value.Driver).MoveToElement(el).Build().Perform();
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
                new Actions(_container.Value.Driver).MoveToElement(el).ContextClick().Build().Perform();
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
                new Actions(_container.Value.Driver).MoveToElement(el).DoubleClick().Build().Perform();
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
            _container.Value.Wait.Until(d => Equals(_container.Value.JavaScriptExecutor.ObjectJSExecutor("return document.readyState").ToString().ToLower(), "complete"));
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

        public void HightLightElement(IWebElement element)
        {
            string highlightJavascript = @"arguments[0].style.cssText = ""border-width: 2px; border-style: solid; border-color: red"";";
            _container.Value.JavaScriptExecutor.JSExecutor(highlightJavascript, new object[] { element });
        }

        public void UnHightLightElement(IWebElement element)
        {
            string highlightJavascript = @"arguments[0].style.cssText = ""border-width: 0px; border-style: solid; border-color: red"";";
            _container.Value.JavaScriptExecutor.JSExecutor(highlightJavascript, new object[] { element });
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
