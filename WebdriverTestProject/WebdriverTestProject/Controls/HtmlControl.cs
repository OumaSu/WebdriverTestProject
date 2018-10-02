using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;

namespace WebdriverTestProject.Controls
{
    public delegate TValue FuncParams<TValue, in TArgs>(TValue s, params TArgs[] p);

    public abstract class HtmlControl
    {
        private readonly string controlDescription;
        public readonly WebElementWrapper element;
        private readonly By locator;
        private readonly ISearchContext searchContext;

        public Func<string, string, bool> @equals = (x, y) => string.Equals(x, y);
        public FuncParams<string, object> format = (s, p) => string.Format(s, p);

        protected HtmlControl(By locator, HtmlControl container = null)
        {
            this.locator = locator;
            controlDescription = FormatControlDescription(locator.ToString(), container);
            searchContext = container != null
                ? container.element
                : WebDriver.GetSearchContext();
            element = new WebElementWrapper(searchContext, locator, controlDescription);
        }

        protected HtmlControl(string idLocator, HtmlControl container = null)
            : this(By.Id(idLocator), container)
        {
        }

        public virtual bool IsEnabled => !HasClass("disabled");

        public bool IsPresent => searchContext.FindElements(locator).Count > 0;

        public virtual bool IsVisible => element.Displayed;

        public bool IsEmpty => GetText() == string.Empty;

        public void WaitVisible() => Assert.IsTrue(IsVisible, FormatWithLocator("Ожидание видимости элемента"));

        public void WaitEmptyWithRetries(int? timeout = null)
        {
            var actionDescription = FormatWithLocator("Ожидание отсутствия текста");
            Waiter.Wait(() => IsEmpty, actionDescription, timeout);
        }
        public void WaitVisibleWithRetries(int? timeout = null)
        {
            Waiter.Wait(() => IsVisible, FormatWithLocator("Ожидание видимости элемента"), timeout);
        }

        public void WaitInvisible()
        {
            Assert.IsFalse(IsVisible, FormatWithLocator("Ожидание невидимости элемента"));
        }

        public void WaitInvisibleWithRetries(int? timeout = null)
        {
            Waiter.Wait(() => !IsVisible, FormatWithLocator("Ожидание невидимости элемента"), timeout);
        }

        public void WaitPresence()
        {
            Assert.IsTrue(IsPresent, FormatWithLocator("Ожидание присутствия элемента"));
        }

        public void WaitPresenceWithRetries(int? timeout = null)
        {
            Waiter.Wait(() => IsPresent, FormatWithLocator("Ожидание присутствия элемента"), timeout);
        }

        public bool CheckPresenceWithRetries()
        {
            var result = true;
            Waiter.DoWait(() => IsPresent, () => result = false, new int?());
            return result;
        }

        public void WaitAbsence()
        {
            Assert.IsFalse(IsPresent, FormatWithLocator("Ожидание отсутствия элемента"));
        }

        public void WaitAbsenceWithRetries(int? timeout = null)
        {
            Waiter.Wait(() => !IsPresent, FormatWithLocator("Ожидание отсутствия элемента"), timeout);
        }

        public void WaitEnabled()
        {
            Assert.IsTrue(IsEnabled, FormatWithLocator("Ожидание доступности элемента"));
        }

        public void WaitEnabledWithRetries(int? timeout = null)
        {
            Waiter.Wait(() => IsEnabled, FormatWithLocator("Ожидание доступности элемента"), timeout);
        }

        public void WaitDisabled()
        {
            Assert.IsFalse(IsEnabled, FormatWithLocator("Ожидание недоступности элемента"));
        }

        public void WaitDisabledWithRetries(int? timeout = null)
        {
            Waiter.Wait(() => !IsEnabled, FormatWithLocator("Ожидание недоступности элемента"), timeout);
        }

        public void WaitText(string expectedText, int? timeout = null)
        {
            var actionDescription =
                FormatWithLocator($"Ожидание появления текста '{expectedText}' в элементе");
            Waiter.Wait(() => GetText() == expectedText, actionDescription, timeout);
        }

        public void WaitTextStartsWith(string expectedText)
        {
            var message =
                FormatWithLocator($"Ожидание текста '{expectedText}' в начале текста элемента");
            StringAssert.StartsWith(expectedText, GetText(), message);
        }

        public void WaitTextContains(string expectedText)
        {
            var message = FormatWithLocator($"Ожидание текста '{expectedText}' внутри текста элемента");
            StringAssert.Contains(expectedText, GetText(), message);
        }

        public void WaitTextContainsWithRetries(string expectedText, int? timeout = null)
        {
            var actionDescription =
                FormatWithLocator($"Ожидание текста '{expectedText}' внутри текста элемента");
            Waiter.Wait(() => GetText().Contains(expectedText), actionDescription, timeout);
        }

        public void WaitClassPresence(string className)
        {
            var str = FormatWithLocator($"Ожидание класса '{className}' у элемента");
            string actualClasses;
            Assert.IsTrue(HasClass(className, out actualClasses),
                $"{str}, актуальный класс: '{actualClasses}'");
        }

        public void WaitClassPresenceWithRetries(string className, int? timeout = null)
        {
            var actionDescription =
                FormatWithLocator($"Ожидание появления класса '{className}' у элемента");
            Waiter.Wait(() => HasClass(className), actionDescription, timeout);
        }

