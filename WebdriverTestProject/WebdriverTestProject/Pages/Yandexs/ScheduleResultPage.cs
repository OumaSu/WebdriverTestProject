using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using WebdriverTestProject.Controls;
using WebdriverTestProject.Controls.Yandexs;
using WebdriverTestProject.Helpers;
using WebdriverTestProject.WebDriverCore;

namespace WebdriverTestProject.Pages.Yandexs
{
    public class ScheduleResultPage : SchedulePage
    {
        public enum Сurrency
        {
            Ru,
            Usd
        }

        public StaticControl Title => new StaticControl(By.XPath("//header/span[1]/h1/span"));
        public StaticControl CurrencyControlButton =>
            new StaticControl(By.XPath("//*[@id='root']/div/main/div/div[1]/div[1]/div/ul/li[3]/div/button"));

        public StaticControl CurrencyRubControl => new StaticControl(By.XPath("//*[@data-value='RUB']"));
        public StaticControl CurrencyUsdControl => new StaticControl(By.XPath("//*[@data-value='USD']"));

        public IEnumerable<ResultRow> ResultRows => GetRows();

        public void WaitTitle(string expectedTitle)
        {
            Waiter.Wait(() => Title.GetText() == expectedTitle,
                $"Expect {expectedTitle} text, but was {Title.GetText()}");
        }

        public int ResultCount()
        {
            var allResult = WebDriver.Driver
                .FindElements(By.XPath(
                    "//*[@class='SearchSegment SearchSegment_isNotInterval SearchSegment_isNotGone SearchSegment_isVisible']"))
                .Count;
            return allResult > 30 ? 30 : allResult;
        }

        public void ChangeСurrency(Сurrency currency)
        {
            CurrencyControlButton.Click();
            switch (currency)
            {
                case Сurrency.Ru:
                {
                    CurrencyRubControl.WaitVisibleWithRetries(defaultTimeout);
                    CurrencyRubControl.Click();
                    CurrencyRubControl.WaitInvisibleWithRetries(defaultTimeout);
                    break;
                }
                case Сurrency.Usd:
                {
                    CurrencyUsdControl.WaitVisibleWithRetries(defaultTimeout);
                    CurrencyUsdControl.Click();
                    CurrencyUsdControl.WaitInvisibleWithRetries(defaultTimeout);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(currency), currency, null);
            }
        }

        private IEnumerable<ResultRow> GetRows()
        {
            for (var i = 1; i <= ResultCount(); i++)
                yield return new ResultRow(i);
        }

        public RacePage LinkToRace(ResultRow resultRow)
        {
            try
            {
                if (!resultRow.Link.IsPresent)
                {
                    return null;
                }
                resultRow.Link.Click();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка в процессе клика по ссылке: {ex.Message}");
                return null;
            }
            return ChangePageType<RacePage>();
        }

        public override void BrowseWaitVisible()
        {
            Title.WaitVisibleWithRetries(defaultTimeout);
        }
    }
}