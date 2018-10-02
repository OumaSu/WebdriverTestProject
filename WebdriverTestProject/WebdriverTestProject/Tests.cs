using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using WebdriverTestProject.Helpers;

namespace WebdriverTestProject
{
    public class Tests
    {
        [SetUp]
        public void SetUp()
        {
            JsLogger.Reset();
        }

        [TearDown]
        public void TearDown()
        {
            var passed = TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Passed;
            try
            {
                ((IJavaScriptExecutor) WebDriver.Driver).ExecuteScript(
                    "tb:test-result=" + (passed ? "passed" : "failed"));
            }
            finally
            {
                WebDriver.Quit();
            }
        }

        [Test]
        public void FirstTest()
        {
            //driver.Navigate().GoToUrl("https://www.google.com/");

            //Assert.AreEqual("SW Test Academy - Software Test Academy", driver.Title);

            //driver.Close();

            //driver.Quit();
        }
    }
}