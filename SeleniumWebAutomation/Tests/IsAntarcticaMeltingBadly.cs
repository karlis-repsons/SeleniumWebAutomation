using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

using Xunit;

namespace SeleniumWebAutomation
{
    public class IsAntarcticaMeltingBadly
    {
        private const bool takeScreenshots = true;

        [Fact]
        public void No() {
            string url = "https://earth.nullschool.net/#current/wind/surface/level/overlay=temp/patterson=9.87,-24.54,237/loc=0.000,-89.990";
            string temperatureCelsius = GetPolarTemperatureCelsius(url);
            Assert.True(double.Parse(temperatureCelsius) <= 0);
        }

        #region impl.
        private static string GetPolarTemperatureCelsius(string homepage) {
            string resultCssSelector = @"[data-name=""spotlight-b""]>div";
            using IWebDriver driver = new FirefoxDriver();
            driver.Navigate().GoToUrl(homepage);
            new WebDriverWait(
                    driver, TimeSpan.FromSeconds(SeleniumConstants.WaitTimeoutSeconds))
                .Until(webDriver => webDriver.FindElement(
                    By.CssSelector(resultCssSelector)).Displayed);

            string result = driver.FindElement(By.CssSelector(resultCssSelector))
                            .GetAttribute("textContent");

            if (takeScreenshots) {
                var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                screenshot.SaveAsFile(
                    "south-pole-temperature-on-map.png", ScreenshotImageFormat.Png);
            }

            return result;
        }
        #endregion impl.
    }
}