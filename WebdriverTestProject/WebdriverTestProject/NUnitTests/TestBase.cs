using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using WebdriverTestProject.Helpers;
using WebdriverTestProject.WebDriverCore;

namespace WebdriverTestProject.NUnitTests
{
    public class TestBase
    {
        [SetUp]
        public virtual void SetUp()
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
    }
}