﻿using OpenQA.Selenium;

namespace WebdriverTestProject.Controls.Yandexs
{
    public class MoreControl : StaticControl
    {
        public MoreControl(By locator, HtmlControl container = null) : base(locator, container)
        {
        }

        public MoreControl(string idLocator, HtmlControl container = null) : base(idLocator, container)
        {
        }
    }
}