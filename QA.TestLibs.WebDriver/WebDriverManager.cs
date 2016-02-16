namespace QA.TestLibs.WebDriver
{
    using System;
    using Commands;
    using OpenQA.Selenium;
    using System.Diagnostics;
    using System.Threading;
    using OpenQA.Selenium.Support.UI;
    using Exceptions;
    using System.Collections.Generic;

    [CommandManager(typeof(WebDriverConfig), "WebDriver", Description = "Manager for WebDriver")]
    public partial class WebDriverManager : CommandManagerBase
    {
        public WebDriverConfig Config { get; protected set; }
        private ThreadLocal<Stopwatch> _sw = new ThreadLocal<Stopwatch>(() => new Stopwatch());
        ThreadLocal<LocalContainer> _container;

        public WebDriverManager(WebDriverConfig config)
            : base(config)
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
                _sw.Value.Start();

                var isDefaultContent = true;
                IWebElement targetElement = null;
                var parentStack = new Stack<WebElement>();

                for (var currentElement = element.ParentElement; currentElement != null; currentElement = currentElement.ParentElement)
                {
                    parentStack.Push(currentElement);
                }

                while (parentStack.Count != 0)
                {
                    var workElement = parentStack.Pop();

                    var frameElement = workElement as FrameWebElement;

                    if (frameElement != null)
                    {
                        SwitchToFrame(frameElement, log);
                        isDefaultContent = false;
                    }
                }

                if (element.Locator.IsRelative)
                {
                    for (var currentElement = element.ParentElement; currentElement != null && !(currentElement.Locator?.IsRelative ?? false); currentElement = currentElement.ParentElement)
                    {
                        var frameElement = currentElement as FrameWebElement;
                        if (frameElement == null)
                            parentStack.Push(currentElement);
                    }
                    if (parentStack.Count != 0)
                    {
                        var currentParent = parentStack.Pop();

                        log?.TRACE($"Start searching parent element: {currentParent.Name}");
                        log?.TRACE($"{currentParent}");
                        targetElement = _container.Value.Driver.FindElement(currentParent.Locator.Get());
                        log?.TRACE($"Parent element: {currentParent.Name} has been found");

                        while (parentStack.Count != 0)
                        {
                            currentParent = parentStack.Pop();
                            log?.TRACE($"Start searching target parent element: {currentParent.Name}");
                            log?.TRACE($"{currentParent}");
                            targetElement = targetElement.FindElement(currentParent.Locator.Get());
                            log?.TRACE($"Target parent element: {currentParent.Name} has been found");
                        }

                        log?.TRACE($"Start searching target element: {currentParent.Name}");
                        log?.TRACE($"{element}");
                        targetElement = targetElement.FindElement(element.Locator.Get());
                        log?.TRACE($"Target element: {element.Name} has been found");
                    }
                    else
                    {
                        log?.TRACE($"Start searching target parent element: {element.Name}");
                        log?.TRACE($"{element}");
                        targetElement = _container.Value.Driver.FindElement(element.Locator.Get());
                        log?.TRACE($"Target parent element: {element.Name} has been found");
                    }
                }
                else
                {
                    log?.TRACE($"Start searching target element: {element.Name}");
                    log?.TRACE($"{element}");
                    targetElement = _container.Value.Driver.FindElement(element.Locator.Get());
                    log?.TRACE($"Target element: {element.Name} has been found");
                }
                if (!isDefaultContent) SwitchToDefaultContent(log);

                _sw.Value.Stop();
                log?.INFO("Click completed");
                log?.TRACE($"Element: {element.Name} has been found. Time: {_sw.Value.ElapsedMilliseconds} ms");

                return targetElement;
            }
            catch (Exception ex)
            {
                log?.ERROR("Couldn't find element");
                throw new CommandAbortException($"Couldn't find element: {element.Name}", ex);
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

        [Command("Switch default content")]
        public void SwitchToDefaultContent(ILogger log)
        {
            try
            {
                log?.DEBUG($"Switch to default content");
                _container.Value.Driver.SwitchTo().DefaultContent();
                log?.DEBUG($"Switching to default content completed");
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during switching to default content");
                throw new CommandAbortException($"Error occurred during switching to default content", ex);
            }
        }

        [Command("Switch to frame by id")]
        public void SwitchToFrameById(string id, ILogger log)
        {
            try
            {
                log?.DEBUG($"Switch to frame");
                _container.Value.Driver.SwitchTo().Frame(id);
                log?.DEBUG($"Switching to frame completed");
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during switching to frame");
                throw new CommandAbortException($"Error occurred during switching to frame", ex);
            }
        }

        [Command("Switch to frame")]
        public void SwitchToFrame(FrameWebElement elem, ILogger log)
        {
            try
            {
                log?.DEBUG($"Switch to frame by locator: {elem}");

                var wElem = Find(elem, log);
                var frameId = wElem.GetAttribute("id");

                _container.Value.Driver.SwitchTo().Frame(frameId);
                log?.DEBUG($"Switching to frame completed");
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during switching to frame by locator: {elem}");
                throw new CommandAbortException($"Error occurred during switching to frame by locator: {elem}", ex);
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
