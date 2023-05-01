using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Okoora
{
    public class XECurrencyRatesParse
    {
        List<string> rates = new List<string>() {"EUR / USD", "EUR / JPY", "GBP / EUR"};
        private WebClient webClient;

        public XECurrencyRatesParse()
        {
            webClient = new WebClient();
        }

        public string GetCurrencyRates()
        {
            // Load the HTML content from the URL
            string page = webClient.DownloadString("https://www.xe.com/currencycharts/");

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(page);
            List<List<string>> table = doc.DocumentNode
                .SelectSingleNode("//table[@class='table__TableBase-sc-1j0jd5l-0 pTERB']")
                .Descendants("tr")
                .Skip(1)
                .Where(tr => tr.Elements("td").Count() > 1)
                .Select(tr => tr.Elements("td").Select(td => td.InnerText.Trim()).ToList())
                .ToList();


            string result = "";
            foreach (var rate in rates)
            {
                var value = table.First(x => x[0] == rate);
                result += "Name: " + value[0] + ", Rate: " + value[1] + ", Date: " + DateTime.Now + Environment.NewLine;
            }

            result += "Name: USD / ILS, Rate: " + GetIlsUsdRate() + ", Date: " +
                      DateTime.Now;

            return result;
        }

        public string GetIlsUsdRate()
        {
            string page =
                webClient.DownloadString("https://www.xe.com/currencyconverter/convert/?Amount=1&From=USD&To=ILS");

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(page);

            var ilsUdsRate = doc.DocumentNode.SelectSingleNode("//p[@class='result__BigRate-sc-1bsijpp-1 iGrAod']");
            ilsUdsRate.ParentNode.RemoveChild(ilsUdsRate, false);
            
            return ilsUdsRate.InnerText.Trim().Substring(0, 7);
        }
    }
}