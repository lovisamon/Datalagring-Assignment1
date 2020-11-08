using App.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Storage;
using Windows.Storage.Pickers.Provider;

namespace App.Services
{
    public static class WriteService
    {
        public static void WriteToFile(StorageFile file, Person person)
        {
            switch (file.FileType)
            {
                case ".json":
                    WriteToJsonFile(file, person);
                    break;
                case ".csv":
                    WriteToCsvFile(file, person);
                    break;
                case ".xml":
                   WriteToXmlFile(file, person);
                    break;
                case ".txt":
                    WriteToTxtFile(file, person);
                    break;
                default:
                    break;
            }
        }

        public static async void WriteToJsonFile(StorageFile file, Person person)
        {
            await FileIO.WriteTextAsync(file, JsonConvert.SerializeObject(person));
        }

        public static async void WriteToCsvFile(StorageFile file, Person person)
        {
            List<string> lines = new List<string>()
            {
                "FirstName;LastName;Age;City",
                $"{person.FirstName};{person.LastName};{person.Age};{person.City}"
            };

            await FileIO.WriteLinesAsync(file, lines);
        }

        public static async void WriteToXmlFile(StorageFile file, Person person)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Person));
            Stream stream = await file.OpenStreamForWriteAsync();
            serializer.Serialize(stream, person);

            await stream.FlushAsync();
            stream.Close();
            stream.Dispose();
        }

        public static async void WriteToTxtFile(StorageFile file, Person person)
        {
            await FileIO.WriteTextAsync(file, $"{person.FirstName},{person.LastName},{person.Age},{person.City}");
        }
    }
}
