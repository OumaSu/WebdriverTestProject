using System;
using OpenQA.Selenium;

namespace WebdriverTestProject.Pages
{
    public abstract class PageBase
    {
        private readonly object alive = new object();

        static PageBase()
        {
        }


        public static TimeSpan WaitPageLoadTimeout => TimeSpan.FromSeconds(60.0);

        public string PageTitle => WebDriver.Driver.Title;

        public abstract void BrowseWaitVisible();


        public TPage ChangePageType<TPage>() where TPage : PageBase, new()
        {
            VerifyPageIsAlive();
            var instance = Activator.CreateInstance<TPage>();
            instance.BrowseWaitVisible();
            return instance;
        }

        public static TPage GoToUrl<TPage>(string url) where TPage : PageBase, new()
        {
            WebDriver.NavigateToUrl(url);

            var local0 = Activator.CreateInstance<TPage>();
            local0.BrowseWaitVisible();
            InitPage();
            return local0;
        }

        public string GetCurrentUrl() => WebDriver.Driver.Url;

        public static TPage RefreshPage<TPage>() where TPage : PageBase, new()
        {
            WebDriver.Refresh();
            var page = Activator.CreateInstance<TPage>();
            page.BrowseWaitVisible();
            InitPage();
            return page;
        }
        private static void InitPage()
        {
            try
            {
                WebDriver.ExecuteScript("$(document.body).addClass('testingMode')");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка выполнения скрипта: {0}", ex.Message);
            }
        }

        private void VerifyPageIsAlive()
        {
            if (alive == null)
                throw new InvalidOperationException("Данная страница уже закрыта");
        }

        public bool HasElementWithId(string idElement)
        {
            try
            {
                GetElementById(idElement);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public IWebElement GetElementById(string idElement) => WebDriver.Driver.FindElement(By.Id(idElement));

        public static void GetAlertMessageAndAccept()
        {
            try
            {
                var alert = WebDriver.Driver.SwitchTo().Alert();
                Console.WriteLine("Сообщение в alert: {0}", alert.Text);
                alert.Accept();
            }
            catch (Exception)
            {
                Console.WriteLine("Не было alert");
            }
        }
    }
}