using OpenQA.Selenium;
using WebdriverTestProject.Controls.Yandexs;

namespace WebdriverTestProject.Pages.Yandexs
{
    public class MainPage : PageBase
    {
        private readonly int defaultTimeout = 5000;
        public MoreControl MoreControl=> new MoreControl(By.XPath("//*[@href='https://www.yandex.ru/all']"));
        public override void BrowseWaitVisible()
        {
            MoreControl.WaitVisibleWithRetries(defaultTimeout);
        }
    }
}
