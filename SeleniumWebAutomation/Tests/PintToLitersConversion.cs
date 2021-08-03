using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

using Xunit;

namespace SeleniumWebAutomation
{
    public class PintToLitersConversion
    {
        const uint decimals = 2;
        const string homepage = "https://www.metric-conversions.org/volume/uk-pints-to-liters.htm";

        [Theory]
        [InlineData("1.23", "0.70")]
        [InlineData("2", "1.1")]
        [InlineData("5.54", "3.1")]
        public void TestMatches(string inputPints, string expectedLiters) {
            string result = GetWebResult(inputPints);
            Assert.Matches(ResultCheckerRegex(expectedLiters), result);
        }
        
        [Theory]
        [InlineData("0", "100")]
        public void TestNonmatches(string inputPints, string wrongLiters) {
            string result = GetWebResult(inputPints);
            Assert.DoesNotMatch(ResultCheckerRegex(wrongLiters), result);
        }

        #region impl.
        private const string inputCssId = "argumentConv";
        private const string resultCssId = "answer";

        private static string GetWebResult(string inputPints) {
            using IWebDriver driver = new FirefoxDriver();
            driver.Navigate().GoToUrl(homepage);
            driver.FindElement(By.Id(inputCssId)).SendKeys(inputPints);
            SelectComboValue(driver, "format", "0");
            SelectComboValue(driver, "sigfig", decimals.ToString());
            new WebDriverWait(
                    driver, TimeSpan.FromSeconds(SeleniumConstants.WaitTimeoutSeconds))
                .Until(webDriver => webDriver.FindElement(By.Id(resultCssId)).Displayed);
            var result = driver.FindElement(By.Id(resultCssId)).GetAttribute("textContent");
            return result;
        }

        private static string ResultCheckerRegex(string expected)
            => @"=\s*" + expected + @"L$";

        private static void SelectComboValue(
            IWebDriver driver, string comboId, string valueToSelect
        ) {
            var combo = new SelectElement(driver.FindElement(By.Id(comboId)));
            combo.SelectByValue(valueToSelect);
        }
        #endregion impl.
    }
}