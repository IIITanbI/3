namespace QA.TestLibs.WebDriver
{
    using Commands;
    using System;
    using OpenQA.Selenium.Interactions;

    public partial class WebDriverManager
    {
        [Command("Actions move to", Description = "Actions move to element")]
        public void ActionsMoveTo(WebElement element, ILogger log)
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

        [Command("Actions click", Description = "Actions click to element")]
        public void ActionsClick(WebElement element, ILogger log)
        {
            var el = Find(element, log);

            WaitUntilElementIsVisible(el, log);
            WaitUntilElementIsEnabled(el, log);

            try
            {
                log?.INFO($"Actions click on element: {element.Name}");
                new Actions(_container.Value.Driver).Click(el).Build().Perform();
                log?.INFO("Actions click completed");
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during actions clicking on element: {element.Name}");
                throw new CommandAbortException($"Error occurred during actions clicking on element: {element.Name}", ex);
            }
        }

        [Command("Actions right click", Description = "Actions right click to element")]
        public void ActionsRightClick(WebElement element, ILogger log)
        {
            var el = Find(element, log);

            WaitUntilElementIsVisible(el, log);
            WaitUntilElementIsEnabled(el, log);

            try
            {
                log?.INFO($"Actions right click on element: {element.Name}");
                new Actions(_container.Value.Driver).ContextClick(el).Build().Perform();
                log?.INFO("Actions right click completed");
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during actions right-clicking on element: {element.Name}");
                throw new CommandAbortException($"Error occurred during actions right-clicking on element: {element.Name}", ex);
            }
        }

        [Command("Actions double click", Description = "Actions double click to element")]
        public void ActionsDoubleClick(WebElement element, ILogger log)
        {
            var el = Find(element, log);

            WaitUntilElementIsVisible(el, log);
            WaitUntilElementIsEnabled(el, log);

            try
            {
                log?.INFO($"Actions double click on element: {element.Name}");
                new Actions(_container.Value.Driver).DoubleClick(el).Build().Perform();
                log?.INFO("Actions double click completed");
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during actions double-clicking on element: {element.Name}");
                throw new CommandAbortException($"Error occurred actions during-clicking on element: {element.Name}", ex);
            }
        }

        [Command("Actions send keys", Description = "Actions send keys to element")]
        public void ActionsSendKeys(WebElement element, string value, ILogger log)
        {
            var el = Find(element, log);

            WaitUntilElementIsVisible(el, log);
            WaitUntilElementIsEnabled(el, log);

            try
            {
                log?.INFO($"Actions send keys to element: {element.Name}");
                new Actions(_container.Value.Driver).SendKeys(el, value).Build().Perform();
                log?.INFO("Actions send keys completed");
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during actions keys sending to element: {element.Name}");
                throw new CommandAbortException($"Error occurred actions keys sending to element: {element.Name}", ex);
            }
        }
    }
}
