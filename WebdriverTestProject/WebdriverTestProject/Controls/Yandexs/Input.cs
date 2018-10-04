using NUnit.Framework;
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

        public override string GetText()
        {
            return GetAttributeValue("value");
        }

        public void Clear()
        {
            var isEmpty = false;

            for (var i = 0; i < 5 && !isEmpty; i++)
            {
                for (var j = GetText().Length; j >= 0; j--)
                {
                    element.SendKeys(Keys.Backspace);
                }
                element.SendKeys(Keys.Tab);
                isEmpty = GetText() == string.Empty;
            }
            Assert.IsTrue(isEmpty);
        }


        public void WaitText(string expectedText)
        {
            Waiter.Wait(()=>this.GetText()==expectedText, $"Ожидался текст {expectedText}, но был {this.GetText()}");
        }

        public void SetValue(string value)
        {
            this.SendKeys(value);
        }

        public void SetValueAfterClear(string value)
        {
            Clear();
            SetValue(value);
        }

        public void SetValueWithCheck(string value)
        {
            SetValueAfterClear(value);
            WaitText(value);
        }
    }
}
