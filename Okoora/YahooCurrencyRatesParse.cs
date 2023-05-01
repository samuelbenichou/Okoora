using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Okoora
{
    public class YahooCurrencyRatesParse :  ICurrencyRatesParse
    {
        List<string> rates = new List<string>() {"EUR/USD", "EUR/JPY", "EUR/GBP"};
        private WebClient webClient;

        public YahooCurrencyRatesParse()
        {
            webClient = new WebClient();
        }

        public string GetCurrencyRates()
        {
            // Load the HTML content from the URL
            string page = webClient.DownloadString("https://finance.yahoo.com/currencies");
            
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(page);
            
            List<List<string>> table = doc.DocumentNode.SelectSingleNode("//table[@class='W(100%)']")
                .Descendants("tr")
                .Skip(1)
                .Where(tr=>tr.Elements("td").Count()>1)
                .Select(tr => tr.Elements("td").Select(td => td.InnerText.Trim()).ToList())
                .ToList();
            
            string result = "";
            
            foreach (var rate in rates)
            {
                var value = table.First(x => x[1] == rate);
                if (value[1] == "EUR/GBP")
                {
                    value[1] = "GBP/EUR";
                    value[2] = (1 / Double.Parse(value[2])).ToString().Substring(0,6);
                }
            
                result += "Name: " + value[1] + ", Rate: " + value[2] + ", Date: " + DateTime.Now + Environment.NewLine;
            }
            
            result += "Name: USD/ILS, Rate: " + GetIlsUsdRate() + ", Date: " + DateTime.Now + Environment.NewLine;
            
            return result;
        }

        public string GetIlsUsdRate()
        {
            string page = webClient.DownloadString("https://finance.yahoo.com/quote/USDILS%3DX?p=USDILS%3DX");
            
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(page);
            
            var ilsUdsRate = doc.DocumentNode.SelectSingleNode("//fin-streamer[@class='Fw(b) Fz(36px) Mb(-4px) D(ib)']").InnerHtml.Trim();

            return ilsUdsRate;
        }
    }
}