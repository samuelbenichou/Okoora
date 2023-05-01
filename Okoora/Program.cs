namespace Okoora
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            YahooCurrencyRatesParse yahoo = new YahooCurrencyRatesParse();
            string res = yahoo.GetCurrencyRates();

            // Bonus
            // XECurrencyRatesParse xe = new XECurrencyRatesParse();
            // string res = xe.GetCurrencyRates();

            FileWritter fileWritter = new FileWritter();
            string fileName = fileWritter.WriteResultToFile(res);

            FileReader fileReader = new FileReader();
            fileReader.ReadDataFromFile(fileName);
        }
    }
}