using System.IO;
using System;

namespace Okoora
{
    public class FileReader
    {
        public FileReader()
        {
        }

        public void ReadDataFromFile(string filePath)
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
            
                while ((line = reader.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
            }
        }
    }
}