using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace WebdriverTestProject
{
    public static class WebDriver
    {
        public static readonly IWebDriver Driver;

        static WebDriver()
        {
            Driver = new ChromeDriver();
        }

        public static void MaximizeWindow()
        {
            Driver.Manage().Window.Maximize();
        }

        public static void NavigateToUrl(string url)
        {
            Driver.Navigate().GoToUrl(url);
        }

        public static object ExecuteScript(string script, params object[] args)
        {
            return ((IJavaScriptExecutor) Driver).ExecuteScript(script, args);
        }


        public static void Refresh()
        {
            Driver.Navigate().Refresh();
        }

        public static ISearchContext GetSearchContext()
        {
            return Driver;
        }


        public static void Back()
        {
            Driver.Navigate().Back();
        }

        public static void Quit()
        {
            try
            {
                try
                {
                    Driver.Close();
                }
                finally
                {
                    try
                    {
                        Driver.Quit();
                    }
                    finally
                    {
                        Driver.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при остановке Driver:\r\n{0}", ex);
            }
        }
    }
}