using System;
using System.Collections.ObjectModel;
using System.Drawing;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using WebdriverTestProject.WebDriverCore;

namespace WebdriverTestProject.Controls
{
    public class WebElementWrapper : IWebElement
    {
        private readonly string description;
        private readonly By locator;
        private readonly ISearchContext searchContext;
        private IWebElement nativeWebElement;

        public WebElementWrapper(ISearchContext searchContext, By locator, string description)
        {
            this.searchContext = searchContext;
            this.locator = locator;
            this.description = description;
        }

        public string TagName => Execute(() => FindNativeWebElement().TagName);

        public string Text => Execute(() => FindNativeWebElement().Text);

        public bool Enabled => Execute(() => FindNativeWebElement().Enabled);

        public bool Selected => Execute(() => FindNativeWebElement().Selected);

        public Point Location => Execute(() => FindNativeWebElement().Location);

        public Size Size => Execute(() => FindNativeWebElement().Size);

        public bool Displayed => Execute(() => FindNativeWebElement().Displayed);

        public IWebElement FindElement(By by)
        {
            throw new NotSupportedException();
        }

        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            return Execute(() => FindNativeWebElement()
                .FindElements(by));
        }

        public void Clear()
        {
            Execute(() =>
            {
                FindNativeWebElement().Clear();
                return 0;
            });
        }

        public void SendKeys(string text)
        {
            Execute(() =>
            {
                FindNativeWebElement().SendKeys(text);
                return 0;
            });
        }

        public void Submit()
        {
            Execute(() =>
            {
                FindNativeWebElement().Submit();
                return 0;
            });
        }

        public void Click()
        {
            var element = FindNativeWebElement();

            if (WebDriver.Driver != null)
            {
                var actions = new Actions(WebDriver.Driver);
                actions.MoveToElement(element).Click().Perform();
            }
        }

        public string GetAttribute(string attributeName)
        {
            return Execute(() => FindNativeWebElement()
                .GetAttribute(attributeName));
        }

        public string GetProperty(string propertyName)
        {
            return Execute(
                () => FindNativeWebElement().GetProperty(propertyName));
        }

        public string GetCssValue(string propertyName)
        {
            return Execute(
                () => FindNativeWebElement().GetCssValue(propertyName));
        }

        public void DoubleClick()
        {
            Execute(() =>
            {
                var element = FindNativeWebElement();
                if (WebDriver.Driver != null)
                {
                    var action = new Actions(WebDriver.Driver);
                    action.MoveToElement(element).DoubleClick().Perform();
                }

                return 0;
            });
        }

        public void ContextClick()
        {
            Execute(() =>
            {
                var element = FindNativeWebElement();
                if (WebDriver.Driver != null)
                {
                    var action = new Actions(WebDriver.Driver);
                    action.MoveToElement(element).ContextClick().Perform();
                }

                return 0;
            });
        }

        public void DragAndDrop(WebElementWrapper target)
        {
            Execute(() =>
            {
                var sourceElement = FindNativeWebElement();
                var targetElement = target.FindNativeWebElementInternal();
                if (WebDriver.Driver != null)
                {
                    var action = new Actions(WebDriver.Driver);
                    action.MoveToElement(sourceElement).DragAndDrop(sourceElement, targetElement).Perform();
                }

                return 0;
            });
        }

        public void KeysDown(string text)
        {
            Execute(() =>
            {
                var element = FindNativeWebElement();
                if (WebDriver.Driver != null)
                {
                    var action = new Actions(WebDriver.Driver);
                    action.MoveToElement(element).KeyDown(text).Perform();
                }

                return 0;
            });
        }

        public void KeysUp(string text)
        {
            Execute(() =>
            {
                var element = FindNativeWebElement();
                if (WebDriver.Driver != null)
                {
                    var action = new Actions(WebDriver.Driver);
                    action.MoveToElement(element).KeyUp(text).Perform();
                }

                return 0;
            });
        }

        public void ClickViaJavascript()
        {
            Execute(() =>
            {
                WebDriver.ExecuteScript("arguments[0].click();", FindNativeWebElement());
                return 0;
            });
        }

        public void ClickLeftUp()
        {
            Execute(() =>
            {
                new Actions((IWebDriver) GetRootSearchContext()).MoveToElement(FindNativeWebElement(), 0, 0)
                    .Click()
                    .Perform();
                return 0;
            });
        }

        public void Mouseover()
        {
            Execute(() =>
            {
                new Actions((IWebDriver) GetRootSearchContext()).MoveToElement(FindNativeWebElement()).Perform();
                return 0;
            });
        }

        public void SendKeysToBody(string text)
        {
            GetRootSearchContext().FindElement(By.TagName("body"))
                .SendKeys(text);
        }

        public void ScrollDown()
        {
            Execute(() =>
            {
                var element = FindNativeWebElement();
                WebDriver.ExecuteScript("arguments[0].scrollIntoView();", element);
                return 0;
            });
        }

        private static void BlurCurrentActiveElement()
        {
            WebDriver.ExecuteScript(
                "if (document.activeElement != null) {document.activeElement.blur();}");
        }

        private IWebElement FindNativeWebElement()
        {
            return nativeWebElement ??
                   (nativeWebElement = FindNativeWebElementInternal());
        }

        private IWebElement FindNativeWebElementInternal()
        {
            var searchCtx = searchContext;
            if (searchCtx is WebElementWrapper webElementWrapper)
                searchCtx = webElementWrapper.FindNativeWebElementInternal();
            return searchCtx.FindElement(locator);
        }

        private ISearchContext GetRootSearchContext()
        {
            if (searchContext is WebElementWrapper webElementWrapper)
                return webElementWrapper.GetRootSearchContext();
            return searchContext;
        }

        private T Execute<T>(Func<T> func)
        {
            for (var index = 5; index >= 0; --index)
                try
                {
                    return func();
                }
                catch (InvalidElementStateException)
                {
                    ClearCache();
                }
                catch (StaleElementReferenceException)
                {
                    ClearCache();
                }
                catch (NoSuchElementException)
                {
                    throw new NoSuchElementException($"Не найден элемент '{description}'");
                }
                catch (InvalidOperationException ex)
                {
                    if (ex.Message.IndexOf("Element is not clickable at point", StringComparison.OrdinalIgnoreCase) ==
                        -1)
                        throw;
                    TryFixPage();
                }

            return func();
        }

        private void TryFixPage()
        {
            ScrollDown();
            BlurCurrentActiveElement();
        }

        private void ClearCache()
        {
            nativeWebElement = null;
        }
    }
}