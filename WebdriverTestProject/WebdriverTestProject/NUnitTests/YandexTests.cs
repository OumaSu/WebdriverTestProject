using NUnit.Framework;
using WebdriverTestProject.Helpers;
using WebdriverTestProject.Pages.Yandexs;
using WebdriverTestProject.WebDriverCore;

namespace WebdriverTestProject.NUnitTests
{
    public class YandexTests : TestBase
    {
        private MainPage mainPage;
        public override void SetUp()
        {
            base.SetUp();
            WebDriver.Driver.Navigate().GoToUrl("https://www.yandex.ru/");
            mainPage = new MainPage();
        }
        [Test]
        public void TestYandex()
        {
            mainPage.BrowseWaitVisible();
        }
    }
}