using System;
using System.Linq;
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
            const string from = "Екатеринбург";
            const string to = "Каменск-Уральский";
            const string resultHeader =
                "Расписание транспорта и билеты на поезд, электричку и автобус из Екатеринбурга в Каменск-Уральский";
            var targetDate = DateTime.Today.GetNextDayOfWeek(DayOfWeek.Saturday);
            mainPage.BrowseWaitVisible();
            var schedulePage = mainPage.GoToSchedule();
            var resultPage = schedulePage.SearchTicket(from, to, targetDate);
            resultPage.WaitTitle(resultHeader);
            //Сохранить данные о самом раннем рейсе,
            //
            //который отправляется не ранее 12:00
            //
            //и билет на который стоит не более 200 р.
            var target = resultPage.ResultRows.Where(i => i.Price <= 200 && i.Departure.Hour < 12)
                .OrderBy(i => i.Position).FirstOrDefault();
            if (target != null)
            {
                resultPage.ChangeСurrency(ScheduleResultPage.Сurrency.Ru);
                var ruPrice = target.Price;
                resultPage.ChangeСurrency(ScheduleResultPage.Сurrency.Usd);
                var usdPrice = target.Price;
                Console.WriteLine($"Output {target.Departure} {ruPrice} {usdPrice}");
            }
            else
            {
                Console.WriteLine($"Output no result");
            }
            ////Открыть страницу информации о рейсе.

            //8.Проверить, что данные о рейсе на странице информации соответствуют данным из пункта 5, а именно:

            //*Название таблицы

            //    * Время и пункт отправления

            //    * Время и пункт прибытия

            //    * Время в пути
        }
    }
}