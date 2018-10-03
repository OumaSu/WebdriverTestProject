using System;
using WebdriverTestProject.WebDriverCore;

namespace WebdriverTestProject.Helpers
{
    public static class JsLogger
    {
        private static string log;

        public static void Complete()
        {
            log = log + Read();
        }

        public static void Reset()
        {
            log = "";
        }

        public static void Show()
        {
            Console.Out.WriteLine("JavaScript log START\n");
            Console.Out.WriteLine(log);
            Console.Out.WriteLine("JavaScript log END");
            Reset();
        }

        public static void CaptureJavascriptErrors()
        {
            var str =
                WebDriver.ExecuteScript("return window.jsErrors") as string;
            if (string.IsNullOrEmpty(str))
                return;
            Console.WriteLine("Javascript errors:\n" + str);
        }


        private static string Read()
        {
            return WebDriver.ExecuteScript("return Logger.read() in browser {0}", "ChromeBrowser", new object[0]) as string;
        }
    }
}