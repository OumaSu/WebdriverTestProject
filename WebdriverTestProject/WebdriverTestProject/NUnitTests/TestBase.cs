using System;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
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
            Console.WriteLine($"TestResult : {passed}");
            WebDriver.Quit();
        }
    }
}