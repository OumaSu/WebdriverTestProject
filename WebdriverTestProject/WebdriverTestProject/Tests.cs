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
            WebDriver.MaximizeWindow();
        }

        [TearDown]
        public void TearDown()
        {
            var passed = TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Passed;
            try
            {
                ((IJavaScriptExecutor) WebDriver.Driver).ExecuteScript(
                    "tb:test-result=" + (passed ? "passed" : "failed"));
                //if (!passed) пока уберу, нужно подумать над необходимостью
                //{
                //    JsLogger.CaptureJavascriptErrors();
                //    JsLogger.Show();
                //}
            }
            finally
            {
                WebDriver.Quit();
            }
        }

        [Test]
        public void TestYandex()
        {
            WebDriver.Driver.Navigate().GoToUrl("https://www.yandex.ru/");
        }

        [Test]
        public void TestMail()
        {
            WebDriver.Driver.Navigate().GoToUrl("https://www.yandex.ru/");
        }

    }
}