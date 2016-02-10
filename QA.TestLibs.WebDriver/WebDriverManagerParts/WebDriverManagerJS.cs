namespace QA.TestLibs.WebDriver
{
    using Commands;
    using OpenQA.Selenium;
    using System;
    using ExtensionMethods;
    using System.Linq;

    public partial class WebDriverManager
    {
        public void WaitForPageToLoad(ILogger log)
        {
            _container.Value.Wait.Until(d => Equals(ObjectJSExecutor("return document.readyState;", log).ToString().ToLower(), "complete"));
        }

        [Command("JS Click", Description = "JS Click to element")]
        public void JSClick(WebElement webElement, ILogger log)
        {
            string jsScript = "var evObj = document.createEvent('MouseEvents');evObj.initMouseEvent('click',true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);arguments[0].dispatchEvent(evObj);";
            JSExecutor(jsScript, webElement, log);
        }

        [Command("JS Double click", Description = "JS Double click to element")]
        public void JSDoubleClick(WebElement webElement, ILogger log)
        {
            string jsScript = "var evObj = document.createEvent('MouseEvents');evObj.initMouseEvent(\"dblclick\",true, false, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);arguments[0].dispatchEvent(evObj);";
            JSExecutor(jsScript, webElement, log);
        }

        [Command("JS Right click", Description = "JS Right click to element")]
        public void JSContextMenu(WebElement webElement, ILogger log)
        {
            string jsScript = "var evObj = document.createEvent('MouseEvents');evObj.initMouseEvent(\"contextmenu\",true, false, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);arguments[0].dispatchEvent(evObj);";
            JSExecutor(jsScript, webElement, log);
        }

        [Command("JS Move to", Description = "JS Move to element")]
        public void JSMouseOver(WebElement webElement, ILogger log)
        {
            string jsScript = "var evObj = document.createEvent('MouseEvents');evObj.initMouseEvent(\"mouseover\",true, false, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);arguments[0].dispatchEvent(evObj);";
            JSExecutor(jsScript, webElement, log);
        }

        [Command("JS Show", Description = "JS Show element")]
        public void JSShow(WebElement webElement, ILogger log)
        {
            JSExecutor("arguments[0].style.display = block;",  webElement, log);
        }

        [Command("JS Hide", Description = "JS Hide element")]
        public void JSHide(WebElement webElement, ILogger log)
        {
            JSExecutor("arguments[0].style.display = none;", webElement, log);
        }

        [Command("JS Scroll Into View", Description = "JS Scroll Into View to element")]
        public void JSScrollIntoView(WebElement webElement, ILogger log)
        {
            JSExecutor($"arguments[0].scrollIntoView(true);", webElement, log);
        }

        [Command("JS Scroll To", Description = "JS Scroll To element")]
        public void JSScrollTo(WebElement webElement, ILogger log)
        {
            var elem = Find(webElement, log);
            JSExecutor($"window.scrollTo({elem.Location.X}, {elem.Location.Y})", log);
        }

        [Command("JS Scroll To Bottom", Description = "JS Scroll To Bottom")]
        public void JSScrollToBottom(ILogger log)
        {
            JSExecutor($"window.scrollTo(0, document.body.scrollHeight)", log);
        }

        [Command("JS HightLight", Description = "JS HightLight Element")]
        public void HightLightElement(IWebElement element, ILogger log)
        {
            string highlightJavascript = @"arguments[0].style.cssText = ""border-width: 2px; border-style: solid; border-color: red"";";
            JSExecutor(highlightJavascript, new object[] { element }, log);
        }

        [Command("JS UnHightLight", Description = "JS UnHightLight Element")]
        public void UnHightLightElement(IWebElement element, ILogger log)
        {
            string highlightJavascript = @"arguments[0].style.cssText = ""border-width: 0px; border-style: solid; border-color: red"";";
            JSExecutor(highlightJavascript, new object[] { element }, log);
        }

        public void JSExecutor(string jsScript, ILogger log)
        {
            try
            {
                log?.INFO($"Execute javascript");
                _container.Value.JavaScriptExecutor.ExecuteScript(jsScript);
                log?.INFO("Javascript executing completed");
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during javascript execution");
                throw new CommandAbortException($"Error occurred during javascript execution:\n{jsScript}", ex);
            }
        }

        public object ObjectJSExecutor(string jsScript, ILogger log)
        {
            try
            {
                log?.INFO($"Execute javascript: {jsScript}");
                return _container.Value.JavaScriptExecutor.ExecuteScript(jsScript);
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during execution: {jsScript}");
                throw new CommandAbortException($"Error occurred during javascript execution:\n{jsScript}", ex);
            }
        }

        public void JSExecutor(string jsScript, WebElement element, ILogger log)
        {
            var el = Find(element, log);

            WaitUntilElementIsVisible(el, log);
            WaitUntilElementIsEnabled(el, log);

            try
            {
                log?.INFO($"Execute javascript: {jsScript}");
                _container.Value.JavaScriptExecutor.ExecuteScript(jsScript, el);
                log?.INFO("Javascript executing completed");
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during execution: {jsScript}");
                throw new CommandAbortException($"Error occurred during javascript execution:\n{jsScript}\nFor element: {element}", ex);
            }
        }

        public void JSExecutor(string jsScript, object[] args, ILogger log)
        {
            try
            {
                log?.INFO($"Execute javascript: {jsScript}");
                _container.Value.JavaScriptExecutor.ExecuteScript(jsScript, args);
                log?.INFO("Javascript executing completed");
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during execution: {jsScript}");
                throw new CommandAbortException($"Error occurred during javascript execution:\n{jsScript}\nWith arguments: {args.ToList().ToStringWithList()}", ex);
            }
        }
    }
}
