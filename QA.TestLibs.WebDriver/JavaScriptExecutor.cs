namespace QA.TestLibs.WebDriver
{
    using OpenQA.Selenium;
    using QA.TestLibs.Exceptions;
    using System;
    public class JavaScriptExecutor
    {
        private IJavaScriptExecutor _javaScriptExecutor;

        public JavaScriptExecutor(IWebDriver webDriver)
        {
            _javaScriptExecutor = webDriver as IJavaScriptExecutor;
            if (_javaScriptExecutor == null)
            {
                throw new TestLibsException($"Can't init javascript executor for webdriver: {webDriver}");
            }
        }

        public void Click(WebElement webElement)
        {
            webElement.Click();
        }

        public void JSClick(WebElement webElement)
        {
            string jsScript = "var evObj = document.createEvent('MouseEvents'); evObj.initMouseEvent('click',true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null); arguments[0].dispatchEvent(evObj);";
            JSExecutor(jsScript, webElement);
        }

        public void JSScrollIntoView(WebElement webElement)
        {
            JSExecutor($"arguments[0].scrollIntoView(true);", webElement);
        }

        public void JSScrollTo(WebElement webElement)
        {
            JSExecutor($"window.scrollTo({webElement.Location.X}, {webElement.Location.Y})");
        }

        public void JSExecutor(string jsScript)
        {
            try
            {
                _javaScriptExecutor.ExecuteScript(jsScript);
            }
            catch (Exception e)
            {
                string.Format("Error occurred during execution javascript:\n%s\nError message: %s", jsScript, e.Message);
                throw;
            }
        }

        public void JSExecutor(string jsScript, WebElement webElement)
        {
            try
            {
                _javaScriptExecutor.ExecuteScript(jsScript, webElement);
            }
            catch (Exception e)
            {
                string.Format("Error occurred during execution javascript:\n%s\nError message: %s", jsScript, e.Message);
                throw;
            }
        }

        public void JSExecutor(string jsScript, object[] args)
        {
            try
            {
                _javaScriptExecutor.ExecuteScript(jsScript, args);
            }
            catch (Exception e)
            {
                string.Format("Error occurred during execution javascript:\n%s\nError message: %s", jsScript, e.Message);
                throw;
            }
        }
    }
}
