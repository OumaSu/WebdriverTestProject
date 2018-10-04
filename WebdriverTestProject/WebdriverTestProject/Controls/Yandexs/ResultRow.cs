using System;
using System.Globalization;
using System.Text.RegularExpressions;
using OpenQA.Selenium;

namespace WebdriverTestProject.Controls.Yandexs
{
    public class ResultRow
    {
        private readonly string dateSubxpath = "/div[1]/div[1]/div[1]/span";
        private readonly string linkToRaceSubxpath = "/header/div/h3/a/span[2]";
        private readonly string mainXpath = "//*[@id='root']/div/main/div/div[1]/div[1]/div/section/article";
        private readonly string priceSelectSubxpath = "/div[2]/div/div/button/span/span[1]/span";
        private readonly string priceTextSubxpath = "/div[2]/div/ul/li/a/span[2]";

        public ResultRow(int index)
        {
            Position = index;
            Link = new StaticControl(By.XPath(mainXpath + $"[{index}]" + linkToRaceSubxpath));
            var depTemp = new StaticControl(By.XPath(mainXpath + $"[{index}]" + dateSubxpath));
            if (!depTemp.IsPresent) depTemp.ScrollDown();
            Departure = DateTime.Parse(depTemp.element.Text);
        }

        public int Position { get; }
        public DateTime Departure { get; }
        public StaticControl Link { get; }
        public double Price => double.Parse(GetPrice(Position), CultureInfo.InvariantCulture);

        private string GetPrice(int index)
        {
            var priceSelect = new StaticControl(By.XPath(mainXpath + $"[{index}]" + priceSelectSubxpath));
            var priceText = new StaticControl(By.XPath(mainXpath + $"[{index}]" + priceTextSubxpath));
            var result = priceSelect.IsPresent ? priceSelect.element.Text : priceText.element.Text;
            return Regex.Replace(result, "[^0-9.]", "");
        }
    }
}