        public void WaitClassAbsence(string className)
        {
            var str = FormatWithLocator($"Ожидание отсутствия класса '{className}' у элемента");
            string actualClasses;
            Assert.IsFalse(HasClass(className, out actualClasses),
                $"{str}, актуальный класс: '{actualClasses}'");
        }

        public void WaitClassAbsenceWithRetries(string className, int? timeout = null)
        {
            var actionDescription =
                FormatWithLocator(string.Format("Ожидание отсутствия класса '{0}' у элемента", className));
            Waiter.Wait(() => !HasClass(className), actionDescription, timeout);
        }

        public void WaitAttributeValue(string attributeName, string expectedText)
        {
            var message =
                FormatWithLocator(string.Format("Ожидание атрибута '{0}' со значением '{1}' в элементе", attributeName,
                    expectedText));
            Assert.AreEqual(expectedText, GetAttributeValue(attributeName), message);
        }

        public void WaitAttributeValueWithRetries(string attributeName, string expectedText, int? timeout = null)
        {
            var actionDescription =
                FormatWithLocator(string.Format("Ожидание атрибута '{0}' со значением '{1}' в элементе", attributeName,
                    expectedText));
            Waiter.Wait(() => expectedText == GetAttributeValue(attributeName), actionDescription,
                timeout);
        }

        public virtual void Click()
        {
            element.Click();
        }

        public virtual void DoubleClick()
        {
            element.DoubleClick();
        }

        public virtual void ContextClick()
        {
            element.ContextClick();
        }

        public virtual void DragAndDrop(HtmlControl target)
        {
            element.DragAndDrop(target.element);
        }

        public void ScrollDown()
        {
            element.ScrollDown();
        }

        public void ClickLeftUp()
        {
            element.ClickLeftUp();
        }

        public void KeysDown(string text)
        {
            element.KeysDown(text);
        }

        public void KeysUp(string text)
        {
            element.KeysUp(text);
        }

        public void Mouseover()
        {
            element.Mouseover();
        }

        public void MouseoverAndWait(int waitTimeout = 1000)
        {
            element.Mouseover();
            Thread.Sleep(waitTimeout);
        }

        public void SendKeysToBody(string text)
        {
            element.SendKeysToBody(text);
        }

        public virtual string GetText()
        {
            return element.Text.Trim();
        }

        public virtual object GetValue()
        {
            return GetText();
        }

        public bool HasAttribute(string attributeName)
        {
            return GetAttributeValue(attributeName) != null;
        }

        public string GetAttributeValue(string attributeName)
        {
            return element.GetAttribute(attributeName);
        }

        public void ClearBlock(string elementId)
        {
            WebDriver.ExecuteScript(string.Format("$('#{0}').html('');", elementId));
        }

        protected string FormatWithLocator(string text)
        {
            return string.Format("{0} '{1}'", text, controlDescription);
        }

        public string GetParameterValue(string value)
        {
            return WebDriver.ExecuteScript(value.Split(new[]
            {
                '.'
            })
                .Aggregate($"return Configs['{GetAttributeValue("id")}']",
                    (current, part) => current + $"['{part}']"), new object[0]) as string;
        }

        private bool HasClass(string className, out string actualClasses)
        {
            actualClasses = GetAttributeValue("class");
            var strArray = actualClasses.Split(new[]
            {
                " ",
                "\r",
                "\n",
                "\t"
            }, StringSplitOptions.RemoveEmptyEntries);
            return className.Split(new[]
            {
                " ",
                "\r",
                "\n",
                "\t"
            }, StringSplitOptions.RemoveEmptyEntries).All((new List<string>(strArray)).Contains<string>);
        }

        public void SendKeys(string keys)
        {
            element.SendKeys(keys);
        }

        public void WaitPossibleVisibleWithRetries(int timeout = 1000)
        {
            Waiter.DoWait(() => IsVisible,
                () => Console.WriteLine("Ожидание видимости элемента не выполнилось за {0} мс", timeout), timeout);
        }

        public void WaitPossibleInvisibleWithRetries(int timeout = 1000)
        {
            Waiter.DoWait(() => !IsVisible,
                () => Console.WriteLine("Ожидание невидимости элемента не выполнилось за {0} мс", timeout), timeout);
        }

        public bool ClassContains(string text)
        {
            var actualClasses = GetAttributeValue("class");
            var strArray = actualClasses.Split(new[]
            {
                " ",
                "\r",
                "\n",
                "\t"
            }, StringSplitOptions.RemoveEmptyEntries);

            return strArray.Any(s => s.IndexOf(text, StringComparison.OrdinalIgnoreCase) != -1);
        }

        public void WaitClassContainsWithRetries(string text, int? timeout = null)
        {
            var actionDescription =
                FormatWithLocator(string.Format("Ожидание появления текста в названии класса '{0}' у элемента", text));
            Waiter.Wait(() => ClassContains(text), actionDescription, timeout);
        }

        public bool IsValid(out string errorText)
        {
            errorText = null;
            var errorImage = new StaticControl(element.GetAttribute("id") + "_EI");
            if (errorImage.IsVisible)
            {
                errorText = errorImage.GetAttributeValue("title");
                return false;
            }
            return true;
        }
    }
}