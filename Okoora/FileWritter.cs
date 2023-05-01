using System.IO;

namespace Okoora
{
    public class FileWritter
    {
        public FileWritter()
        {
        }

        public string WriteResultToFile(string data)
        {
            string directoryPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string fileNamePath = Path.Combine(directoryPath, "CurrenciesRates.txt");

            using (StreamWriter writer = new StreamWriter(fileNamePath))
            {
                writer.Write(data);
            }

            return fileNamePath;
        }
    }
}