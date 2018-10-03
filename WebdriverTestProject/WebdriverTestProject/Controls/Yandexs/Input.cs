using OpenQA.Selenium;
using WebdriverTestProject.Helpers;

namespace WebdriverTestProject.Controls.Yandexs
{
    public class Input : StaticControl
    {
        public Input(By locator, HtmlControl container = null) : base(locator, container)
        {
        }

        public Input(string idLocator, HtmlControl container = null) : base(idLocator, container)
        {
        }

        public void WaitText(string expectedText)
        {
            Waiter.Wait(()=>this.GetText()==expectedText, $"Ожидался текст {expectedText}, но был {this.GetText()}");
        }

        public void SetValue(string value)
        {
            this.Click();
            this.SendKeys(value);
        }

        public void SetValueWithCheck(string value)
        {
            SetValue(value);
            WaitText(value);
        }
    }
}
