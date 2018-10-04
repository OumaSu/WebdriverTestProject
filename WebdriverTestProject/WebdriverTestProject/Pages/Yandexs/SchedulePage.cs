using System;
using System.Globalization;
using OpenQA.Selenium;
using WebdriverTestProject.Controls;
using WebdriverTestProject.Controls.Yandexs;

namespace WebdriverTestProject.Pages.Yandexs
{
    public class SchedulePage : PageBase
    {
        public Input From => new Input(By.XPath("//*[@name='fromName']"));
        public Input To => new Input(By.XPath("//*[@name='toName']"));
        public Input Date => new Input(By.XPath("//*[@class='date-input_search__input']"));
        public StaticControl SearchButton => new StaticControl(By.XPath("//*[@class='y-button_islet-rasp-search _pin-left _init']"));

        public ScheduleResultPage SearchTicket(string from, string to, DateTime date)
        {
            From.SetValueWithCheck(from);
            To.SetValueWithCheck(to);
            Date.SetValue(date.ToString("dd'.'MM'.'yyyy", CultureInfo.InvariantCulture));
            SearchButton.Click();
            return ChangePageType<ScheduleResultPage>();
        }

        public override void BrowseWaitVisible()
        {
            From.WaitVisibleWithRetries(defaultTimeout);
            To.WaitVisibleWithRetries(defaultTimeout);
            SearchButton.WaitVisibleWithRetries(defaultTimeout);
        }
    }
}