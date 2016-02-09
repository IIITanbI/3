namespace QA.TestLibs.WebDriver
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Commands;
    using OpenQA.Selenium;
    using System.Diagnostics;
    using System.Threading;

    [CommandManager("WebCommand",
        Description = "Manager for WebCommands")]
    public class WebCommandManager
    {
        private ThreadLocal<Stopwatch> _sw = new ThreadLocal<Stopwatch>(() => new Stopwatch());

        public void HightLightElement(WebDriver driver, string xPath, ILogger log)
        {

        }

        public IWebElement Find(WebDriver driver, WebElement element, ILogger log)
        {
            log?.DEBUG($"Start searching element: {element.Name}");
            log?.TRACE($"{element}");
            try
            {
                _sw.Value.Reset();
                var el = driver.FindElement(element.Locator.Get());
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

        [Command("Wait until element is visible")]
        public void WaitUntilElementIsVisible(WebDriver driver, IWebElement element, ILogger log)
        {
            log?.DEBUG("Wait until element is visible");
            try
            {
                _sw.Value.Reset();
                driver.WaitUntilElementIsVisible(element);
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
        public void WaitUntilElementIsEnabled(WebDriver driver, IWebElement element, ILogger log)
        {
            log?.DEBUG("Wait until element is enabled");
            try
            {
                _sw.Value.Reset();
                driver.WaitUntilElementIsVisible(element);
                _sw.Value.Stop();
                log?.DEBUG($"Waitig for enabling has been completed. Time: {_sw.Value.ElapsedMilliseconds} ms");
            }
            catch (Exception ex)
            {
                log?.ERROR("Waitig for enabling has been completed with exception");
                throw new CommandAbortException("Waitig for enabling has been completed with exception", ex);
            }
        }

        [Command("Click", Description = "Click to element")]
        public void Click(WebDriver driver, WebElement element, ILogger log)
        {
            var el = Find(driver, element, log);

            WaitUntilElementIsVisible(driver, el, log);
            WaitUntilElementIsEnabled(driver, el, log);

            try
            {
                log?.INFO($"Click on element: {element.Name}");
                el.Click();
                log?.INFO("Click completed");
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occured during clicking on element: {element.Name}");
                throw new CommandAbortException($"Error occured during clicking on element: {element.Name}", ex);
            }
        }
    }
}
