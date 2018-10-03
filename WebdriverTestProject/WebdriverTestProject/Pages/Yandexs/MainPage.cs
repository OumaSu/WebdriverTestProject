using OpenQA.Selenium;
using WebdriverTestProject.Controls.Yandexs;

namespace WebdriverTestProject.Pages.Yandexs
{
    public class MainPage : PageBase
    {
        public MoreControl MoreControl => new MoreControl(By.XPath("//*[@href='https://www.yandex.ru/all']"));

        public SchedulePage GoToSchedule()
        {
            MoreControl.Click();
            MoreControl.Schedule.WaitVisibleWithRetries(defaultTimeout);
            MoreControl.Schedule.Click();
            return ChangePageType<SchedulePage>();
        }

        public override void BrowseWaitVisible()
        {
            MoreControl.WaitVisibleWithRetries(defaultTimeout);
        }
    }
}