using OpenQA.Selenium;
using System.Drawing;
using System;
using OpenQA.Selenium.Support.UI;

namespace QA.TestLibs.WebDriver
{
    public class WebDriver /*: IWebDriver*/
    {
        private IWebDriver _driver;

        public WebDriver() { }

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
    }
}
