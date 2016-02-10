namespace QA.TestLibs.WebDriver
{
    using Commands;
    using OpenQA.Selenium;
    using System;
    using System.Linq;
    using System.Drawing;

    public partial class WebDriverManager
    {
        public void Navigate(string url, ILogger log)
        {
            try
            {
                log?.INFO($"Start URL navigating: {url}");
                _container.Value.Driver.Navigate().GoToUrl(url);
                log?.INFO("URL navigating completed");
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during URL {url} navigating");
                throw new CommandAbortException($"Error occurred during URL {url} navigating", ex);
            }
        }

        public void Close(ILogger log)
        {
            try
            {
                log?.INFO($"Start driver closing");
                _container.Value.Driver.Close();
                log?.INFO("Driver closing completed");
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during driver closing");
                throw new CommandAbortException($"Error occurred during driver closing", ex);
            }
        }

        public void Quit(ILogger log)
        {
            try
            {
                log?.INFO($"Start driver quitting");
                _container.Value.Driver.Quit();
                log?.INFO("Driver quitting completed");
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during driver quitting");
                throw new CommandAbortException($"Error occurred during driver quitting", ex);
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
    }
}
