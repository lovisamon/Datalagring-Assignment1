using App.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Windows.Storage;
using Windows.UI.Xaml.Shapes;

namespace App.Services
{
    public static class ReadService
    {
        public static ObservableCollection<Person> personList = new ObservableCollection<Person>();

        public static async void ReadFromFile(StorageFile file)
        {
            string data = await FileIO.ReadTextAsync(file);
            personList.Clear();

            switch (file.FileType)
            {
                case ".json":
                    ReadFromJsonFile(data);
                    break;
                case ".csv":
                    ReadFromCsvFile(data);
                    break;
                case ".xml":
                    ReadFromXmlFile(data);
                    break;
                case ".txt":
                    ReadFromTxtFile(data);
                    break;
                default:
                    break;
            }
        }

        public static void ReadFromJsonFile(string data)
        {
            try
            {
                Person person = JsonConvert.DeserializeObject<Person>(data); // A single person
                personList.Add(person);
            }
            catch
            {
                List<Person> jsonList = JsonConvert.DeserializeObject<List<Person>>(data); // Multiple persons
                foreach (Person person in jsonList)
                {
                    personList.Add(person);
                }
            }
        }

        public static void ReadFromCsvFile(string data)
        {
            string[] lines = data.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            bool isHeader = true; // Assume first line is the header
            foreach (string line in lines)
            {

                if (isHeader || line == "")
                {
                    isHeader = false;
                    continue;
                }

                string[] fields = line.Split(';');
                personList.Add(new Person(fields[0], fields[1], Int32.Parse(fields[2]), fields[3]));
            }
        }

        public static void ReadFromXmlFile(string data)
        {
            XmlReader xmlReader = XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes(data)));

            if (xmlReader.IsStartElement("person")) // A single person
            {   
                XmlSerializer serializer = new XmlSerializer(typeof(Person));
                Person person = (Person)serializer.Deserialize(xmlReader);
                personList.Add(person);
            }
            if (xmlReader.IsStartElement("group")) // Multiple persons
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Group));
                Group group = (Group)serializer.Deserialize(xmlReader);
                foreach (Person person in group.Persons)
                {
                    personList.Add(person);
                }
            }

            xmlReader.Close();
            xmlReader.Dispose();
        }

        public static void ReadFromTxtFile(string data)
        {
            string[] lines = data.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            foreach (string line in lines)
            {
                if (line == "") continue;
                string[] personData = line.Split(",");
                personList.Add(new Person(personData[0], personData[1], Int32.Parse(personData[2]), personData[3])); ;
            }
        }
    }
}
